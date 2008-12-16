using System;
using System.Linq;
using System.Web.UI;
using System.ComponentModel;
using Zeus.ContentTypes;
using Zeus.ContentTypes.Properties;

namespace Zeus.Web.UI.WebControls
{
	public class ItemDetailView : Control, IContentItemContainer
	{
		#region Fields

		private ContentItem _currentItem;

		#endregion

		#region Properties

		public ContentItem CurrentItem
		{
			get
			{
				if (_currentItem == null)
					_currentItem = this.FindCurrentItem();
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				ChildControlsCreated = false;
			}
		}

		public string PropertyName
		{
			get { return ViewState["PropertyName"] as string ?? string.Empty; }
			set { ViewState["PropertyName"] = value; }
		}

		[Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(ItemDetailView))]
		public virtual ITemplate LayoutTemplate
		{
			get;
			set;
		}

		#endregion

		protected override void CreateChildControls()
		{
			// Get current item.
			ContentItem contentItem = this.CurrentItem;
			
			// Get selected property from content item.
			ContentType contentType = Zeus.Context.Current.ContentTypes[contentItem.GetType()];
			IDisplayer displayer = contentType.Displayers.SingleOrDefault(d => d.Name == this.PropertyName);
			if (displayer == null)
				throw new ZeusException("Could not find Displayer on property '{0}' on content type '{1}'.", this.PropertyName, contentType.Discriminator);
			displayer.AddTo(this, contentItem, this.PropertyName);
		}

		protected virtual void EnsureLayoutTemplate()
		{
			if (this.Controls.Count == 0)
			{
				this.Controls.Clear();
				this.CreateLayoutTemplate();
			}
		}

		protected virtual void CreateLayoutTemplate()
		{
			Control container = new Control();
			if (this.LayoutTemplate != null)
			{
				this.LayoutTemplate.InstantiateIn(container);
				this.Controls.Add(container);
			}
		}
	}
}
