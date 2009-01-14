using System;
using System.Linq;
using Quiksoft.EasyMail.SMTP;
using Isis.ExtensionMethods.Web;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Isis.ExtensionMethods.Linq;
using Zeus.Admin;
using Bermedia.Gibbons.Web.Items;
using System.Web;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.Plugins.Newsletters
{
	[Zeus.Admin.GridViewPlugin(typeof(Newsletter), "Run", "~/Plugins/Newsletters/Send.aspx?selected={selected}", AdminTargetFrame.Preview, "Run", 100, Expression = "it.Status == \"NotStarted\"")]
	[Zeus.Admin.GridViewPlugin(typeof(Newsletter), "ViewResults", "~/Plugins/Newsletters/Send.aspx?selected={selected}&started=true", AdminTargetFrame.Preview, "View Results", 100, Expression = "it.Status == \"Failed\" || it.Status == \"Successful\"")]
	[Zeus.Admin.GridViewPlugin(typeof(Newsletter), "ViewProgress", "~/Plugins/Newsletters/Send.aspx?selected={selected}&started=true", AdminTargetFrame.Preview, "View Progress", 100, Expression = "it.Status == \"InProgress\"")]
	[Zeus.Admin.GridViewPlugin(typeof(Newsletter), "Reset", "~/Plugins/Newsletters/Reset.aspx?selected={selected}", AdminTargetFrame.Preview, "Reset", 110, Expression = "it.Status == \"Failed\" || it.Status == \"Successful\"")]
	//[Zeus.Admin.GridViewPlugin(typeof(Newsletter), "Edit", "~/Plugins/Newsletters/Reset.aspx?selected={selected}", AdminTargetFrame.Preview, "Edit", 110, Expression = "it.Status == \"NotStarted\"")]
	public partial class Send : AdminPage
	{
		protected Newsletter SelectedNewsletter
		{
			get { return this.SelectedItem as Newsletter; }
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			cdsNewsletterLog.CurrentItem = this.SelectedNewsletter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			MailoutManager.SetLicenseKey();

			if (Request.GetOptionalString("started") == "true")
				if (this.SelectedNewsletter.Status != NewsletterStatus.Successful && this.SelectedNewsletter.Status != NewsletterStatus.Failed)
					Response.AppendHeader("Refresh", "10");

			cdsNewsletterLog.WhereParameters.Add(new Parameter("NewsletterID", DbType.Int32, this.SelectedNewsletter.ID.ToString()));
		}

		private void StartMailout()
		{
			// Create and start a worker thread, to process the mailing
			_mailoutID = this.SelectedNewsletter.ID;

			this.SelectedNewsletter.Status = NewsletterStatus.InProgress;
			this.SelectedNewsletter.ErrorMessage = null;
			Zeus.Context.Persister.Save(this.SelectedNewsletter);

			ThreadStart threadStart = new ThreadStart(SendMailing);
			_workerThread = new Thread(threadStart);
			_workerThread.Start();

			Response.Redirect("send.aspx?selected=" + Server.UrlEncode(this.SelectedNewsletter.Path) + "&started=true");
		}

		protected void btnSend_Click(object sender, EventArgs e)
		{
			StartMailout();
		}

		protected void btnResume_Click(object sender, EventArgs e)
		{
			StartMailout();
		}

		#region Stuff done inside thread

		private static int _batchSize = Convert.ToInt32(ConfigurationManager.AppSettings["NewsletterBatchSize"]);

		private int _mailoutID;
		private Newsletter _mailout;
		private IDictionary<int, NewsletterLogEntry> _logEntries;
		private int _currentMessage;
		private Thread _workerThread;

		private void SendMailing()
		{
			_mailout = Zeus.Context.Persister.Get<Newsletter>(_mailoutID);

			EmailMessage email = MailoutManager.GetEmailMessage(_mailout);

			// This placeholder will be replaced with the email address from the database.
			email.Recipients.Add("$$EmailAddress$$");

			try
			{
				DataTable reportResults; NewsletterLogEntry[] logEntries;
				if (_mailout.LogEntries.Any())
				{
					// Filter report results to just those which haven't already been sent.
					logEntries = _mailout.LogEntries.ToArray();
					_logEntries = logEntries.ToDictionary(le => le.ID);

					reportResults = Zeus.Context.Current.Finder.OfType<NewsletterSubscription>()
						.ToList()
						.Where(ns => !ns.LogEntries.ToList().Any(nl => nl.Parent == _mailout && nl.Status != NewsletterLogEntryStatus.NotAttempted))
						.ToDataTable();

					_currentMessage = logEntries.Length - reportResults.Rows.Count + 1;
				}
				else
				{
					reportResults = Zeus.Context.Current.Finder.OfType<NewsletterSubscription>().ToDataTable();

					// Create log entries.
					List<NewsletterLogEntry> tempLogEntries = new List<NewsletterLogEntry>();
					foreach (NewsletterSubscription subscription in Zeus.Context.Current.Finder.OfType<NewsletterSubscription>())
					{
						NewsletterLogEntry logEntry = new NewsletterLogEntry { Newsletter = _mailout };
						logEntry.AddTo(subscription);
						Zeus.Context.Persister.Save(logEntry);

						tempLogEntries.Add(logEntry);
					}
					_mailout.TotalMessages = reportResults.Rows.Count;

					logEntries = tempLogEntries.ToArray();

					_logEntries = logEntries.ToDictionary(le => le.ID);
					_currentMessage = 1;
				}

				// Do the send.
				_mailout.Status = NewsletterStatus.InProgress;
				Zeus.Context.Persister.Save(_mailout);

				MailoutManager.Send(smtp =>
					{
						// Add the event to capture the status of the send.
						smtp.SMTPBulkDetails += new SMTPBulkStatusDetails(smtp_SMTPBulkDetails);
						smtp.SendBulkMerge(email, reportResults, 0, 0, "$$");
					});
				_mailout.Status = NewsletterStatus.Successful;
				Zeus.Context.Persister.Save(_mailout);
			}
			catch (Exception ex)
			{
				_mailout.Status = NewsletterStatus.Failed;
				_mailout.ErrorMessage = ex.ToString();
			}
			finally
			{
				_workerThread = null;
				Zeus.Context.Persister.Save(_mailout);
			}
		}

		/// <summary>
		/// Call back called by SubmitBulkMerge.
		/// </summary>
		/// <param name="currentRow">Current datarow of table.</param>
		/// <param name="success">True or False on error.</param>
		/// <param name="failException">The exception that caused the error if available.</param>
		private void smtp_SMTPBulkDetails(DataRow currentRow, bool success, Exception failException)
		{
			NewsletterLogEntry logEntry = _mailout.LogEntries.Single(nle => nle.Newsletter == _mailout && nle.Parent.ID == (int) currentRow["ID"]);

			if (success)
			{
				logEntry.Status = NewsletterLogEntryStatus.Queued;
			}
			else
			{
				logEntry.Status = NewsletterLogEntryStatus.Failed;
				logEntry.ErrorMessage = failException.Message;
			}

			if (_currentMessage % _batchSize == 0 || _currentMessage == _mailout.TotalMessages)
			{
				_mailout.CurrentMessage = _currentMessage;
				Zeus.Context.Persister.Save(logEntry);
				Zeus.Context.Persister.Save(_mailout);
			}

			++_currentMessage;
		}

		#endregion
	}
}