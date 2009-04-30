using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public class TimeRange : Control
	{
		private TimePicker fromTime;
		private TimePicker toTime;

		private RequiredFieldValidator fromTimeValidator;
		private RequiredFieldValidator toTimeValidator;

		private Label between;

		public bool Enabled
		{
			get { return (bool) (ViewState["Enabled"] ?? true); }
			set { ViewState["Enabled"] = value; }
		}

		public string From
		{
			get { return fromTime.Text; }
			set { fromTime.Text = value; }
		}

		public string To
		{
			get { return toTime.Text; }
			set { toTime.Text = value; }
		}

		public string BetweenText
		{
			get
			{
				EnsureChildControls();
				return between.Text;
			}
			set
			{
				EnsureChildControls();
				between.Text = value;
			}
		}

		public string StartTitle
		{
			get;
			set;
		}

		public bool StartRequired
		{
			get
			{
				EnsureChildControls();
				return fromTimeValidator.Enabled;
			}
			set
			{
				EnsureChildControls();
				fromTimeValidator.Enabled = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			fromTime = new TimePicker { ID = "from" + ID, Enabled = Enabled };
			Controls.Add(fromTime);

			fromTimeValidator = new RequiredFieldValidator
			{
				ID = "rfv" + fromTime.ID,
				ControlToValidate = fromTime.ID,
				Display = ValidatorDisplay.Dynamic,
				Text = "*",
				ErrorMessage = StartTitle + " is required",
				Enabled = false
			};
			Controls.Add(fromTimeValidator);

			between = new Label();
			Controls.Add(between);

			toTime = new TimePicker { ID = "to" + ID, Enabled = Enabled };
			Controls.Add(toTime);

			fromTime.TextChanged += TextChanged;
			toTime.TextChanged += TextChanged;
		}

		public event EventHandler TextChanged;
	}
}
