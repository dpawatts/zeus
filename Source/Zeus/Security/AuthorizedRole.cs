using System;
using System.Security.Principal;

namespace Zeus.Security
{
	public class AuthorizedRole
	{
		#region Public Properties

		/// <summary>Gets or sets the database identifier of this class.</summary>
		public virtual int ID
		{
			get;
			set;
		}

		/// <summary>Gets or sets the item this AuthorizedRole referrs to.</summary>
		public virtual ContentItem EnclosingItem
		{
			get;
			set;
		}

		/// <summary>Gets the role name this class referrs to.</summary>
		public virtual string Role
		{
			get;
			set;
		}

		#endregion

		/// <summary>Determines wether a user is permitted according to this role.</summary>
		/// <param name="user">The user to check.</param>
		/// <returns>True if the user is permitted.</returns>
		public bool IsAuthorized(IPrincipal user)
		{
			if (user != null && user.IsInRole(Role))
				return true;
			return false;
		}
	}
}
