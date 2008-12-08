using System;

namespace Zeus.ContentTypes.Properties
{
	public class ObjectDetail : ContentDetail<object>
	{
		#region Constuctors

		public ObjectDetail()
			: base()
		{
		}

		public ObjectDetail(ContentItem containerItem, string name, object value)
			: base(containerItem, name, value)
		{

		}

		#endregion
	}
}