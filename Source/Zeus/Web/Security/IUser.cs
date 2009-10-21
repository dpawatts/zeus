namespace Zeus.Web.Security
{
	public interface IUser
	{
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