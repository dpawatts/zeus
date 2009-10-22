using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Zeus.Configuration;
using AuthenticationSection=Zeus.Configuration.AuthenticationSection;

namespace Zeus.Web.Security
{
	public class DefaultCredentialRepository : ICredentialRepository
	{
		public void CreateUser(string username, string password, string[] roles)
		{
			throw new NotSupportedException();
		}

		public IEnumerable<string> GetAllRoles()
		{
			throw new NotSupportedException();
		}

		public IEnumerable<IUser> GetAllUsers()
		{
			AuthenticationSection configSection = WebConfigurationManager.GetSection("zeus/authentication") as AuthenticationSection;
			if (configSection == null || configSection.Credentials.Count == 0)
				return new List<IUser>();

			return configSection.Credentials.Cast<UserElement>()
				.Select(ue => new DefaultUser { Username = ue.Name, Password = ue.Password })
				.Cast<IUser>();
		}

		public IUser GetUser(string username)
		{
			return GetAllUsers().SingleOrDefault(u => u.Username == username);
		}
	}
}