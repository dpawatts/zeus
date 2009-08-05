using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	public class ExtendedParameter : Parameter
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

		public string Value
		{
			get
			{
				object obj2 = this.ViewState["Value"];
				return (obj2 as string);
			}
			set
			{
				if (this.Value != value)
				{
					this.ViewState["Value"] = value;
					this.OnParameterChanged();
				}
			}
		}

		protected override object Evaluate(HttpContext context, Control control)
		{
			// If EnumType is set, convert to enum.
			if (!string.IsNullOrEmpty(this.EnumType))
			{
				Type enumType = BuildManager.GetType(this.EnumType, true);
				return Enum.Parse(enumType, this.Value);
			}
			else
			{
				return null;
			}
		}
	}
}
