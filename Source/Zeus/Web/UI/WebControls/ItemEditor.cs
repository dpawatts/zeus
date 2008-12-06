using System;
using System.Web.UI.WebControls;
using Zeus.Engine;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Collections.Generic;
using Zeus.ContentTypes.Properties;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditor : WebControl
	{
		private IDictionary<string, Control> _addedEditors;
		private ContentItem _currentItem;
		private ContentEngine _engine;

		#region Properties

		/// <summary>Gets or sets the item to edit with this form.</summary>
		public ContentItem CurrentItem
		{
			get
			{
				if (_currentItem == null && !string.IsNullOrEmpty(this.Discriminator))
				{
					ContentItem parentItem = this.Engine.Persister.Get(this.ParentItemID);
					_currentItem = this.Engine.ContentTypes.CreateInstance(this.CurrentItemType, parentItem);
				}
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				if (value != null)
				{
					this.Discriminator = this.Engine.ContentTypes[value.GetType()].Discriminator;
					EnsureChildControls();
					foreach (IEditor editable in this.CurrentItemDefinition.EditableProperties)
						editable.UpdateEditor(value, _addedEditors[editable.Name]);
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
					return this.Engine.ContentTypes[this.Discriminator];
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
				ContentType def = this.CurrentItemDefinition;
				if (def != null)
					return def.ItemType;
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

		protected ContentEngine Engine
		{
			get { return _engine ?? Zeus.Context.Current; }
			set { _engine = value; }
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
			_addedEditors = new Dictionary<string, Control>();
			foreach (IEditor editable in itemDefinition.EditableProperties)
				_addedEditors.Add(editable.Name, editable.AddTo(this));
			if (!Page.IsPostBack)
				foreach (IEditor editable in itemDefinition.EditableProperties)
					editable.UpdateEditor(this.CurrentItem, _addedEditors[editable.Name]);

			base.CreateChildControls();
		}

		public void Save()
		{
			EnsureChildControls();
			foreach (IEditor editable in this.CurrentItemDefinition.EditableProperties)
				editable.UpdateItem(this.CurrentItem, _addedEditors[editable.Name]);
			this.Engine.Persister.Save(this.CurrentItem);
		}

		#endregion
	}
}
