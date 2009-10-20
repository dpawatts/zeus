using System;
using System.ComponentModel;
using Isis.ExtensionMethods;

namespace Isis.Web.UI.WebControls
{
	public class DynamicFile : System.Web.UI.WebControls.HyperLink
	{
		private DatabaseSource _databaseSource;
		private bool _sourceEnsured;

		#region Properties

		private DatabaseSource InternalSource
		{
			get { return ViewState["InternalSource"] as DatabaseSource; }
			set { ViewState["InternalSource"] = value; }
		}

		[Category("Source"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DatabaseSource Source
		{
			get
			{
				if (_databaseSource == null)
					_databaseSource = new DatabaseSource();
				return _databaseSource;
			}
		}

		[Browsable(false)]
		public new string NavigateUrl
		{
			get
			{
				EnsureNavigateUrl();
				return base.NavigateUrl;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		#endregion

		#region Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
		}

		private void Page_PreRenderComplete(object sender, EventArgs e)
		{
			if (this.Visible)
				EnsureNavigateUrl();
		}

		private void EnsureNavigateUrl()
		{
			if (!_sourceEnsured)
			{
				DatabaseSource databaseSource = this.InternalSource;
				if (databaseSource == null)
				{
					databaseSource = _databaseSource;
					this.InternalSource = _databaseSource;
				}
				// check if database contains a non-null value
				string fileName;
				if (databaseSource.CheckFileExists(out fileName))
					base.NavigateUrl = string.Format("/DynamicFile.axd?file={1}", fileName, databaseSource.ToBase64String());
				else
					this.Visible = false;
				_sourceEnsured = true;
			}
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
			this.Page.PreRenderComplete -= new EventHandler(Page_PreRenderComplete);
		}

		#endregion
	}
}
