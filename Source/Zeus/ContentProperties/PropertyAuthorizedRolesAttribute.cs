using System;

namespace Zeus.ContentProperties
{
	/// <summary>This class is used to restrict access to certain details in edit mode. Only users in the specified roles can edit details decorated with this attribute.</summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyAuthorizedRolesAttribute : Attribute
	{
		public string[] Roles { get; set; }

		/// <summary>Initializes a new instance of the DetailAuthorizedRolesAttribute used to restrict access to details in edit mode.</summary>
		public PropertyAuthorizedRolesAttribute()
		{
		}

		/// <summary>Initializes a new instance of the DetailAuthorizedRolesAttribute used to restrict access to details in edit mode.</summary>
		/// <param name="roles">The roles allowed to edit the decorated detail.</param>
		public PropertyAuthorizedRolesAttribute(params string[] roles)
		{
			Roles = roles;
		}
	}
}