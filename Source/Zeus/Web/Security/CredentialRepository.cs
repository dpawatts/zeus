using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Isis.Web.Security;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.Security;
using Zeus.Web.Security.Items;

namespace Zeus.Web.Security
{
	public class CredentialRepository : ICredentialRepository, IWebSecurityManager
	{
		#region Fields

		private readonly IHost _host;
		private readonly IPersister _persister;

		#endregion

		#region Constructor

		public CredentialRepository(IPersister persister, IHost host)
		{
			_persister = persister;
			_host = host;

			DefaultRoles = new[] {"Everyone", "Members", "Editors", "Administrators"};
		}

		#endregion

		#region Properties

		protected int SecurityContainerParentID
		{
			get { return _host.CurrentSite.RootItemID; }
		}

		public string[] DefaultRoles { get; set; }

		#endregion

		#region Methods

		public Role GetRole(string roleName)
		{
			RoleContainer roles = GetRoleContainer(false);
			return roles.GetChild(roleName) as Role;
		}

		public IEnumerable<Role> GetRoles(IPrincipal user)
		{
			RoleContainer roleContainer = GetRoleContainer(true);
			return roleContainer.GetChildren().Authorized(user, Context.SecurityManager, Operations.Read).Cast<Role>();
		}

		void ICredentialRepository.CreateUser(string username, string password, string[] roles)
		{
			UserContainer userContainer = GetUserContainer(true);
			RoleContainer roleContainer = GetRoleContainer(true);

			User user = new User {Name = username, Password = password};
			foreach (string role in roles)
				user.Roles.Add(roleContainer.GetChild(role));
			user.AddTo(userContainer);

			Context.Persister.Save(user);
		}

		IUser ICredentialRepository.GetUser(string username)
		{
			return GetUser(username);
		}

		IEnumerable<string> ICredentialRepository.GetAllRoles()
		{
			RoleContainer roles = GetRoleContainer(false);
			return roles == null ? null : roles.GetRoleNames();
		}

		IEnumerable<IUser> ICredentialRepository.GetAllUsers()
		{
			UserContainer users = GetUserContainer(false);
			return users == null ? null : users.GetChildren().Cast<IUser>();
		}

		private User GetUser(string username)
		{
			UserContainer users = GetUserContainer(false);
			if (users == null)
				return null;
			return users.GetChild(username) as User;
		}

		private UserContainer GetUserContainer(bool create)
		{
			SecurityContainer security = GetSecurityContainer(create);
			if (security != null)
				return (UserContainer) security.GetChild(UserContainer.ContainerName);
			return null;
		}

		private RoleContainer GetRoleContainer(bool create)
		{
			SecurityContainer security = GetSecurityContainer(create);
			if (security != null)
				return (RoleContainer) security.GetChild(RoleContainer.ContainerName);
			return null;
		}

		private SecurityContainer GetSecurityContainer(bool create)
		{
			ContentItem parent = _persister.Get(SecurityContainerParentID);
			SecurityContainer m = parent.GetChild(SecurityContainer.ContainerName) as SecurityContainer;
			if (m == null && create)
				m = CreateSecurityContainer(parent);
			return m;
		}

		private SecurityContainer CreateSecurityContainer(ContentItem parent)
		{
			SecurityContainer security = Context.ContentTypes.CreateInstance<SecurityContainer>(parent);

			RoleContainer roles = Context.ContentTypes.CreateInstance<RoleContainer>(security);
			roles.AddTo(security);

			foreach (string role in DefaultRoles)
				roles.AddRole(role);

			UserContainer users = Context.ContentTypes.CreateInstance<UserContainer>(security);
			users.AddTo(security);

			_persister.Save(security);
			return security;
		}

		#endregion
	}
}