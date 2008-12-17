using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	[ParseChildren(true), PersistChildren(false)]
	public class ChildrenDataSource : ContentDataSource<ChildrenDataSourceView>
	{
		[DefaultValue("")]
		public string OfType
		{
			get { return this.View.OfType; }
			set { this.View.OfType = value; }
		}

		[DefaultValue("")]
		public string Where
		{
			get { return this.View.Where; }
			set { this.View.Where = value; }
		}

		public Axis Axis
		{
			get { return this.View.Axis; }
			set { this.View.Axis = value; }
		}

		[DefaultValue(null), Browsable(false), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty), Category("Data")]
		public ParameterCollection WhereParameters
		{
			get { return this.View.WhereParameters; }
		}

		protected override ChildrenDataSourceView CreateView()
		{
			return new ChildrenDataSourceView(this, DefaultViewName, this.Context, this.CurrentItem);
		}

		private void LoadCompleteEventHandler(object sender, EventArgs e)
		{
			this.WhereParameters.UpdateValues(this.Context, this);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.LoadComplete += new EventHandler(this.LoadCompleteEventHandler);
		}
	}

	public enum Axis
	{
		Child,
		Descendant
	}
}
