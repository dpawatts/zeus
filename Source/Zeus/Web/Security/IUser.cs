namespace Zeus.Web.Security
{
	public interface IUser
	{
		string Identifier
		{
			get;
		}

		bool Verified
		{
			get;
		}

		string Username
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}

		string[] Roles
		{
			get;
		}
	}
}