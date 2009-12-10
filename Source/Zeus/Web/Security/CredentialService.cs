using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeus.BaseLibrary.Security;
using Zeus.Net.Mail;

namespace Zeus.Web.Security
{
	public class CredentialService : ICredentialService
	{
		private readonly ICredentialStore _store;
		private readonly IMailSender _mailSender;

		public CredentialService(ICredentialStore store, IMailSender mailSender)
		{
			_store = store;
			_mailSender = mailSender;
		}

		public IUser CreateUser(string username, string password, string email, string[] roles,
			bool isVerified, out UserCreateStatus createStatus)
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
			if (_store.GetUser(FormatUsername(username)) != null)
			{
				createStatus = UserCreateStatus.DuplicateUserName;
				return null;
			}

			// Create user.
			_store.CreateUser(FormatUsername(username), password, roles, isVerified);
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
			return _store.GetAllRoles();
		}

		public IEnumerable<IUser> GetAllUsers()
		{
			return _store.GetAllUsers();
		}

		public IUser GetUser(string username)
		{
			return _store.GetUser(FormatUsername(username));
		}

		public bool ValidateUser(string username, string password)
		{
			// Get user from store.
			IUser user = _store.GetUser(FormatUsername(username));
			return (user != null && user.Password == password);
		}

		private static string FormatUsername(string username)
		{
			return username.ToLower();
		}

		public void SendVerificationEmail(IUser user, string linkRoot, string recipientEmail,
			string senderEmail, string emailSubject, string emailBody)
		{
			// Check that emailBody contains {VERIFICATIONLINK}.
			if (!emailBody.Contains("{VERIFICATIONLINK}"))
				throw new ArgumentException("Email body must contain {VERIFICATIONLINK}", "emailBody");

			// Construct nonce.
			string nonce = NonceUtility.GenerateNonce();
			string verificationLink = linkRoot + nonce;

			// Save nonce.
			_store.SaveNonce(user, nonce);

			// Construct email.
			emailBody = emailBody.Replace("{VERIFICATIONLINK}", verificationLink);

			try
			{
				_mailSender.Send(senderEmail, recipientEmail, emailSubject, emailBody);
			}
			catch (Exception ex)
			{
				throw new ZeusException("Failed to send verification email to '" + recipientEmail + "'", ex);
			}
		}

		public UserVerificationResult Verify(string nonce, out IUser user)
		{
			if (string.IsNullOrEmpty(nonce))
				throw new ArgumentNullException("nonce");

			// Get user from store matching nonce.
			user = _store.GetUserByNonce(nonce);
			if (user == null)
				return UserVerificationResult.NoMatchingUser;

			if (user.Verified)
				return UserVerificationResult.AlreadyVerified;

			_store.VerifyUser(user);
			return UserVerificationResult.Verified;
		}
	}
}