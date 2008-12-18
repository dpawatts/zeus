using System.Collections;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System;

namespace Zeus.Web.UI.WebControls
{
	public abstract class BaseContentDataSource<TView> : DataSourceControl
		where TView : BaseContentDataSourceView
	{
		private TView _view = null;
		private ICollection _viewNames;
		protected const string DefaultViewName = "DefaultView";
		private ContentItem _currentItem = null;

		internal TView View
		{
			get
			{
				if (this._view == null)
					this.View = CreateView();
				return this._view;
			}

			private set
			{
				this._view = value;
				if ((this._view != null) && base.IsTrackingViewState && (this is IStateManager))
					((IStateManager) this._view).TrackViewState();
			}
		}

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

		#region Methods

		protected abstract TView CreateView();

		protected override ICollection GetViewNames()
		{
			if (this._viewNames == null)
				this._viewNames = new ReadOnlyCollection<string>(new string[] { DefaultViewName });
			return this._viewNames;
		}

		protected override DataSourceView GetView(string viewName)
		{
			if (viewName == null)
				throw new ArgumentNullException("viewName");
			if ((viewName.Length != 0) && !string.Equals(viewName, DefaultViewName, StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException(string.Format("Content data source '{0}' only supports a single view named '{1}'. You may also leave the view name empty for the default view to be chosen.", new object[] { this.ID, DefaultViewName }), "viewName");
			return this.View;
		}

		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
			}
			else
			{
				Pair pair = (Pair) savedState;
				base.LoadViewState(pair.First);
				if ((this is IStateManager) && pair.Second != null)
					((IStateManager) this.View).LoadViewState(pair.Second);
			}
		}

		protected override object SaveViewState()
		{
			Pair pair = new Pair();
			pair.First = base.SaveViewState();
			if ((this is IStateManager) && this._view != null)
				pair.Second = ((IStateManager) this._view).SaveViewState();
			if ((pair.First == null) && (pair.Second == null))
				return null;
			return pair;
		}

		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ((this is IStateManager) && this._view != null)
				((IStateManager) this._view).TrackViewState();
		}

		#endregion
	}
}
