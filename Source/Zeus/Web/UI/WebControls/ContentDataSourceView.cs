using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using System.Web.UI;
using Isis.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections;

namespace Zeus.Web.UI.WebControls
{
	public class ContentDataSourceView : BaseContentDataSourceView, IStateManager
	{
		private static readonly string _identifierPattern = @"^\s*[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}_][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Nd}\p{Pc}\p{Mn}\p{Mc}\p{Cf}_]*";
		private static readonly Regex _identifierRegex = new Regex(_identifierPattern + @"\s*$");

		private DataSourceControl _owner;
		private HttpContext _context;

		private bool _tracking;

		private string _ofType, _where, _select, _query;
		private ContentDataSourceAxis _axis = ContentDataSourceAxis.Child;
		private ParameterCollection _whereParameters;

		public override bool CanPage
		{
			get { return true; }
		}

		public string OfType
		{
			get
			{
				return (this._ofType ?? string.Empty);
			}
			set
			{
				if (this._ofType != value)
				{
					this._ofType = value;
					this.OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		public string Where
		{
			get
			{
				return (this._where ?? string.Empty);
			}
			set
			{
				if (this._where != value)
				{
					this._where = value;
					this.OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		public string SelectNew
		{
			get
			{
				return (this._select ?? string.Empty);
			}
			set
			{
				if (this._select != value)
				{
					this._select = value;
					this.OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		public ContentDataSourceAxis Axis
		{
			get { return _axis; }
			set
			{
				if (this._axis != value)
				{
					this._axis = value;
					this.OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		public string Query
		{
			get
			{
				return (this._query ?? string.Empty);
			}
			set
			{
				if (this._query != value)
				{
					this._query = value;
					this.OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		public ParameterCollection WhereParameters
		{
			get
			{
				if (this._whereParameters == null)
				{
					this._whereParameters = new ParameterCollection();
					this._whereParameters.ParametersChanged += new EventHandler(this.WhereParametersChangedEventHandler);
					if (this._tracking)
						((IStateManager) this._whereParameters).TrackViewState();
				}
				return this._whereParameters;
			}
		}

		public bool IsTrackingViewState
		{
			get { return this._tracking; }
		}

		public ContentDataSourceView(DataSourceControl owner, string viewName, HttpContext context, ContentItem parentItem)
			: base(owner, viewName, parentItem)
		{
			_owner = owner;
			_context = context;
		}

		protected override IEnumerable GetItems()
		{
			if (this.ParentItem != null)
			{
				ContentItem startingPoint = this.ParentItem;
				if (!string.IsNullOrEmpty(this.Query))
					startingPoint = Find.RootItem;

				IQueryable children = null;
				switch (this.Axis)
				{
					case ContentDataSourceAxis.Child:
						children = startingPoint.GetChildren().AsQueryable();
						break;
					case ContentDataSourceAxis.Descendant:
						children = Find.EnumerateAccessibleChildren(startingPoint).AsQueryable();
						break;
					default :
						throw new NotImplementedException();
				}

				if (!string.IsNullOrEmpty(this.OfType))
				{
					Type typeFilter = BuildManager.GetType(this.OfType, true);
					children = children.OfType(typeFilter);
				}

				IDictionary<string, object> whereParameterValues = this.GetParameterValues(this.WhereParameters);
				if (!string.IsNullOrEmpty(this.Where))
					children = children.Where(this.Where, EscapeParameterKeys(whereParameterValues));

				if (!string.IsNullOrEmpty(this.SelectNew))
				{
					IEnumerable tempChildren = children.Select(this.SelectNew, null);
					List<object> result = new List<object>();
					foreach (IEnumerable child in tempChildren)
						foreach (object value in child)
							result.Add(value);
					children = result.AsQueryable();
				}

				return children;
			}
			else
				return null;
		}

		private IDictionary<string, object> EscapeParameterKeys(IDictionary<string, object> parameters)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(parameters.Count, StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, object> pair in parameters)
			{
				string key = pair.Key;
				if (string.IsNullOrEmpty(key))
					throw new InvalidOperationException(string.Format("Parameters for ChildrenDataSource '{0}' must be named.", new object[] { this._owner.ID }));
				this.ValidateParameterName(key);
				dictionary.Add('@' + key, pair.Value);
			}
			return dictionary;
		}

		private IDictionary<string, object> GetParameterValues(IOrderedDictionary parameterValues)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(parameterValues.Count, StringComparer.OrdinalIgnoreCase);
			foreach (DictionaryEntry entry in parameterValues)
				dictionary[(string) entry.Key] = entry.Value;
			return dictionary;
		}

		private IDictionary<string, object> GetParameterValues(ParameterCollection parameters)
		{
			return this.GetParameterValues(parameters.GetValues(this._context, this._owner));
		}

		public void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] objArray = (object[]) savedState;
				if (objArray[0] != null)
					((IStateManager) this.WhereParameters).LoadViewState(objArray[0]);
			}
		}

		private object SaveParametersViewState(ParameterCollection parameters)
		{
			if ((parameters != null) && (parameters.Count > 0))
				return ((IStateManager) parameters).SaveViewState();
			return null;
		}

		public object SaveViewState()
		{
			object[] objArray = new object[1];
			objArray[0] = this.SaveParametersViewState(this._whereParameters);
			return objArray;
		}

		private void WhereParametersChangedEventHandler(object o, EventArgs e)
		{
			this.OnDataSourceViewChanged(EventArgs.Empty);
		}

		private void TrackParametersViewState(ParameterCollection parameters)
		{
			if ((parameters != null) && (parameters.Count > 0))
				((IStateManager) parameters).TrackViewState();
		}

		public void TrackViewState()
		{
			this._tracking = true;
			this.TrackParametersViewState(this._whereParameters);
		}

		protected virtual void ValidateParameterName(string name)
		{
			if (!_identifierRegex.IsMatch(name))
				throw new InvalidOperationException(string.Format("The name for parameter '{0}' on ChildrenDataSource '{1}' is not a valid identifier name.", new object[] { name, this._owner.ID }));
		}
	}
}
