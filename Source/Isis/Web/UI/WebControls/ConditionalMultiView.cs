using System;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

namespace Isis.Web.UI.WebControls
{
	[ParseChildren(typeof(ConditionalView)), ControlBuilder(typeof(ConditionalMultiViewControlBuilder))]
	public class ConditionalMultiView : Control
	{
		private ConditionalViewItem _activeItem;

		public string DataItemTypeName
		{
			get;
			set;
		}

		public object Value
		{
			get;
			set;
		}

		[PersistenceMode(PersistenceMode.InnerDefaultProperty), Browsable(false)]
		public virtual ConditionalViewCollection Views
		{
			get { return (ConditionalViewCollection) this.Controls; }
		}

		protected override void AddParsedSubObject(object obj)
		{
			if (obj is ConditionalView)
				this.Controls.Add((Control) obj);
			else if (!(obj is LiteralControl))
				throw new HttpException(string.Format("ConditionalMultiView cannot have children of type {0}", obj.GetType().Name));
		}

		protected override ControlCollection CreateControlCollection()
		{
			return new ConditionalViewCollection(this);
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			_activeItem = this.GetActiveViewItem();
			if (_activeItem != null)
				this.Controls.Add(_activeItem);
		}

		protected override void OnDataBinding(EventArgs e)
		{
			EnsureChildControls();

			base.OnDataBinding(e);
			if (_activeItem != null)
				_activeItem.DataBind();
		}

		/// <summary>
		/// Gets active view based on bound value.
		/// </summary>
		/// <returns></returns>
		public ConditionalViewItem GetActiveViewItem()
		{
			DataBind();

			// Test each of the views in turn. The first one that matches will be used.
			ConditionalView chosenView = null;
			foreach (ConditionalView view in this.Views)
			{
				if (view.Expression == null)
				{
					chosenView = view;
					break;
				}

				if (this.Value != null)
				{
					Array sourceArray = Array.CreateInstance(this.Value.GetType(), 1);
					sourceArray.SetValue(this.Value, 0);
					IQueryable results = sourceArray.AsQueryable().Where(view.Expression);
					if (results.Any())
					{
						chosenView = view;
						break;
					}
				}
			}

			if (chosenView == null)
				return null;

			ConditionalViewItem control = new ConditionalViewItem();
			control.DataItem = this.Value;
			chosenView.ItemTemplate.InstantiateIn(control);

			return control;
		}

		public override void RenderControl(HtmlTextWriter writer)
		{
			EnsureChildControls();
			base.RenderControl(writer);

			//if (_activeItem != null)
			//	_activeItem.RenderControl(writer);
		}
	}
}
