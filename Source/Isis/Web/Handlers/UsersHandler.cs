using System;
using System.Web;
using System.Web.Security;

namespace Isis.Web.Handlers
{
	public class UsersHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			if (!context.Request.IsLocal)
				throw new Exception("Only accessible from local computer");

			string[] roles = Roles.GetAllRoles();

			if (context.Request.QueryString["editsubmit"] != null)
			{
				MembershipUser membershipUser = Membership.GetUser(context.Request.QueryString["username"]);
				membershipUser.ChangePassword(membershipUser.GetPassword(), context.Request.QueryString["password"]);

				string[] existingRoles = Roles.GetRolesForUser(context.Request.QueryString["username"]);
				Roles.RemoveUserFromRoles(context.Request.QueryString["username"], existingRoles);
				foreach (string role in roles)
				{
					if (context.Request.QueryString["role" + role] != null)
						Roles.AddUserToRole(membershipUser.UserName, role);
				}
			}
			else if (context.Request.QueryString["username"] != null)
			{
				MembershipUser membershipUser = Membership.CreateUser(context.Request.QueryString["username"], context.Request.QueryString["password"]);

				foreach (string role in roles)
				{
					if (context.Request.QueryString["role" + role] != null)
						Roles.AddUserToRole(membershipUser.UserName, role);
				}
			}
			else if (context.Request.QueryString["delete"] != null)
			{
				Membership.DeleteUser(context.Request.QueryString["delete"], true);
			}
			else if (context.Request.QueryString["unlock"] != null)
			{
				MembershipUser membershipUser = Membership.GetUser(context.Request.QueryString["unlock"]);
				membershipUser.UnlockUser();
			}

			context.Response.Write("<h1>Existing Users</h1>");
			context.Response.Write("<table border='1'>");
			context.Response.Write("<tr>");
			context.Response.Write("<th>UserName</th>");
			context.Response.Write("<th>Password</th>");
			context.Response.Write("<th>IsLockedOut</th>");
			context.Response.Write("<th>LastLoginDate</th>");
			context.Response.Write("<th>Roles</th>");
			context.Response.Write("<th>Unlock</th>");
			context.Response.Write("<th>Edit</th>");
			context.Response.Write("<th>Delete</th>");
			context.Response.Write("</tr>");

			MembershipUserCollection users = Membership.GetAllUsers();
			foreach (MembershipUser membershipUser in users)
			{
				context.Response.Write("<tr>");
				context.Response.Write(string.Format("<td>{0}</td>", membershipUser.UserName));
				context.Response.Write(string.Format("<td>{0}</td>", membershipUser.GetPassword()));
				context.Response.Write(string.Format("<td>{0}</td>", membershipUser.IsLockedOut));
				context.Response.Write(string.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", membershipUser.LastLoginDate));
				context.Response.Write(string.Format("<td>{0}</td>", string.Join(", ", Roles.GetRolesForUser(membershipUser.UserName))));
				context.Response.Write(string.Format("<td><a href=\"{0}?unlock={1}\">Unlock</a></td>", context.Request.Path, membershipUser.UserName));
				context.Response.Write(string.Format("<td><a href=\"{0}?edit={1}\">Edit</a></td>", context.Request.Path, membershipUser.UserName));
				context.Response.Write(string.Format("<td><a href=\"{0}?delete={1}\">Delete</a></td>", context.Request.Path, membershipUser.UserName));
				context.Response.Write("</tr>");
			}

			context.Response.Write("</table>");

			MembershipUser existingUser = null;
			if (context.Request.QueryString["edit"] != null)
			{
				context.Response.Write("<h1>Edit User</h1>");
				existingUser = Membership.GetUser(context.Request.QueryString["edit"]);
			}
			else
				context.Response.Write("<h1>Add User</h1>");

			context.Response.Write("<form method=\"get\">");

			if (context.Request.QueryString["edit"] != null)
			{
				context.Response.Write("<td><input type=\"hidden\" name=\"editsubmit\" value=\"true\" /></td>");
				context.Response.Write("<td><input type=\"hidden\" name=\"username\" value=\"" + existingUser.UserName + "\" /></td>");
			}

			context.Response.Write("<table border='1'>");

			if (context.Request.QueryString["edit"] != null)
			{
				context.Response.Write("<tr>");
				context.Response.Write("<td>UserName</td>");
				context.Response.Write("<td>" + existingUser.UserName + "</td>");
				context.Response.Write("</tr>");
			}
			else
			{
				context.Response.Write("<tr>");
				context.Response.Write("<td>UserName</td>");
				context.Response.Write("<td><input type=\"text\" id=\"username\" name=\"username\" /></td>");
				context.Response.Write("</tr>");
			}

			context.Response.Write("<tr>");
			context.Response.Write("<td>Password</td>");

			if (context.Request.QueryString["edit"] != null)
				context.Response.Write("<td><input type=\"text\" id=\"password\" name=\"password\" value=\"" + existingUser.GetPassword() + "\" /></td>");
			else
				context.Response.Write("<td><input type=\"text\" id=\"password\" name=\"password\" /></td>");

			context.Response.Write("</tr>");
			context.Response.Write("<tr>");
			context.Response.Write("<td>Roles</td>");
			context.Response.Write("<td>");

			foreach (string role in roles)
			{
				if (context.Request.QueryString["edit"] != null)
					context.Response.Write("<input type=\"checkbox\" id=\"role" + role + "\" name=\"role" + role + "\" checked=\"" + Roles.IsUserInRole(existingUser.UserName, role) + "\" />" + role + "<br />");
				else
					context.Response.Write("<input type=\"checkbox\" id=\"role" + role + "\" name=\"role" + role + "\" />" + role + "<br />");
			}

			context.Response.Write("</td>");
			context.Response.Write("</tr>");
			context.Response.Write("<tr>");
			context.Response.Write("<td></td>");

			if (context.Request.QueryString["edit"] != null)
				context.Response.Write("<td><input type=\"submit\" value=\"Update\" /></td>");
			else
				context.Response.Write("<td><input type=\"submit\" value=\"Add\" /></td>");

			context.Response.Write("</tr>");
			context.Response.Write("</table>");
			context.Response.Write("</form>");
		}
	}
}
