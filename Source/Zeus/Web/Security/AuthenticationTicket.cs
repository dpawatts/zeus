using System;

namespace Zeus.Web.Security
{
	[Serializable]
	public class AuthenticationTicket
	{
		#region Constructors

		public AuthenticationTicket(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, string userData, string cookiePath)
		{
			this.Version = version;
			this.Name = name;
			this.Expiration = expiration;
			this.IssueDate = issueDate;
			this.IsPersistent = isPersistent;
			this.UserData = userData;
			this.CookiePath = cookiePath;
		}

		#endregion

		#region Properties

		public string CookiePath
		{
			get;
			private set;
		}

		public DateTime Expiration
		{
			get;
			private set;
		}

		public bool Expired
		{
			get
			{
				return (this.Expiration < DateTime.Now);
			}
		}

		public bool IsPersistent
		{
			get;
			private set;
		}

		public DateTime IssueDate
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string UserData
		{
			get;
			private set;
		}

		public int Version
		{
			get;
			private set;
		}

		#endregion
	}
}