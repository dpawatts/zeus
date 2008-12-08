using System;

namespace Zeus.ContentTypes.Properties
{
	public class DateTimeDetail : ContentDetail<DateTime>
	{
		#region Constuctors

		public DateTimeDetail()
			: base()
		{

		}

		public DateTimeDetail(ContentItem containerItem, string name, DateTime value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion
	}
}
