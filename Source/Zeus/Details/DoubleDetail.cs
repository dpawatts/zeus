using System;

namespace Zeus.Details
{
	public class DoubleDetail : ContentDetail
	{
		#region Constuctors

		public DoubleDetail()
		{
		}

		public DoubleDetail(ContentItem containerItem, string name, double value)
		{
			this.ID = 0;
			this.EnclosingItem = containerItem;
			this.Name = name;
			this.DoubleValue = value;
		}

		#endregion

		#region Properties

		public virtual double DoubleValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.DoubleValue; }
			set { this.DoubleValue = (double) value; }
		}

		public override Type ValueType
		{
			get { return typeof(double); }
		}

		#endregion
	}
}
