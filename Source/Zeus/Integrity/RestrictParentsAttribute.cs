using System;
using Zeus.ContentTypes;
using System.Collections.Generic;

namespace Zeus.Integrity
{
	/// <summary>
	/// A class decoration used to restrict which items may be placed under which.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RestrictParentsAttribute : TypeIntegrityAttribute, IInheritableDefinitionRefiner
	{
		/// <summary>Initializes a new instance of the RestrictParentsAttribute which is used to restrict which types of items may be added below which.</summary>
		public RestrictParentsAttribute()
		{
			RefinementOrder = RefineOrder.Before;
		}

		/// <summary>Initializes a new instance of the RestrictParentsAttribute which is used to restrict which types of items may be added below which.</summary>
		/// <param name="allowedTypes">Defines wether all types of items are allowed as parent items.</param>
		public RestrictParentsAttribute(AllowedTypes allowedTypes)
			: this()
		{
			if (allowedTypes == AllowedTypes.All)
				Types = null;
			else
				Types = new Type[0];
		}

		/// <summary>Initializes a new instance of the RestrictParentsAttribute which is used to restrict which types of items may be added below which.</summary>
		/// <param name="allowedParentTypes">A list of allowed types. Null is interpreted as all types are allowed.</param>
		public RestrictParentsAttribute(params Type[] allowedParentTypes)
			: this()
		{
			Types = allowedParentTypes;
		}

		/// <summary>Changes allowed parents on the item definition.</summary>
		/// <param name="currentDefinition">The definition to alter.</param>
		/// <param name="allDefinitions">All definitions.</param>
		public override void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			foreach (ContentType definition in allDefinitions)
			{
				bool assignable = IsAssignable(definition.ItemType);
				if (assignable)
					definition.AddAllowedChild(currentDefinition);
			}
		}
	}
}
