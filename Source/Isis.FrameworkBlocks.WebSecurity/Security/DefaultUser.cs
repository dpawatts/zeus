namespace Isis.Web.Security
{
	public class DefaultUser : IUser
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string[] Roles { get; set; }
	}
}