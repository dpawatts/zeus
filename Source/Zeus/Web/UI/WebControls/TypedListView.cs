using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	[ControlBuilder(typeof(TypedListViewControlBuilder))]
	public class TypedListView : ExtendedListView
	{
		/*private ItemTemplateCollection _itemTemplates;

		[PersistenceMode(PersistenceMode.InnerDefaultProperty), Browsable(false)]
		public virtual ItemTemplateCollection ItemTemplates
		{
			get
			{
				if (_itemTemplates == null)
					_itemTemplates = new ItemTemplateCollection(this);
				return _itemTemplates;
			}
		}*/

		public string DataItemTypeName
		{
			get;
			set;
		}

		/*protected override IList<ListViewDataItem> CreateItemsInGroups(ListViewPagedDataSource dataSource, bool dataBinding, InsertItemPosition insertPosition, ArrayList keyArray)
		{
			if (this._groupsOriginalIndexOfGroupPlaceholderInContainer == -1)
				this._groupsGroupPlaceholderContainer = this.GetPreparedContainerInfo(this, false, out this._groupsOriginalIndexOfGroupPlaceholderInContainer);
			int addLocation = this._groupsOriginalIndexOfGroupPlaceholderInContainer;
			this._groupsItemCreatedCount = 0;
			int placeholderIndex = 0;
			Control control = null;
			List<ListViewDataItem> list = new List<ListViewDataItem>();
			int num3 = 0;
			int displayIndex = 0;
			if (insertPosition == InsertItemPosition.FirstItem)
			{
				ListViewContainer container = new ListViewContainer();
				this.AutoIDControl(container);
				this.InstantiateGroupTemplate(container);
				this.AddControlToContainer(container, this._groupsGroupPlaceholderContainer, addLocation);
				addLocation++;
				control = this.GetPreparedContainerInfo(container, true, out placeholderIndex);
				ListViewItem item = this.CreateInsertItem();
				this.AddControlToContainer(item, control, placeholderIndex);
				placeholderIndex++;
				num3++;
			}
			foreach (object obj2 in dataSource)
			{
				if ((num3 % this._groupItemCount) == 0)
				{
					if ((num3 != 0) && (this._groupSeparatorTemplate != null))
					{
						ListViewContainer container2 = new ListViewContainer();
						this.AutoIDControl(container2);
						this.InstantiateGroupSeparatorTemplate(container2);
						this.AddControlToContainer(container2, this._groupsGroupPlaceholderContainer, addLocation);
						addLocation++;
					}
					ListViewContainer container3 = new ListViewContainer();
					this.AutoIDControl(container3);
					this.InstantiateGroupTemplate(container3);
					this.AddControlToContainer(container3, this._groupsGroupPlaceholderContainer, addLocation);
					addLocation++;
					control = this.GetPreparedContainerInfo(container3, true, out placeholderIndex);
				}
				ListViewDataItem item2 = this.CreateDataItem(displayIndex + this.StartRowIndex, displayIndex);
				this.InstantiateItemTemplate(item2, displayIndex);
				if (dataBinding)
				{
					item2.DataItem = obj2;
					OrderedDictionary keyTable = new OrderedDictionary(this.DataKeyNamesInternal.Length);
					foreach (string str in this.DataKeyNamesInternal)
					{
						object propertyValue = DataBinder.GetPropertyValue(obj2, str);
						keyTable.Add(str, propertyValue);
					}
					if (keyArray.Count == displayIndex)
					{
						keyArray.Add(new DataKey(keyTable, this.DataKeyNamesInternal));
					}
					else
					{
						keyArray[displayIndex] = new DataKey(keyTable, this.DataKeyNamesInternal);
					}
				}
				this.OnItemCreated(new ListViewItemEventArgs(item2));
				if (((num3 % this._groupItemCount) != 0) && (this._itemSeparatorTemplate != null))
				{
					ListViewContainer container4 = new ListViewContainer();
					this.InstantiateItemSeparatorTemplate(container4);
					this.AddControlToContainer(container4, control, placeholderIndex);
					placeholderIndex++;
				}
				this.AddControlToContainer(item2, control, placeholderIndex);
				placeholderIndex++;
				list.Add(item2);
				if (dataBinding)
				{
					item2.DataBind();
					this.OnItemDataBound(new ListViewItemEventArgs(item2));
					item2.DataItem = null;
				}
				num3++;
				displayIndex++;
			}
			if (insertPosition == InsertItemPosition.LastItem)
			{
				if ((num3 % this._groupItemCount) == 0)
				{
					if ((num3 != 0) && (this._groupSeparatorTemplate != null))
					{
						ListViewContainer container5 = new ListViewContainer();
						this.AutoIDControl(container5);
						this.InstantiateGroupSeparatorTemplate(container5);
						this.AddControlToContainer(container5, this._groupsGroupPlaceholderContainer, addLocation);
						addLocation++;
					}
					ListViewContainer container6 = new ListViewContainer();
					this.AutoIDControl(container6);
					this.InstantiateGroupTemplate(container6);
					this.AddControlToContainer(container6, this._groupsGroupPlaceholderContainer, addLocation);
					addLocation++;
					control = this.GetPreparedContainerInfo(container6, true, out placeholderIndex);
				}
				if (((num3 % this._groupItemCount) != 0) && (this._itemSeparatorTemplate != null))
				{
					ListViewContainer container7 = new ListViewContainer();
					this.InstantiateItemSeparatorTemplate(container7);
					this.AddControlToContainer(container7, control, placeholderIndex);
					placeholderIndex++;
				}
				ListViewItem item3 = this.CreateInsertItem();
				this.AddControlToContainer(item3, control, placeholderIndex);
				placeholderIndex++;
				num3++;
			}
			if (this._emptyItemTemplate != null)
			{
				while ((num3 % this._groupItemCount) != 0)
				{
					if (this._itemSeparatorTemplate != null)
					{
						ListViewContainer container8 = new ListViewContainer();
						this.InstantiateItemSeparatorTemplate(container8);
						this.AddControlToContainer(container8, control, placeholderIndex);
						placeholderIndex++;
					}
					ListViewItem item4 = this.CreateEmptyItem();
					this.AddControlToContainer(item4, control, placeholderIndex);
					placeholderIndex++;
					num3++;
				}
			}
			this._groupsItemCreatedCount = addLocation - this._groupsOriginalIndexOfGroupPlaceholderInContainer;
			return list;
		}*/
	}

	public class TypedListView<TDataItem> : TypedListView
	{
		protected override ListViewDataItem CreateDataItem(int dataItemIndex, int displayIndex)
		{
			return new TypedListViewDataItem<TDataItem>(dataItemIndex, displayIndex);
		}
	}
}