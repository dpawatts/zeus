using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	public class ExtendedFormView : System.Web.UI.WebControls.FormView
	{
		[DefaultValue((string) null), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(ExtendedFormView), BindingDirection.TwoWay), TemplateInstance(TemplateInstance.Single)]
		public ITemplate InsertEditItemTemplate
		{
			get { return this.InsertItemTemplate; }
			set
			{
				this.InsertItemTemplate = value;
				this.EditItemTemplate = value;
			}
		}

		public string ContinueDestinationPageUrl
		{
			get;
			set;
		}

		protected override void OnItemInserted(FormViewInsertedEventArgs e)
		{
			base.OnItemInserted(e);
			if (e.Exception == null)
				Redirect();
		}

		protected override void OnItemUpdated(FormViewUpdatedEventArgs e)
		{
			base.OnItemUpdated(e);
			if (e.Exception == null)
				Redirect();
		}

		private void Redirect()
		{
			if (this.ContinueDestinationPageUrl != null)
				Page.Response.Redirect(this.ContinueDestinationPageUrl);
		}
	}
}
