using System;
using System.Web.UI;

namespace Isis.Web.UI.WebControls
{
	public class ConditionalViewCollection : System.Web.UI.ControlCollection
	{
		public ConditionalViewCollection(Control owner)
			: base(owner)
		{
		}

		public override void Add(Control v)
		{
			if (!(v is ConditionalView) && !(v is ConditionalViewItem))
				throw new ArgumentException("ConditionalViewCollection must contain ConditionalView");
			base.Add(v);
		}

		public override void AddAt(int index, Control v)
		{
			if (!(v is ConditionalView) && !(v is ConditionalViewItem))
				throw new ArgumentException("ConditionalViewCollection must contain ConditionalView");
			base.AddAt(index, v);
		}

		// Properties
		public new ConditionalView this[int i]
		{
			get { return (ConditionalView) base[i]; }
		}
	}
}
