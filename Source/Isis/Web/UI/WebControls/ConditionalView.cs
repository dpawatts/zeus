using System;
using System.Web.UI;
using System.ComponentModel;

namespace Isis.Web.UI.WebControls
{
	[PersistChildren(false), ParseChildren(true), ControlBuilder(typeof(ConditionalViewControlBuilder))]
	public class ConditionalView : System.Web.UI.Control
	{
		public string Expression
		{
			get;
			set;
		}

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(ConditionalViewItem)), DefaultValue((string) null)]
		public ITemplate ItemTemplate
		{
			get;
			set;
		}
	}
}
