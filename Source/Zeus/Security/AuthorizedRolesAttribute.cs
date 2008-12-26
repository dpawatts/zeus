using System;

namespace Zeus.Security
{
	/// <summary>
	/// This class is a base class for attributes used to restrict permissions.
	/// </summary>
	public abstract class AuthorizedRolesAttribute : Attribute
	{
		public AuthorizedRolesAttribute()
		{
		}

		public AuthorizedRolesAttribute(params string[] authorizedRoles)
		{
			this.Roles = authorizedRoles;
		}

		public string[] Roles
		{
			get;
			set;
		}
	}
}
