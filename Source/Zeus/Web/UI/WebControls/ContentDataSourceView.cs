using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public abstract class ContentDataSourceView : DataSourceView
	{
		#region Fields

		private ContentItem _parentItem = null;
		private IEnumerable<ContentItem> _items = null;
		private string _currentSortExpression = string.Empty;

		#endregion

		#region Constructor

		public ContentDataSourceView(IDataSource owner, string viewName, ContentItem parentItem)
			: base(owner, viewName)
		{
			_parentItem = parentItem;
		}

		#endregion

		#region Properties

		public ContentItem ParentItem
		{
			get { return _parentItem; }
			set
			{
				_parentItem = value;
				_items = null;
				this.OnDataSourceViewChanged(EventArgs.Empty);
			}
		}

		public override bool CanInsert
		{
			get { return _parentItem != null; }
		}

		public override bool CanRetrieveTotalRowCount
		{
			get { return true; }
		}

		public override bool CanSort
		{
			get { return true; }
		}

		#endregion

		#region Methods

		private IEnumerable<ContentItem> GetCachedItems()
		{
			if (_items == null)
				_items = GetItems();
			return _items;
		}

		protected abstract IEnumerable<ContentItem> GetItems();

		protected override System.Collections.IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			IEnumerable<ContentItem> allItems = GetCachedItems();
			if (allItems == null)
				return null;

			if (arguments.RetrieveTotalRowCount)
				arguments.TotalRowCount = allItems.Count();

			if (!string.IsNullOrEmpty(arguments.SortExpression) && arguments.SortExpression != _currentSortExpression)
			{
				allItems = allItems.AsQueryable().OrderBy(arguments.SortExpression);
				_currentSortExpression = arguments.SortExpression;
			}

			return allItems;
		}

		#endregion
	}
}
