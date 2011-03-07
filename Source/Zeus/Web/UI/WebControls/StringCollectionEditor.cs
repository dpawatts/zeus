using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.ContentProperties;

namespace Zeus.Web.UI.WebControls
{
	public class StringCollectionEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string ItemTitle
		{
			get { return "String"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, PropertyData detail)
		{
			StringProperty stringProperty = detail as StringProperty;

			TextBox txt = new TextBox { CssClass = "linkedItem", ID = ID + "_txt_" + id };
			if (stringProperty != null)
				txt.Text = stringProperty.StringValue;
			return txt;
		}
	}
}