using System;
using System.Linq;
using System.Web.UI;
using System.ComponentModel;
using Zeus.ContentTypes;
using Zeus.ContentTypes.Properties;

namespace Zeus.Web.UI.WebControls
{
	public class Displayer : Control, IContentItemContainer
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

		[Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(Displayer))]
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
			Property property = contentType.Properties.SingleOrDefault(p => p.Name == this.PropertyName);
			if (property == null)
				throw new ZeusException("Could not find property '{0}' on content type '{1}'.", this.PropertyName, contentType.Discriminator);

			// Add Displayer control.
			if (!property.HasDisplayer)
				throw new ZeusException("Could not find Displayer on property '{0}' on content type '{1}'.", this.PropertyName, contentType.Discriminator);
			property.Displayer.AddTo(this, contentItem, this.PropertyName);
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
