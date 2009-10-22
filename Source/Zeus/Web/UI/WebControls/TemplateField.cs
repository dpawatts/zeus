using System;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class TemplateField : System.Web.UI.WebControls.TemplateField
	{
		public event EventHandler DataBinding;

		public virtual Control BindingContainer
		{
			get;
			internal set;
		}

		public virtual void DataBind()
		{
			if (DataBinding != null)
				DataBinding(this, EventArgs.Empty);
		}

		public override bool Initialize(bool sortingEnabled, System.Web.UI.Control control)
		{
			DataBind();
			return base.Initialize(sortingEnabled, control);
		}
	}
}