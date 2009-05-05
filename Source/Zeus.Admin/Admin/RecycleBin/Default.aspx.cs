using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Isis.Web.Security;
using Isis.Web.UI;
using Zeus.Security;

[assembly: EmbeddedResourceFile("Zeus.Admin.RecycleBin.Default.aspx", "Zeus.Admin")]
namespace Zeus.Admin.RecycleBin
{
	public partial class Default : PreviewFrameAdminPage
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
			base.OnLoad(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		#endregion

		protected void btnEmpty_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		protected void grvRecycleBinItems_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
