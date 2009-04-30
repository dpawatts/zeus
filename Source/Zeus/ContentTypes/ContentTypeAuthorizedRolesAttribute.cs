using System;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ContentTypeAuthorizedRolesAttribute : AbstractContentTypeRefiner, IInheritableDefinitionRefiner
	{
		/// <summary>Initializes a new ContentTypeAuthorizedRolesAttribute used to restrict permission to create items in edit mode.</summary>
		public ContentTypeAuthorizedRolesAttribute()
		{
		}

		public string[] Roles { get; set; }

		/// <summary>Initializes a new ContentTypeAuthorizedRolesAttribute used to restrict permission to create items in edit mode.</summary>
		/// <param name="roles">The roles allowed to edit the decorated item.</param>
		public ContentTypeAuthorizedRolesAttribute(params string[] roles)
		{
			Roles = roles;
		}

		public override void Refine(ContentType definition, IList<ContentType> allDefinitions)
		{
			definition.AuthorizedRoles = Roles;
		}
	}
}