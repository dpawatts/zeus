using System;

namespace Zeus.Details
{
	public class StringDetail : ContentDetail
	{
		#region Constuctors

		public StringDetail()
		{
		}

		public StringDetail(ContentItem containerItem, string name, string value)
		{
			ID = 0;
			EnclosingItem = containerItem;
			Name = name;
			StringValue = value;
		}

		#endregion

		#region Properties

		public virtual string StringValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return StringValue; }
			set { StringValue = (string) value; }
		}

		public override Type ValueType
		{
			get { return typeof(string); }
		}

		#endregion
	}
}
