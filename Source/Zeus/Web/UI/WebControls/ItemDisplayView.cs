using System;
using Zeus.Design.Displayers;

namespace Zeus.Web.UI.WebControls
{
	public class ItemDisplayView : ItemView
	{
		protected override void AddPropertyControls()
		{
			foreach (IDisplayer displayer in this.CurrentItemDefinition.Displayers)
			{
				displayer.InstantiateIn(this);
				displayer.SetValue(this, (ContentItem) CurrentItem, displayer.Name);
				//this.PropertyControls.Add(displayer.Name, displayer.AddTo(this, this.CurrentItem, displayer.Name));
			}
		}
	}
}
