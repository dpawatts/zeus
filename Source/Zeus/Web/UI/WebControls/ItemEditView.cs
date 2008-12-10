using System;
using Zeus.ContentTypes.Properties;
using Zeus.ContentTypes;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : ItemView
	{
		protected override void AddPropertyControls()
		{
			// Add editors and containers recursively.
			AddPropertyControlsRecursive(this, this.CurrentItemDefinition.RootContainer);
				
			if (!Page.IsPostBack)
				UpdateEditors();
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
			Zeus.Context.Persister.Save(this.CurrentItem);
		}
	}
}
