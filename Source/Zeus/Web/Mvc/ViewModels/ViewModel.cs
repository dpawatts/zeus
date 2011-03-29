using System;
using Zeus.Web.UI;
using Spark;

namespace Zeus.Web.Mvc.ViewModels
{
	public abstract class ViewModel
	{
		public ContentItem CurrentItem
		{
			get { throw new NotSupportedException(); }
		}
	}

	public class ViewModel<T> : ViewModel, IContentItemContainer<T>
		where T : ContentItem
	{
		public ViewModel(T currentItem)
		{
			CurrentItem = currentItem;

            //fire changed signal if needed
            var SessionVal = System.Web.HttpContext.Current.Application["zeusChange_" + currentItem.ID];
            if ((SessionVal == null) || (SessionVal != null && (System.DateTime)SessionVal != currentItem.Updated))
            {
                _allDataSignal.FireChanged();
                System.Web.HttpContext.Current.Application["zeusChange_" + currentItem.ID] = currentItem.Updated;
            }
		}

        public static CacheSignal _allDataSignal = new CacheSignal();

        public ICacheSignal GetSignalForContentID
        {
            get
            {
                return _allDataSignal;
            }
        }

		/// <summary>Gets the item associated with the item container.</summary>
		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentItem; }
		}

		public new T CurrentItem { get; set; }
	}
}