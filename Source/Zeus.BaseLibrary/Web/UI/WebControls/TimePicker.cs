using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	public class TimePicker : System.Web.UI.WebControls.WebControl
	{
		private DropDownList _hours;
		private DropDownList _minutes;
		private DropDownList _timeOfDay;
		
		public TimeSpan Time
		{
			get
			{
				int hours = Convert.ToInt32(_hours.SelectedValue);
				if (_timeOfDay.SelectedValue == "AM" && hours == 12)
					hours = 0;
				else if (_timeOfDay.SelectedValue == "PM" && hours < 12)
					hours += 12;
				return new TimeSpan(hours, Convert.ToInt32(_minutes.SelectedValue), 0);
			}
			set
			{
				int hours = value.Hours;
				if (hours == 0)
					hours = 12;
				else if (hours > 12)
					hours -= 12;
				_hours.SelectedValue = hours.ToString();
				_minutes.SelectedValue = value.Minutes.ToString();
				_timeOfDay.SelectedValue = (value.Hours >= 12) ? "PM" : "AM";
			}
		}

		protected override void CreateChildControls()
		{
			_hours = new DropDownList();
			_hours.CssClass = this.CssClass;
			_hours.Items.Add("12");
			for (int i = 1; i <= 11; i++)
				_hours.Items.Add(i.ToString());
			this.Controls.Add(_hours);

			this.Controls.Add(new LiteralControl(" "));

			_minutes = new DropDownList();
			_minutes.CssClass = this.CssClass;
			for (int i = 0; i <= 59; i++)
				_minutes.Items.Add(i.ToString().PadLeft(2, '0'));
			this.Controls.Add(_minutes);

			this.Controls.Add(new LiteralControl(" "));

			_timeOfDay = new DropDownList();
			_timeOfDay.CssClass = this.CssClass;
			_timeOfDay.Items.Add("AM");
			_timeOfDay.Items.Add("PM");
			this.Controls.Add(_timeOfDay);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EnsureChildControls();
		}
	}
}
