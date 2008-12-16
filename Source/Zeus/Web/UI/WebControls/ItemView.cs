using System;
using System.Web.UI.WebControls;
using Zeus.Engine;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Collections.Generic;
using Zeus.ContentTypes.Properties;
using Zeus.Admin;

namespace Zeus.Web.UI.WebControls
{
	public abstract class ItemView : WebControl, INamingContainer, IItemView
	{
		private ContentItem _currentItem;

		#region Properties

		public IDictionary<string, Control> PropertyControls
		{
			get;
			private set;
		}

		/// <summary>Gets or sets the item to edit with this form.</summary>
		public ContentItem CurrentItem
		{
			get
			{
				if (_currentItem == null)
				{
					if (!string.IsNullOrEmpty(this.Discriminator))
					{
						ContentItem parentItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(this.ParentPath);
						_currentItem = Zeus.Context.Current.ContentTypes.CreateInstance(this.CurrentItemType, parentItem);
					}
				}
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				if (value != null)
				{
					this.Discriminator = Zeus.Context.Current.ContentTypes[value.GetType()].Discriminator;
					EnsureChildControls();
					OnCurrentItemChanged(EventArgs.Empty);
				}
				else
				{
					this.Discriminator = null;
				}
			}
		}

		protected virtual void OnCurrentItemChanged(EventArgs eventArgs)
		{
			
		}

		public string ParentPath
		{
			get { return (string) ViewState["ParentPath"] ?? string.Empty; }
			set { ViewState["ParentPath"] = value; }
		}

		public ContentType CurrentItemDefinition
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Discriminator))
					return Zeus.Context.Current.ContentTypes[this.Discriminator];
				else if (this.CurrentItem != null)
					return Zeus.Context.Current.ContentTypes[this.CurrentItem.GetType()];
				else
					return null;
			}
		}

		/// <summary>Gets the type defined by <see cref="ItemTypeName"/>.</summary>
		/// <returns>The item's type.</returns>
		public Type CurrentItemType
		{
			get
			{
				ContentType contentType = this.CurrentItemDefinition;
				if (contentType != null)
					return contentType.ItemType;
				else
					return null;
			}
		}

		/// <summary>The type of item to edit. ItemEditor will look at <see cref="Zeus.EditableAttribute"/> attributes on this type to render input controls.</summary>
		public string Discriminator
		{
			get { return (string) ViewState["Discriminator"] ?? string.Empty; }
			set { ViewState["Discriminator"] = value; }
		}

		#endregion

		#region Methods

		/*protected override void OnLoad(EventArgs e)
		{
			EnsureChildControls();
			base.OnLoad(e);
		}*/

		protected override void CreateChildControls()
		{
			// Get ItemDefinition for current type.
			ContentType itemDefinition = this.CurrentItemDefinition;
			this.PropertyControls = new Dictionary<string, Control>();
			AddPropertyControls();

			base.CreateChildControls();
		}

		protected abstract void AddPropertyControls();

		#endregion
	}
}
