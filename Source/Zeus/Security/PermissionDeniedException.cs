using System.Security.Principal;

namespace Zeus.Security
{
	/// <summary>
	/// An exeption thrown when a user tries to access an unauthorized item.
	/// </summary>
	public class PermissionDeniedException : ZeusException
	{
		public PermissionDeniedException(ContentItem item, IPrincipal user, string operation)
			: base(string.Format("Permission denied for user '{0}' to perform operation '{1}' on content item ID '{2}'", user.Identity.Name, operation, item.ID))
		{
			User = user;
			Item = item;
		}

		public PermissionDeniedException(string message, PermissionDeniedException innerException)
			: base(message, innerException)
		{
			User = innerException.User;
			Item = innerException.Item;
		}

		#region Properties

		/// <summary>Gets the user which caused the exception.</summary>
		public IPrincipal User { get; private set; }

		/// <summary>Gets the item that caused the exception.</summary>
		public ContentItem Item { get; private set; }

		#endregion
	}
}