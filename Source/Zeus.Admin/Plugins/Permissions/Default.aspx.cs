using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Security;
using Isis.Web.UI;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Permissions
{
	[AvailableOperation(Operations.Administer, "Administer", 50)]
	public partial class Default : PreviewFrameAdminPage
	{
		#region Fields

		private readonly IList<string> _displayedRoles = new List<string>();
		private readonly IList<string> _displayedUsers = new List<string>();

		#endregion

		#region Properties

		private IList<string> AddedRoles
		{
			get
			{
				IList<string> addedRoles = ViewState["AddedRoles"] as IList<string>;
				if (addedRoles == null)
					ViewState["AddedRoles"] = addedRoles = new List<string>();
				return addedRoles;
			}
		}

		private IList<string> AddedUsers
		{
			get
			{
				IList<string> addedUsers = ViewState["AddedUsers"] as IList<string>;
				if (addedUsers == null)
					ViewState["AddedUsers"] = addedUsers = new List<string>();
				return addedUsers;
			}
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			Title = "Change Permissions for '" + SelectedItem.Title + "'";
			CreatePermissionsTable();

			if (!IsPostBack)
			{
				BindAvailableRoles();
				BindAvailableUsers();
			}

			hlCancel.NavigateUrl = CancelUrl();

			base.OnLoad(e);
		}

		private IEnumerable<string> GetAllowedOperations()
		{
			// Get all available operations.
			List<string> allowedOperations = new List<string>();
			foreach (string operation in Engine.SecurityManager.GetAvailableOperations())
				if (Engine.SecurityManager.IsAuthorized(SelectedItem, User, operation))
					allowedOperations.Add(operation);
			return allowedOperations;
		}

		private void CreatePermissionsTable()
		{
			IEnumerable<string> allowedOperations = GetAllowedOperations();
			CreateHeaderRow(allowedOperations);
			CreateRows(allowedOperations);
		}

		private void CreateHeaderRow(IEnumerable<string> allowedOperations)
		{
			// The columns are the available operations.
			TableHeaderRow headerRow = new TableHeaderRow();
			headerRow.Cells.Add(new TableHeaderCell());
			headerRow.Cells.Add(new TableHeaderCell());
			foreach (string operation in allowedOperations)
				headerRow.Cells.Add(new TableHeaderCell { Text = operation });
			headerRow.Cells.Add(new TableHeaderCell());
			tblPermissions.Rows.Add(headerRow);
		}

		private void CreateRows(IEnumerable<string> allowedOperations)
		{
			foreach (string role in Engine.SecurityManager.GetAuthorizedRoles(SelectedItem))
				CreateRow(role, AuthorizationType.Role, allowedOperations);
			foreach (string user in Engine.SecurityManager.GetAuthorizedUsers(SelectedItem))
				CreateRow(user, AuthorizationType.User, allowedOperations);
			foreach (string role in AddedRoles)
				CreateRow(role, AuthorizationType.Role, allowedOperations);
			foreach (string user in AddedUsers)
				CreateRow(user, AuthorizationType.User, allowedOperations);
		}

		private void CreateRow(string roleOrUser, AuthorizationType type, IEnumerable<string> allowedOperations)
		{
			int index = tblPermissions.Rows.Count;
			string imageResourceName = (type == AuthorizationType.Role) ? "Zeus.Admin.Resources.group.png" : "Zeus.Admin.Resources.user.png";
			GenericIdentity identity = new GenericIdentity((type == AuthorizationType.User) ? roleOrUser : string.Empty);
			string[] roles = (type == AuthorizationType.Role) ? new[] { roleOrUser } : new string[] { };
			IPrincipal user = new GenericPrincipal(identity, roles);
			TableRow row = new TableRow();
			TableCell imageCell = new TableCell();
			imageCell.Controls.Add(new LiteralControl("<img src=\"" + WebResourceUtility.GetUrl(typeof(Default), imageResourceName) + "\" />"));
			imageCell.Controls.Add(new HiddenField { ID = "hdn" + index + "Type", Value = type.ToString() });
			imageCell.Controls.Add(new HiddenField { ID = "hdn" + index + "RoleOrUser", Value = roleOrUser });
			row.Cells.Add(imageCell);
			row.Cells.Add(new TableCell { Text = roleOrUser });
			foreach (string operation in allowedOperations)
			{
				CheckBox checkBox = new CheckBox
				{
					ID = "chk" + index + operation,
					Checked = Engine.SecurityManager.IsAuthorized(SelectedItem, user, operation)
				};
				TableCell cell = new TableCell { CssClass = "operation" };
				cell.Controls.Add(checkBox);
				row.Cells.Add(cell);
			}

			TableCell deleteCell = new TableCell();
			LinkButton deleteButton = new LinkButton { ID = "btnDelete" + index, Text = "Delete" };
			deleteButton.Click += deleteButton_Click;
			deleteCell.Controls.Add(deleteButton);
			row.Cells.Add(deleteCell);

			tblPermissions.Rows.Add(row);

			switch (type)
			{
				case AuthorizationType.Role:
					_displayedRoles.Add(roleOrUser);
					break;
				case AuthorizationType.User:
					_displayedUsers.Add(roleOrUser);
					break;
			}
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			((LinkButton) sender).Parent.Parent.Visible = false;
		}

		private void ApplyRulesRecursive(ContentItem item, IEnumerable<string> allowedOperations)
		{
			// Only apply the rules if the current user has permission to administer this item.
			if (Engine.SecurityManager.IsAuthorized(item, User, Operations.Administer))
				ApplyRules(item, allowedOperations);

			// Apply recursively.
			foreach (ContentItem child in item.GetChildren())
				ApplyRulesRecursive(child, allowedOperations);
		}

		private void ApplyRules(ContentItem item, IEnumerable<string> allowedOperations)
		{
			// Clear existing rules.
			item.AuthorizationRules.Clear();

			// Create new rules.
			for (int i = 1; i < tblPermissions.Rows.Count; ++i)
			{
				TableRow row = tblPermissions.Rows[i];
				if (!row.Visible)
					continue;

				// Get user or role for this row.
				string roleOrUser = ((HiddenField) row.FindControl("hdn" + i + "RoleOrUser")).Value;
				AuthorizationType type = ((HiddenField) row.FindControl("hdn" + i + "Type")).Value.ToEnum<AuthorizationType>();

				// Loop through operations, creating an authorization rule for each one.
				foreach (string operation in allowedOperations)
				{
					CheckBox checkBox = (CheckBox) row.FindControl("chk" + i + operation);
					item.AuthorizationRules.Add(new Zeus.Security.AuthorizationRule(item, operation, roleOrUser, type, checkBox.Checked));
				}
			}

			// Save item, with associated authorization rules.
			Engine.Persister.Save(item);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			ApplyRules(SelectedItem, GetAllowedOperations());
			Refresh(SelectedItem, AdminFrame.Navigation, false);
		}

		protected void btnSaveRecursive_Click(object sender, EventArgs e)
		{
			ApplyRulesRecursive(SelectedItem, GetAllowedOperations());
			Refresh(SelectedItem, AdminFrame.Navigation, false);
		}

		protected void btnAddRole_Click(object sender, EventArgs e)
		{
			AddedRoles.Add(ddlRoles.SelectedValue);
			CreateRow(ddlRoles.SelectedValue, AuthorizationType.Role, GetAllowedOperations());
			BindAvailableRoles();
		}

		private void BindAvailableRoles()
		{
			ddlRoles.Items.Clear();
			ddlRoles.Items.Add(string.Empty);

			ddlRoles.DataSource = Engine.Resolve<ICredentialContextService>().GetCurrentService().GetAllRoles().Where(r => !_displayedRoles.Contains(r));
			ddlRoles.DataBind();
		}

		protected void btnAddUser_Click(object sender, EventArgs e)
		{
			AddedUsers.Add(ddlUsers.SelectedValue);
			CreateRow(ddlUsers.SelectedValue, AuthorizationType.User, GetAllowedOperations());
			BindAvailableUsers();
		}

		private void BindAvailableUsers()
		{
			ddlUsers.Items.Clear();
			ddlUsers.Items.Add(string.Empty);

			ddlUsers.DataSource = Engine.Resolve<ICredentialContextService>().GetCurrentService().GetAllUsers().Select(u => u.Username).Where(u => !_displayedUsers.Contains(u));
			ddlUsers.DataBind();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.permissions.css");
			base.OnPreRender(e);
		}

		#endregion
	}
}