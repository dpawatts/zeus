using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Isis.Web.Security
{
	public class CredentialService : ICredentialService
	{
		private readonly ICredentialRepository _repository;

		public CredentialService(ICredentialRepository repository)
		{
			_repository = repository;
		}

		public IUser CreateUser(string username, string password, string[] roles, out UserCreateStatus createStatus)
		{
			// Validate username.
			if (!ValidateParameter(username))
			{
				createStatus = UserCreateStatus.InvalidUserName;
				return null;
			}

			// Validate password.
			if (!ValidateParameter(password))
			{
				createStatus = UserCreateStatus.InvalidPassword;
				return null;
			}

			// Check if username is already present.
			if (_repository.GetUser(FormatUsername(username)) != null)
			{
				createStatus = UserCreateStatus.DuplicateUserName;
				return null;
			}

			// Create user.
			_repository.CreateUser(FormatUsername(username), password, roles);
			createStatus = UserCreateStatus.Success;
			return GetUser(username);
		}

		private static bool ValidateParameter(string value)
		{
			if (string.IsNullOrEmpty(value))
				return false;

			return Regex.IsMatch(value, "[a-z0-9]+", RegexOptions.IgnoreCase);
		}

		public IEnumerable<string> GetAllRoles()
		{
			return _repository.GetAllRoles();
		}

		public IEnumerable<IUser> GetAllUsers()
		{
			return _repository.GetAllUsers();
		}

		public IUser GetUser(string username)
		{
			return _repository.GetUser(FormatUsername(username));
		}

		public bool ValidateUser(string username, string password)
		{
			// Get user from repository.
			IUser user = _repository.GetUser(FormatUsername(username));
			return (user != null && user.Password == password);
		}

		private static string FormatUsername(string username)
		{
			return username.ToLower();
		}
	}
}