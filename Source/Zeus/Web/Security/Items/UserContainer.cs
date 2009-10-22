using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Integrity;

namespace Zeus.Web.Security.Items
{
	[ContentType("User Container")]
	[RestrictParents(typeof(SecurityContainer))]
	public class UserContainer : ContentItem
	{
		public const string ContainerName = "users";
		public const string ContainerTitle = "Users";

		public UserContainer()
		{
			Name = ContainerName;
			Title = ContainerTitle;
		}

		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(UserContainer), "Zeus.Web.Resources.Icons.group.png"); }
		}
	}
}
