using System;

namespace Zeus.Web.UI.WebControls
{
	public class CurrentItemDataSource : ContentDataSource<CurrentItemDataSourceView>
	{
		protected override CurrentItemDataSourceView CreateView()
		{
			return new CurrentItemDataSourceView(this, DefaultViewName, this.CurrentItem);
		}
	}
}
