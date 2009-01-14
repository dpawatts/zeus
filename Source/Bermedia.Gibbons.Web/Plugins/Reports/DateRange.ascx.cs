using System;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.Plugins.Reports
{
	public partial class DateRange : System.Web.UI.UserControl
	{
		public event EventHandler ValueChanged;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDropDowns();
				DateRange_ValueChanged(null, null);
			}
		}

		public DateTime StartDate
		{
			get
			{
				BindDropDowns();
				return ParseAndFix(int.Parse(ddlStartYear.SelectedValue), ddlStartMonth.SelectedIndex + 1, ddlStartDay.SelectedIndex + 1);
			}
		}

		public DateTime EndDate
		{
			get
			{
				BindDropDowns();
				return ParseAndFix(int.Parse(ddlEndYear.SelectedValue), ddlEndMonth.SelectedIndex + 1, ddlEndDay.SelectedIndex + 1);
			}
		}

		protected void DateRange_ValueChanged(object sender, EventArgs e)
		{
			DateTime lStartDate = StartDate;
			DateTime lEndDate = EndDate;

			if (lEndDate < lStartDate)
				lEndDate = lStartDate;

			ddlStartDay.SelectedIndex = lStartDate.Day - 1;
			ddlStartMonth.SelectedIndex = lStartDate.Month - 1;
			ddlStartYear.SelectedValue = lStartDate.Year.ToString();
			ddlEndDay.SelectedIndex = lEndDate.Day - 1;
			ddlEndMonth.SelectedIndex = lEndDate.Month - 1;
			ddlEndYear.SelectedValue = lEndDate.Year.ToString();

			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		private static DateTime ParseAndFix(int year, int month, int day)
		{
			try { return new DateTime(year, month, day); }
			catch (Exception) { return ParseAndFix(year, month, day - 1); }
		}

		private void BindDropDowns()
		{
			if (ddlStartDay.Items.Count == 0 || ddlEndDay.Items.Count == 0)
			{
				ddlStartDay.Items.Clear();
				ddlEndDay.Items.Clear();

				for (int i = 1; i < 32; i++)
				{
					ddlStartDay.Items.Add(i.ToString());
					ddlEndDay.Items.Add(i.ToString());
				}

				ddlStartDay.SelectedIndex = 0;
				ddlEndDay.SelectedIndex = ddlEndDay.Items.Count - 1;
			}

			if (ddlStartMonth.Items.Count == 0 || ddlEndMonth.Items.Count == 0)
			{
				ddlStartMonth.Items.Clear();
				ddlEndMonth.Items.Clear();

				DateTime lDate = new DateTime();
				for (int i = 0; i < 12; i++)
				{
					ddlStartMonth.Items.Add(lDate.ToString("MMMM"));
					ddlEndMonth.Items.Add(lDate.ToString("MMMM"));
					lDate = lDate.AddMonths(1);
				}

				ddlStartMonth.SelectedIndex = 0;
				ddlEndMonth.SelectedIndex = ddlEndMonth.Items.Count - 1;
			}

			if (ddlStartYear.Items.Count == 0 || ddlEndYear.Items.Count == 0)
			{
				ddlStartYear.Items.Clear();
				ddlEndYear.Items.Clear();

				for (int i = 2007; i <= DateTime.Now.Year; i++)
				{
					ddlStartYear.Items.Add(i.ToString());
					ddlEndYear.Items.Add(i.ToString());
				}

				ddlStartYear.SelectedIndex = 0;
				ddlEndYear.SelectedIndex = ddlEndYear.Items.Count - 1;
			}
		}
	}
}