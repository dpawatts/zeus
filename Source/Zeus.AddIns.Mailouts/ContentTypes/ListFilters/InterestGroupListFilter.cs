using System;
using System.ComponentModel;
using System.Linq;
using Zeus.AddIns.Mailouts.Design.Editors;
using Zeus.AddIns.Mailouts.Services;
using Zeus.ContentTypes;
using Zeus.Design.Editors;

namespace Zeus.AddIns.Mailouts.ContentTypes.ListFilters
{
	[ContentType("Interest Group List Filter")]
	[ContentTypeAuthorizedRoles("Administrators")]
	public class InterestGroupListFilter : ListFilter
	{
		[EnumEditor("Operator", 10, typeof(InterestGroupOperator))]
		public virtual int Operator
		{
			get { return GetDetail("Operator", (int) InterestGroupOperator.OneOf); }
			set { SetDetail("Operator", value); }
		}

		[InterestGroupListBoxEditor("Interest Group(s)", 20, Required = true)]
		public virtual string[] InterestGroups
		{
			get { return GetDetail("InterestGroups", new string[] {}); }
			set { SetDetail("InterestGroups", value); }
		}

		public override bool Matches(IMailoutRecipient recipient)
		{
			switch ((InterestGroupOperator) Operator)
			{
				case InterestGroupOperator.OneOf:
					return recipient.InterestGroups.Intersect(InterestGroups).Any();
				case InterestGroupOperator.AllOf:
					return recipient.InterestGroups.Intersect(InterestGroups).Count() == InterestGroups.Count();
				case InterestGroupOperator.NoneOf:
					return !recipient.InterestGroups.Intersect(InterestGroups).Any();
				default :
					throw new InvalidOperationException();
			}
		}

		public enum InterestGroupOperator
		{
			[Description("One of")]
			OneOf,

			[Description("All of")]
			AllOf,

			[Description("None of")]
			NoneOf
		}
	}
}