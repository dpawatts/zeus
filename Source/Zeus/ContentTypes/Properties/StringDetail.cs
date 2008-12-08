using System;

namespace Zeus.ContentTypes.Properties
{
	public class StringDetail : ContentDetail<string>
	{
		#region Constuctors

		public StringDetail()
			: base()
		{

		}

		public StringDetail(ContentItem containerItem, string name, string value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion
	}
}
