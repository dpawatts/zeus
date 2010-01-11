using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Zeus.BaseLibrary.Security;
using Zeus.ContentTypes;
using Zeus.Net.Mail;
using Zeus.Persistence;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class CredentialService : ICredentialService
	{
		public const string VerificationLinkName = "{VERIFICATIONLINK}";
		public const string PasswordResetLinkName = "{PASSWORDRESETLINK}";

		private readonly TimeSpan _passwordResetRequestTimeout = TimeSpan.FromMinutes(20);

		private readonly ICredentialStore _store;
		private readonly IMailSender _mailSender;
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IPersister _persister;

		public CredentialService(ICredentialStore store, IMailSender mailSender,
			IContentTypeManager contentTypeManager,
			IPersister persister)
		{
			_store = store;
			_mailSender = mailSender;
			_contentTypeManager = contentTypeManager;
			_persister = persister;
		}

		public User CreateUser(string username, string password, string email, string[] roles,
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

			// Check if email is already present.
			if (_store.GetUserByEmail(email) != null)
			{
				createStatus = UserCreateStatus.DuplicateEmail;
				return null;
			}

			// Create user.
			_store.CreateUser(FormatUsername(username), EncryptPassword(password), roles, email, isVerified);
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

		public IEnumerable<User> GetAllUsers()
		{
			return _store.GetAllUsers();
		}

		public User GetUser(string username)
		{
			return _store.GetUser(FormatUsername(username));
		}

		public bool ValidateUser(string username, string password)
		{
			// Get user from store.
			User user = _store.GetUser(FormatUsername(username));
			return (user != null && user.Password == EncryptPassword(password) && user.Verified);
		}

		private static string FormatUsername(string username)
		{
			return username.ToLower();
		}

		public string EncryptPassword(string password)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(password,
				FormsAuthPasswordFormat.SHA1.ToString());
		}

		public void SendVerificationEmail(User user, string linkRoot, string recipientEmail,
			string senderEmail, string emailSubject, string emailBody)
		{
			// Check that emailBody contains {VERIFICATIONLINK}.
			if (!emailBody.Contains(VerificationLinkName))
				throw new ArgumentException("Email body must contain " + VerificationLinkName, "emailBody");

			// Construct nonce.
			string nonce = NonceUtility.GenerateNonce();
			string verificationLink = linkRoot + HttpUtility.UrlEncode(nonce);

			// Save nonce.
			_store.SaveNonce(user, nonce);

			// Construct email.
			emailBody = emailBody.Replace(VerificationLinkName, verificationLink);

			try
			{
				_mailSender.Send(senderEmail, recipientEmail, emailSubject, emailBody);
			}
			catch (Exception ex)
			{
				throw new ZeusException("Failed to send verification email to '" + recipientEmail + "'", ex);
			}
		}

		public UserVerificationResult Verify(string nonce, out User user)
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

		public PasswordResetRequestResult SendPasswordResetEmail(string username, string linkRoot,
			string senderEmail, string emailSubject, string emailBody)
		{
			// Check that emailBody contains {PASSWORDRESETLINK}.
			if (!emailBody.Contains(PasswordResetLinkName))
				throw new ArgumentException("Email body must contain " + PasswordResetLinkName, "emailBody");

			// Check that user exists.
			User user = GetUser(username);
			if (user == null)
				return PasswordResetRequestResult.UserNotFound;

			// Check that there isn't an existing password reset request.
			var existingRequests = user.GetChildren<PasswordResetRequest>();
			if (existingRequests.Any(prr => !prr.Used && prr.Created.Add(_passwordResetRequestTimeout) > DateTime.Now))
				return PasswordResetRequestResult.RequestExists;

			// Check that there aren't too many existing reset requests.
			if (existingRequests.Count(prr => prr.Created > DateTime.Now.AddHours(-1)) > 3)
				return PasswordResetRequestResult.TooManyRequests;

			// Construct nonce.
			string nonce = NonceUtility.GenerateNonce();
			string passwordResetLink = linkRoot + HttpUtility.UrlEncode(nonce);

			// Create a password reset request.
			PasswordResetRequest resetRequest = _contentTypeManager.CreateInstance<PasswordResetRequest>(user);
			resetRequest.Nonce = nonce;
			_persister.Save(resetRequest);

			// Construct email.
			emailBody = emailBody.Replace(PasswordResetLinkName, passwordResetLink);

			try
			{
				_mailSender.Send(senderEmail, user.Email, emailSubject, emailBody);
				return PasswordResetRequestResult.Sent;
			}
			catch (Exception ex)
			{
				throw new ZeusException("Failed to send password reset email to '" + user.Email + "'", ex);
			}
		}

		public PasswordResetRequestValidity CheckPasswordResetRequestValidity(string nonce, out PasswordResetRequest resetRequest)
		{
			resetRequest = null;
			if (string.IsNullOrEmpty(nonce))
				return PasswordResetRequestValidity.NoMatchingRequest;

			// Get user from store matching password reset request nonce.
			resetRequest = _store.GetPasswordResetRequestByNonce(nonce);
			if (resetRequest == null)
				return PasswordResetRequestValidity.NoMatchingRequest;

			if (resetRequest.Used)
				return PasswordResetRequestValidity.AlreadyUsed;

			if (resetRequest.Created.Add(_passwordResetRequestTimeout) < DateTime.Now)
				return PasswordResetRequestValidity.Expired;

			return PasswordResetRequestValidity.Valid;
		}

		public PasswordResetResult ResetPassword(string nonce, string newPassword)
		{
			PasswordResetRequest resetRequest;
			if (CheckPasswordResetRequestValidity(nonce, out resetRequest) != PasswordResetRequestValidity.Valid)
				return PasswordResetResult.Failed;

			User user = (User) resetRequest.Parent;
			user.Password = EncryptPassword(newPassword);

			resetRequest.Used = true;
			_persister.Save(resetRequest);

			return PasswordResetResult.Succeeded;

		}
	}
}