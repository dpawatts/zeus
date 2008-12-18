using System;

namespace Zeus.Web.UI.WebControls
{
	public class CurrentItemDataSource : BaseContentDataSource<CurrentItemDataSourceView>
	{
		protected override CurrentItemDataSourceView CreateView()
		{
			return new CurrentItemDataSourceView(this, DefaultViewName, this.CurrentItem);
		}
	}
}
