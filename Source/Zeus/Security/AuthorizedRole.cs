using System;

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
	}
}
