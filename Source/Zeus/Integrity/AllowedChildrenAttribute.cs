using System;
using System.Collections.Generic;
using Zeus.ContentTypes;

namespace Zeus.Integrity
{
	/// <summary>
	/// A class decoration used to define which items are allowed below this 
	/// item. When this attribute intersects with 
	/// <see cref="RestrictParentsAttribute"/>, the union of these two are 
	/// considered to be allowed.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class AllowedChildrenAttribute : TypeIntegrityAttribute, IInheritableDefinitionRefiner
	{
		/// <summary>Initializes a new instance of the AllowedChildrenAttribute which is used to restrict which types of items may be added below which.</summary>
		public AllowedChildrenAttribute()
		{
			RefinementOrder = RefineOrder.After;
		}

		/// <summary>Initializes a new instance of the AllowedChildrenAttribute which is used to restrict which types of items may be added below which.</summary>
		/// <param name="allowedChildTypes">A list of allowed types. Null is interpreted as all types are allowed.</param>
		public AllowedChildrenAttribute(params Type[] allowedChildTypes)
			: this()
		{
			Types = allowedChildTypes;
		}

		public override void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			foreach (ContentType definition in allDefinitions)
			{
				bool assignable = IsAssignable(definition.ItemType);
				if (assignable)
					currentDefinition.AddAllowedChild(definition);
			}
		}
	}
}