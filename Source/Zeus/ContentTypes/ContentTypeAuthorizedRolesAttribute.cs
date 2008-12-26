using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeus.Security;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ContentTypeAuthorizedRolesAttribute : AuthorizedRolesAttribute, IInheritableDefinitionRefiner
	{
		/// <summary>Initializes a new ContentTypeAuthorizedRolesAttribute used to restrict permission to create items in edit mode.</summary>
		public ContentTypeAuthorizedRolesAttribute()
		{
		}

		/// <summary>Initializes a new ContentTypeAuthorizedRolesAttribute used to restrict permission to create items in edit mode.</summary>
		/// <param name="roles">The roles allowed to edit the decorated item.</param>
		public ContentTypeAuthorizedRolesAttribute(params string[] roles)
			: base(roles)
		{
		}

		public void Refine(ContentType definition, IList<ContentType> allDefinitions)
		{
			definition.AuthorizedRoles = Roles;
		}
	}
}
