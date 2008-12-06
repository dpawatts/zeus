using System.Collections;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ContentDataSource : DataSourceControl
	{
		public const string CurrentItemViewName = "CurrentItem";
		public const string ChildrenViewName = "Children";

		private CurrentItemDataSourceView _currentItemView = null;
		private ChildrenDataSourceView _childrenView = null;
		private ContentItem _currentItem = null;

		public string OfType
		{
			get;
			set;
		}

		#region Methods

		protected virtual CurrentItemDataSourceView GetCurrentItemView()
		{
			if (_currentItemView == null)
				_currentItemView = new CurrentItemDataSourceView(this, CurrentItemViewName, this.CurrentItem);
			return _currentItemView;
		}

		protected virtual ChildrenDataSourceView GetChildrenView()
		{
			if (_childrenView == null)
			{
				_childrenView = new ChildrenDataSourceView(this, ChildrenViewName, this.CurrentItem);
				_childrenView.OfType = this.OfType;
			}
			return _childrenView;
		}

		#endregion

		public virtual ContentItem CurrentItem
		{
			get
			{
				if (_currentItem == null)
					_currentItem = (ContentItem) this.Context.Items["CurrentPage"];
				return _currentItem;
			}
			set { _currentItem = value; }
		}

		protected override ICollection GetViewNames()
		{
			return new string[] { CurrentItemViewName, ChildrenViewName };
		}

		protected override DataSourceView GetView(string viewName)
		{
			if (viewName == CurrentItemViewName)
				return GetCurrentItemView();
			else
				return GetChildrenView();
		}
	}

	public enum ContentDataSourceMode
	{
		CurrentItem,
		Children
	}
}
