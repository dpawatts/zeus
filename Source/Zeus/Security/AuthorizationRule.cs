using System;
using System.Security.Principal;

namespace Zeus.Security
{
	public class AuthorizationRule : ICloneable
	{
		/// <summary>The role considered as everyone.</summary>
		public const string Everyone = "Everyone";

		#region Constructors

		public AuthorizationRule() {}

		/// <summary>Creates a new instance of the AuthorizedRole class associating it with a content item and defining the role name.</summary>
		/// <param name="item">The item this role is associated with.</param>
		/// <param name="operation"></param>
		/// <param name="roleOrUser">The role or user name.</param>
		/// <param name="type"></param>
		public AuthorizationRule(ContentItem item, string operation, string roleOrUser, AuthorizationType type, bool allowed)
			: this(item, operation, (type == AuthorizationType.Role) ? roleOrUser : null, (type == AuthorizationType.User) ? roleOrUser : null, allowed)
		{
			
		}

		/// <summary>Creates a new instance of the AuthorizedRole class associating it with a content item and defining the role name.</summary>
		/// <param name="item">The item this role is associated with.</param>
		/// <param name="operation"></param>
		/// <param name="role">The role or user name.</param>
		/// <param name="user"></param>
		/// <param name="allowed"></param>
		public AuthorizationRule(ContentItem item, string operation, string role, string user, bool allowed)
		{
			EnclosingItem = item;
			Operation = operation;
			Role = role;
			User = user;
			Allowed = allowed;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the database identifier of this class.</summary>
		public virtual int ID { get; set; }

		/// <summary>Gets or sets the item this AuthorizedRole referrs to.</summary>
		public virtual ContentItem EnclosingItem { get; set; }

		/// <summary>Gets the operation (i.e. Read, Edit, Publish) this class referrs to.</summary>
		public virtual string Operation { get; set; }

		/// <summary>Gets the role name this class refers to.</summary>
		public virtual string Role { get; set; }

		/// <summary>Gets the user name this class refers to.</summary>
		public virtual string User { get; set; }

		/// <summary>
		/// Gets or sets whether this the specified user or role is allowed to perform this operation.
		/// If a rule does not exist for a particular operation, the default is that the user or role
		/// is NOT authorized.
		/// </summary>
		public virtual bool Allowed { get; set; }

		/// <summary>Gets wether this role refers to everyone, i.e. the unauthenticated user.</summary>
		public virtual bool IsEveryone
		{
			get { return Role == Everyone; }
		}

		#endregion

		#region ICloneable Members

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion

		/// <summary>Determines whether a user is permitted according to this rule.</summary>
		/// <param name="user">The user to check.</param>
		/// <param name="operation"></param>
		/// <returns>True if the user is permitted.</returns>
		public bool IsAuthorized(IPrincipal user, string operation)
		{
			if (!Allowed)
				return false;

			if (IsEveryone && (operation == null || operation == Operation))
				return true;

			if (user != null && (operation == null || operation == Operation))
			{
				if (user.IsInRole(Role))
					return true;
				if (user.Identity.Name == User)
					return true;
			}

			return false;
		}

		/// <summary>Copies this AuthorizedRole clearing id and enclosing item.</summary>
		/// <returns>A copy of this AuthorizedRole.</returns>
		public virtual AuthorizationRule Clone()
		{
			AuthorizationRule cloned = (AuthorizationRule) MemberwiseClone();
			cloned.ID = 0;
			cloned.EnclosingItem = null;
			return cloned;
		}
	}

	public enum AuthorizationType
	{
		Role,
		User
	}
}