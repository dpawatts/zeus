using System;

namespace Zeus.ContentTypes.Properties
{
	public class IntegerDetail : ContentDetail<int>
	{
		#region Constructors

		public IntegerDetail()
			: base()
		{
		}

		public IntegerDetail(ContentItem containerItem, string name, int value)
			: base(containerItem, name, value)
		{

		}

		#endregion
	}
}
