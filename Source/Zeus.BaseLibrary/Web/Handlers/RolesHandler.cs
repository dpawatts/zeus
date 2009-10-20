using System;
using System.Web;
using System.Web.Security;

namespace Isis.Web.Handlers
{
	public class RolesHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			if (!context.Request.IsLocal)
				throw new Exception("Only accessible from local computer");

			if (context.Request.QueryString["role"] != null)
			{
				Roles.CreateRole(context.Request.QueryString["role"]);
			}
			else if (context.Request.QueryString["delete"] != null)
			{
				Roles.DeleteRole(context.Request.QueryString["delete"], true);
			}

			context.Response.Write("<h1>Existing Roles</h1>");
			context.Response.Write("<table border='1'>");
			context.Response.Write("<tr>");
			context.Response.Write("<th>Role</th>");
			context.Response.Write("<th>Delete</th>");
			context.Response.Write("</tr>");

			string[] roles = Roles.GetAllRoles();
			foreach (string role in roles)
			{
				context.Response.Write("<tr>");
				context.Response.Write(string.Format("<td>{0}</td>", role));
				context.Response.Write(string.Format("<td><a href=\"{0}?delete={1}\">Delete</a></td>", context.Request.Path, role));
				context.Response.Write("</tr>");
			}

			context.Response.Write("</table>");

			context.Response.Write("<h1>Add Role</h1>");
			context.Response.Write("<form method=\"get\">");
			context.Response.Write("<table border='1'>");
			context.Response.Write("<tr>");
			context.Response.Write("<td>Role</td>");
			context.Response.Write("<td><input type=\"text\" id=\"role\" name=\"role\" /></td>");
			context.Response.Write("</tr>");
			context.Response.Write("<tr>");
			context.Response.Write("<td></td>");
			context.Response.Write("<td><input type=\"submit\" value=\"Add\" /></td>");
			context.Response.Write("</tr>");
			context.Response.Write("</table>");
			context.Response.Write("</form>");
		}
	}
}
