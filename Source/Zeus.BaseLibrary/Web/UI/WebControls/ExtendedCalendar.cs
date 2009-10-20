using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using Isis.Web.Configuration;

[assembly: WebResource("Isis.Web.UI.WebControls.Calendar-Arrow-Left.gif", "image/gif")]
[assembly: WebResource("Isis.Web.UI.WebControls.Calendar-Arrow-Right.gif", "image/gif")]
[assembly: WebResource("Isis.Web.UI.WebControls.ExtendedCalendar.css", "text/css")]
namespace Isis.Web.UI.WebControls
{
	[System.Web.UI.ValidationProperty("SelectedDate")]
	public class ExtendedCalendar : System.Web.UI.WebControls.Calendar
	{
		private bool _adminMode;

		public bool AdminMode
		{
			get { return _adminMode; }
			set { _adminMode = value; }
		}

		private ExtendedCalendarSection _configSection;
		private XPathDocument _eventsXml;
		private List<DateTime> _eventDates;

		private void ExtendedCalendar_DayRender(object sender, DayRenderEventArgs e)
		{
			if (_eventDates.Contains(e.Day.Date))
			{
				e.Cell.Font.Bold = true;

				e.Cell.Controls.Clear();
				HyperLink link = new HyperLink();
				link.Text = e.Day.DayNumberText;
				link.ToolTip = string.Format("{0} - {1} event(s) on this day",
					e.Day.Date.ToString("dd MMMM"),
					_eventDates.FindAll(delegate(DateTime date) { return (date == e.Day.Date); }).Count);
				link.NavigateUrl = e.SelectUrl;
				link.ForeColor = (e.Cell.ForeColor.IsEmpty) ? ((this.ForeColor.IsEmpty) ? Color.Black : this.ForeColor) : e.Cell.ForeColor;
				e.Cell.Controls.Add(link);
			}
		}

		private string GetNextPrevImageTag(bool next)
		{
			string imageFilename = (next) ? "Calendar-Arrow-Right.gif" : "Calendar-Arrow-Left.gif";
			string imageUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Isis.Web.UI.WebControls." + imageFilename);
			return "<img src=\"" + imageUrl + "\" border=\"0\" width=\"5\" height=\"9\" />";
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.SelectedDate = DateTime.Today;
			this.NextPrevFormat = NextPrevFormat.CustomText;
			this.PrevMonthText = GetNextPrevImageTag(false);
			this.NextMonthText = GetNextPrevImageTag(true);
			this.DayNameFormat = DayNameFormat.FirstTwoLetters;
			this.OtherMonthDayStyle.ForeColor = Color.Gray;
			this.DayStyle.CssClass = "calendar-day";
			this.DayRender += new DayRenderEventHandler(ExtendedCalendar_DayRender);

			System.Web.UI.HtmlControls.HtmlLink lCssLink = new System.Web.UI.HtmlControls.HtmlLink();
			lCssLink.Href = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Isis.Web.UI.WebControls.ExtendedCalendar.css");
			lCssLink.Attributes.Add("rel", "stylesheet");
			lCssLink.Attributes.Add("type", "text/css");
			this.Page.Header.Controls.Add(lCssLink);

			_configSection = (ExtendedCalendarSection) ConfigurationManager.GetSection("isis.web/extendedCalendar");

			_eventsXml = new XPathDocument(this.Page.Server.MapPath(_configSection.EventsXmlFile));

			// highlight days that have events
			XPathNavigator xmlNavigator = _eventsXml.CreateNavigator();
			XPathNodeIterator xmlIterator = xmlNavigator.Select("/ExtendedCalendar/Events/Event");
			_eventDates = new List<DateTime>();
			while (xmlIterator.MoveNext())
			{
				string startDateAttribute = xmlIterator.Current.GetAttribute("StartDate", string.Empty);
				string endDateAttribute = xmlIterator.Current.GetAttribute("EndDate", string.Empty);
				string visibleAttribute = xmlIterator.Current.GetAttribute("Visible", string.Empty);
				DateTime startDate = DateTime.Parse(startDateAttribute).Date;
				DateTime endDate = DateTime.Parse(endDateAttribute).Date;
				bool visible = Convert.ToBoolean(visibleAttribute);

				if (!visible && !_adminMode)
					continue;

				// add all dates in range
				DateTime currentDate = startDate;
				while (currentDate <= endDate)
				{
					_eventDates.Add(currentDate.Date);
					currentDate = currentDate.AddDays(1);
				}
			}
		}
	}
}
