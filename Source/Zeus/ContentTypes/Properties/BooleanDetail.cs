using System;

namespace Zeus.ContentTypes.Properties
{
	public class BooleanDetail : ContentDetail<bool>
	{
		#region Constuctors

		public BooleanDetail()
			: base()
		{
		}

		public BooleanDetail(ContentItem containerItem, string name, bool value)
			: base(containerItem, name, value)
		{

		}

		#endregion
	}
}
