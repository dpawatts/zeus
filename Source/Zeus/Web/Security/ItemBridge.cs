using System;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Linq;
using System.Configuration;
using Zeus.Configuration;
using System.Web.Compilation;

namespace Zeus.Web.Security
{
	/// <summary>
	/// Provides access to users and roles stored as nodes in the item 
	/// hierarchy.
	/// </summary>
	public class ItemBridge
	{
		private readonly IContentTypeManager definitions;
		private readonly IPersister persister;
		private readonly Host host;
		private readonly IItemFinder finder;
		private string userContainerName = "Users";
		private string[] defaultRoles = new string[] { "Everyone", "Members", "Editors", "Administrators" };

		public ItemBridge(IContentTypeManager definitions, IPersister persister, Host host, IItemFinder finder)
		{
			this.definitions = definitions;
			this.persister = persister;
			this.host = host;
			this.finder = finder;
		}

		protected int UserContainerParentID
		{
			get { return host.RootItemID; }
		}

		public string UserContainerName
		{
			get { return userContainerName; }
			set { userContainerName = value; }
		}

		public string[] DefaultRoles
		{
			get { return defaultRoles; }
			set { defaultRoles = value; }
		}

		public IItemFinder Finder
		{
			get { return finder; }
		}

		public virtual Items.User CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey)
		{
			Items.User u = CreateUserObject();
			u.Title = username;
			u.Name = username;
			u.Password = password;
			u.Email = email;
			u.PasswordQuestion = passwordQuestion;
			u.PasswordAnswer = passwordAnswer;
			u.IsApproved = isApproved;
			u.ProviderUserKey = providerUserKey;

			persister.Save(u);

			return u;
		}

		protected virtual Items.User CreateUserObject()
		{
			Items.UserContainer parent = GetUserContainer(true);

			MembershipSection membershipSection = ConfigurationManager.GetSection("zeus/membership") as MembershipSection;
			if (membershipSection != null && !string.IsNullOrEmpty(membershipSection.CustomUserClass))
				return (Items.User) definitions.CreateInstance(BuildManager.GetType(membershipSection.CustomUserClass, true), parent);
			else
				return definitions.CreateInstance<Items.User>(parent);
		}

		public virtual Items.User GetUser(string username)
		{
			Items.UserContainer users = GetUserContainer(false);
			if (users == null)
				return null;
			return users.GetChild(username) as Items.User;
		}

		public virtual Items.UserContainer GetUserContainer(bool create)
		{
			ContentItem parent = persister.Get(UserContainerParentID);
			Items.UserContainer m = parent.GetChild(UserContainerName) as Items.UserContainer;
			if (m == null && create)
				m = CreateUserContainer(parent);
			return m;
		}

		protected Items.UserContainer CreateUserContainer(ContentItem parent)
		{
			Items.UserContainer m = Context.ContentTypes.CreateInstance<Items.UserContainer>(parent);
			m.Title = "Users";
			m.Name = UserContainerName;
			foreach (string role in DefaultRoles)
				m.AddRole(role);

			persister.Save(m);
			return m;
		}

		public virtual void Delete(ContentItem item)
		{
			persister.Delete(item);
		}

		public virtual void Save(ContentItem item)
		{
			persister.Save(item);
		}
	}
}
