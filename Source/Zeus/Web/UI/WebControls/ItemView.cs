using System;
using System.Web.UI.WebControls;
using Zeus.Engine;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Collections.Generic;
using Zeus.ContentTypes.Properties;

namespace Zeus.Web.UI.WebControls
{
	public class ItemView : WebControl
	{
		private ContentItem _currentItem;

		#region Properties

		public IDictionary<string, Control> PropertyControls
		{
			get;
			private set;
		}

		public ItemViewMode Mode
		{
			get { return ViewState["Mode"] as ItemViewMode? ?? ItemViewMode.Display; }
			set { ViewState["Mode"] = value; }
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
						ContentItem parentItem = Zeus.Context.Current.Persister.Get(this.ParentItemID);
						_currentItem = Zeus.Context.Current.ContentTypes.CreateInstance(this.CurrentItemType, parentItem);
					}
					else
					{
						_currentItem = this.FindCurrentItem();
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
					if (this.Mode == ItemViewMode.Edit)
						foreach (Property editableProperty in this.CurrentItemDefinition.EditableProperties)
							editableProperty.Editor.UpdateEditor(value, this.PropertyControls[editableProperty.Name]);
				}
				else
				{
					this.Discriminator = null;
				}
			}
		}

		public int ParentItemID
		{
			get { return (int) (ViewState["ParentItemID"] ?? 0); }
			set { ViewState["ParentItemID"] = value; }
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

		protected override void OnLoad(EventArgs e)
		{
			EnsureChildControls();
			base.OnInit(e);
		}

		protected override void CreateChildControls()
		{
			// Get ItemDefinition for current type.
			ContentType itemDefinition = this.CurrentItemDefinition;
			this.PropertyControls = new Dictionary<string, Control>();
			switch (this.Mode)
			{
				case ItemViewMode.Display:
					foreach (Property displayableProperty in itemDefinition.DisplayableProperties)
						this.PropertyControls.Add(displayableProperty.Name, displayableProperty.Displayer.AddTo(this, this.CurrentItem, displayableProperty.Name));
					break;
				case ItemViewMode.Edit:
					foreach (Property editableProperty in itemDefinition.EditableProperties)
						this.PropertyControls.Add(editableProperty.Name, editableProperty.Editor.AddTo(this));
					if (!Page.IsPostBack)
						foreach (Property editableProperty in itemDefinition.EditableProperties)
							editableProperty.Editor.UpdateEditor(this.CurrentItem, this.PropertyControls[editableProperty.Name]);
					break;
			}

			base.CreateChildControls();
		}

		public void Save()
		{
			EnsureChildControls();
			foreach (Property editableProperty in this.CurrentItemDefinition.EditableProperties)
				editableProperty.Editor.UpdateItem(this.CurrentItem, this.PropertyControls[editableProperty.Name]);
			Zeus.Context.Persister.Save(this.CurrentItem);
		}

		#endregion
	}
}
