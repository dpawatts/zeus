using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	public class ExtendedControlParameter : ControlParameter
	{
		public string EnumType
		{
			get
			{
				object obj2 = base.ViewState["EnumType"];
				if (obj2 == null)
				{
					return string.Empty;
				}
				return (string) obj2;
			}
			set
			{
				if (this.EnumType != value)
				{
					base.ViewState["EnumType"] = value;
					base.OnParameterChanged();
				}
			}
		}

		protected override object Evaluate(HttpContext context, Control control)
		{
			object value = base.Evaluate(context, control);

			// If EnumType is set, convert to enum.
			if (!string.IsNullOrEmpty(EnumType))
			{
				Type enumType = BuildManager.GetType(EnumType, true);
				return Enum.Parse(enumType, value.ToString());
			}
			else
			{
				return value;
			}
		}
	}
}
