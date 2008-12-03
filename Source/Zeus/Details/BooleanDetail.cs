using System;

namespace Zeus.Details
{
	public class BooleanDetail : ContentDetail
	{
		#region Constuctors

		public BooleanDetail()
		{
		}

		public BooleanDetail(ContentItem containerItem, string name, bool value)
		{
			ID = 0;
			EnclosingItem = containerItem;
			Name = name;
			BoolValue = value;
		}

		#endregion

		#region Properties

		public virtual bool BoolValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.BoolValue; }
			set { this.BoolValue = (bool) value; }
		}

		public override Type ValueType
		{
			get { return typeof(bool); }
		}

		#endregion
	}
}
