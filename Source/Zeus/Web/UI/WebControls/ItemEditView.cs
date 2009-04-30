using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Design.Editors;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : ItemView
	{
		private bool _postedBack;

		#region Events

		public event EventHandler<ItemViewEditableObjectEventArgs> Saving;
		public event EventHandler<ItemViewEditableObjectEventArgs> Saved;

		#endregion

		/// <summary>Gets or sets wether a version should be saved before the item is updated.</summary>
		public ItemEditorVersioningMode VersioningMode
		{
			get { return (ItemEditorVersioningMode) (ViewState["VersioningMode"] ?? ItemEditorVersioningMode.VersionAndSave); }
			set { ViewState["VersioningMode"] = value; }
		}

		public override IEditableObject CurrentItem
		{
			get { return base.CurrentItem; }
			set
			{
				base.CurrentItem = value;
				if (value != null && ((ContentItem) value).VersionOf != null && ((ContentItem) value).ID == 0)
					VersioningMode = ItemEditorVersioningMode.SaveOnly;
			}
		}

		protected override void AddPropertyControls()
		{
			if (CurrentItemDefinition != null)
			{
				// Add editors and containers recursively.
				AddPropertyControlsRecursive(this, CurrentItemDefinition.RootContainer);

				if (!_postedBack)
					UpdateEditors();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (_postedBack)
				EnsureChildControls();
		}

		private void AddPropertyControlsRecursive(Control control, IContainable contained)
		{
			Control addedControl = contained.AddTo(control);
			if (contained is IEditor)
				PropertyControls.Add(contained.Name, addedControl);
			if (contained is IEditorContainer)
				foreach (IContainable subContained in ((IEditorContainer) contained).GetContained(Page.User))
					AddPropertyControlsRecursive(addedControl, subContained);
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			_postedBack = true;
		}

		private void UpdateEditors()
		{
			foreach (IEditor editor in CurrentItemDefinition.GetEditors(Page.User))
				editor.UpdateEditor(CurrentItem, PropertyControls[editor.Name]);
		}

		public IEditableObject Save(ContentItem item, ItemEditorVersioningMode mode)
		{
			EnsureChildControls();

			// TODO- remove this because it won't work with DynamicContent
			item = Zeus.Context.AdminManager.Save(item, PropertyControls, mode, Page.User);

			OnSaving(new ItemViewEditableObjectEventArgs(CurrentItem));
			OnSaved(new ItemViewEditableObjectEventArgs(CurrentItem));
			
			return item;
		}

		/// <summary>Saves <see cref="CurrentItem"/> with the values entered in the form.</summary>
		/// <returns>The saved item.</returns>
		public IEditableObject Save()
		{
			CurrentItem = Save((ContentItem) CurrentItem, VersioningMode);
			return CurrentItem;
		}

		protected virtual void OnSaving(ItemViewEditableObjectEventArgs args)
		{
			if (Saving != null)
				Saving(this, args);
		}

		protected virtual void OnSaved(ItemViewEditableObjectEventArgs args)
		{
			if (Saved != null)
				Saved(this, args);
		}
	}
}