using System;

namespace Zeus.Details
{
	public class DateTimeDetail : ContentDetail
	{
		#region Constuctors

		public DateTimeDetail()
		{
		}

		public DateTimeDetail(ContentItem containerItem, string name, DateTime value)
		{
			this.ID = 0;
			this.EnclosingItem = containerItem;
			this.Name = name;
			this.DateTimeValue = value;
		}
		#endregion

		#region Properties

		public virtual DateTime DateTimeValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.DateTimeValue; }
			set { this.DateTimeValue = (DateTime) value; }
		}

		public override Type ValueType
		{
			get { return typeof(DateTime); }
		}
		#endregion
	}
}
