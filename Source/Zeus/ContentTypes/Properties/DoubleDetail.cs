using System;

namespace Zeus.ContentTypes.Properties
{
	public class DoubleDetail : ContentDetail<double>
	{
		#region Constuctors

		public DoubleDetail()
			: base()
		{
		}

		public DoubleDetail(ContentItem containerItem, string name, double value)
			: base(containerItem, name, value)
		{

		}

		#endregion
	}
}
