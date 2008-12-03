using System;

namespace Zeus.Details
{
	public class IntegerDetail : ContentDetail
	{
		#region Constructors

		public IntegerDetail()
		{
		}

		public IntegerDetail(ContentItem containerItem, string name, int value)
		{
			this.ID = 0;
			this.EnclosingItem = containerItem;
			this.Name = name;
			this.IntValue = value;
		}

		#endregion

		#region Properties

		public virtual int IntValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.IntValue; }
			set { this.IntValue = (int) value; }
		}

		public override Type ValueType
		{
			get { return typeof(int); }
		}

		#endregion
	}
}
