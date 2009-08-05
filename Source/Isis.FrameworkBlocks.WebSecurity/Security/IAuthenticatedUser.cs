namespace Isis.Web.Security
{
	public interface IAuthenticatedUser
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
	}
}
