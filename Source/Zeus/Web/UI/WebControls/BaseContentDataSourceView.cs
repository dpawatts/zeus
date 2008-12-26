using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Collections;

namespace Zeus.Web.UI.WebControls
{
	public abstract class BaseContentDataSourceView : DataSourceView
	{
		#region Fields

		private ContentItem _parentItem = null;
		private IEnumerable _items = null;

		#endregion

		#region Constructor

		public BaseContentDataSourceView(IDataSource owner, string viewName, ContentItem parentItem)
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

		public override bool CanDelete
		{
			get { return true; }
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

		private IEnumerable GetCachedItems()
		{
			if (_items == null)
				_items = GetItems();
			return _items;
		}

		protected abstract IEnumerable GetItems();

		protected virtual IEnumerable<ContentItem> GetItemsFromIdentifiers(IDictionary keys)
		{
			List<ContentItem> items = new List<ContentItem>();
			foreach (int itemID in keys.Values)
				items.Add(Zeus.Context.Persister.Get(itemID));
			return items;
		}

		protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
		{
			IEnumerable<ContentItem> items = GetItemsFromIdentifiers(keys);

			foreach (ContentItem item in items)
				Zeus.Context.Persister.Delete(item);

			if (items.Count() > 0)
				OnDataSourceViewChanged(EventArgs.Empty);

			return items.Count();
		}

		protected override System.Collections.IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			IEnumerable allItems = GetCachedItems();
			if (allItems == null)
				return null;

			if (arguments.RetrieveTotalRowCount)
				arguments.TotalRowCount = allItems.AsQueryable().Count();

			if (arguments.MaximumRows > 0 && arguments.StartRowIndex >= 0)
			{
				allItems = allItems.AsQueryable().Skip(arguments.StartRowIndex);
				allItems = allItems.AsQueryable().Take(arguments.MaximumRows);
			}

			if (!string.IsNullOrEmpty(arguments.SortExpression))
			{
				if (allItems.AsQueryable().Any())
				{
					Array typedArray = Array.CreateInstance(allItems.Cast<object>().First().GetType(), allItems.AsQueryable().Count());
					Array.Copy(allItems.Cast<object>().ToArray(), typedArray, typedArray.Length);
					allItems = typedArray;
				}
				allItems = allItems.AsQueryable().OrderBy(arguments.SortExpression);
			}

			return allItems;
		}

		#endregion
	}
}
