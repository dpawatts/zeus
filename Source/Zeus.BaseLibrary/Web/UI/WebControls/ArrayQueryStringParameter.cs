using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	public class ArrayQueryStringParameter : QueryStringParameter
	{
		public string Separator
		{
			get
			{
				object obj2 = base.ViewState["ArraySeparator"];
				if (obj2 == null)
					return ",";
				return (string) obj2;
			}
			set
			{
				if (this.Separator != value)
				{
					base.ViewState["ArraySeparator"] = value;
					base.OnParameterChanged();
				}
			}
		}

		public string ItemType
		{
			get
			{
				object obj2 = base.ViewState["ItemType"];
				if (obj2 == null)
					return string.Empty;
				return (string) obj2;
			}
			set
			{
				if (this.Separator != value)
				{
					base.ViewState["ItemType"] = value;
					base.OnParameterChanged();
				}
			}
		}

		protected override object Evaluate(HttpContext context, Control control)
		{
			if (this.Type != TypeCode.Object)
				throw new InvalidOperationException("Type must be set to Object for ArrayQueryStringParameter");

			if (string.IsNullOrEmpty(this.ItemType))
				throw new InvalidOperationException("ItemType must be set for ArrayQueryStringParameter");

			// Get value.
			string value = (string) base.Evaluate(context, control);
			if (string.IsNullOrEmpty(value))
				return null;

			// Split using Separator.
			string[] values = value.Split(new string[] {this.Separator}, StringSplitOptions.RemoveEmptyEntries);

			// Convert each item to item type.
			Type newType = BuildManager.GetType(this.ItemType, true);
			object[] output = Array.ConvertAll<string, object>(values, o => Convert.ChangeType(o, newType));
			return output;
		}
	}
}
