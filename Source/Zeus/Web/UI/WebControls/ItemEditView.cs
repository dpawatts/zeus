using System;
using Zeus.ContentTypes.Properties;
using Zeus.ContentTypes;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : ItemView
	{
		private bool _postedBack;
		public event EventHandler<ItemEventArgs> Saved;

		protected override void AddPropertyControls()
		{
			if (CurrentItemDefinition != null)
			{
				// Add editors and containers recursively.
				AddPropertyControlsRecursive(this, this.CurrentItemDefinition.RootContainer);

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
				this.PropertyControls.Add(contained.Name, addedControl);
			if (contained is IEditorContainer)
				foreach (IContainable subContained in ((IEditorContainer) contained).GetContained())
					AddPropertyControlsRecursive(addedControl, subContained);
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			_postedBack = true;
		}

		private void UpdateEditors()
		{
			foreach (IEditor editor in this.CurrentItemDefinition.Editors)
				editor.UpdateEditor(this.CurrentItem, this.PropertyControls[editor.Name]);
		}

		public void Save()
		{
			EnsureChildControls();
			foreach (IEditor editor in this.CurrentItemDefinition.Editors)
				editor.UpdateItem(this.CurrentItem, this.PropertyControls[editor.Name]);
			OnSaved(new ItemEventArgs(this.CurrentItem));
			Zeus.Context.Persister.Save(this.CurrentItem);
		}

		protected virtual void OnSaved(ItemEventArgs args)
		{
			if (this.Saved != null)
				this.Saved(this, args);
		}
	}
}
