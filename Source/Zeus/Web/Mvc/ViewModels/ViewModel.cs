using System;
using Zeus.Web.UI;
using Spark;
using System.Collections.Generic;

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
            ChangeSignalFired = false;

            //check watchers
            bool bWatcherChanged = false;
            if (CacheWatchers != null)
            {
                foreach (ContentItem ci in CacheWatchers)
                {
                    var WatcherSessionVal = System.Web.HttpContext.Current.Application["zeusWatchChange_" + ActionForCache + "_" + currentItem.ID + "_" + ci.ID];
                    if ((WatcherSessionVal == null) || (WatcherSessionVal != null && (System.DateTime)WatcherSessionVal != ci.Updated))
                    {
                        System.Web.HttpContext.Current.Application["zeusWatchChange_" + ActionForCache + "_" + currentItem.ID + "_" + ci.ID] = ci.Updated;
                        bWatcherChanged = true;
                    }
                }
            }

            //check itself
            var SessionVal = System.Web.HttpContext.Current.Application["zeusChange_" + ActionForCache + "_" + currentItem.ID];
            bool itemChanged = (SessionVal == null) || (SessionVal != null && (System.DateTime)SessionVal != currentItem.Updated);
            if (bWatcherChanged || itemChanged)
            {
                if (SessionVal != null)
                {
                    //only fire the signal if the item has actually changed (this is because once a signal is fired, all items for that template will be recached!)
                    _allDataSignal.FireChanged();
                    ChangeSignalFired = true;
                }
                if (itemChanged)
                    System.Web.HttpContext.Current.Application["zeusChange_" + ActionForCache + "_" + currentItem.ID] = currentItem.Updated;
            }
		}

        public virtual List<ContentItem> CacheWatchers { get; set; }

        public bool ChangeSignalFired { get; set; }
        public static CacheSignal _allDataSignal = new CacheSignal();

        public virtual string ActionForCache { get { return ""; } }

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