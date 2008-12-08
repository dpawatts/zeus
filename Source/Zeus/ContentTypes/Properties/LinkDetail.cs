using System;

namespace Zeus.ContentTypes.Properties
{
	public class LinkDetail : ContentDetail<ContentItem>
	{
		#region Constructors

		public LinkDetail()
			: base()
		{
		}

		public LinkDetail(ContentItem containerItem, string name, ContentItem value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion

		#region Properties

		public override ContentItem TypedValue
		{
			get { return base.TypedValue; }
			set
			{
				base.TypedValue = value;
				if (value != null)
					LinkValue = value.ID;
				else
					LinkValue = null;
			}
		}

		protected virtual int? LinkValue
		{
			get;
			set;
		}

		#endregion
	}
}
