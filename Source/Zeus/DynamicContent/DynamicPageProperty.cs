using System;
using Zeus.Design.Editors;

namespace Zeus.DynamicContent
{
	public class DynamicPageProperty : IDynamicContent
	{
		#region Fields

		private int _contentID;
		private string _detailName;

		#endregion

		#region Properties

		[TextBoxEditor("Content Item", 10)]
		public int ContentID
		{
			get;
			set;
		}

		[TextBoxEditor("Property Name", 20)]
		public string DetailName
		{
			get;
			set;
		}

		string IDynamicContent.State
		{
			get { return _contentID + "," + _detailName; }
			set
			{
				string[] values = value.Split(new[] { ',' });
				_contentID = Convert.ToInt32(values[0]);
				_detailName = values[1];
			}
		}

		#endregion

		#region Methods

		public string Render()
		{
			ContentItem contentItem = Context.Persister.Get(_contentID);
			object value = contentItem[_detailName];
			if (value != null)
				return value.ToString();
			return string.Empty;
		}

		#endregion
	}
}