using System;
using System.Collections.Generic;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.ContentTypes;
using Zeus.Integrity;
using Zeus.Security;
using ListItem=System.Web.UI.WebControls.ListItem;

namespace Zeus.Admin.Plugins.NewItem
{
	[ActionPluginGroup("NewEditDelete", 10)]
	[AvailableOperation(Operations.Create, "Create", 20)]
	public partial class Default : PreviewFrameAdminPage
	{
		private ContentType ParentItemDefinition = null;
		private string ZoneName = null;

		public ContentItem ActualItem
		{
			get
			{
				if (rblPosition.SelectedIndex == 1)
					return base.SelectedItem;
				return base.SelectedItem.Parent;
			}
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
			if (SelectedItem.Parent == null)
			{
				rblPosition.Enabled = false;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ParentItemDefinition = Engine.ContentTypes.GetContentType(ActualItem.GetType());
			if (!IsPostBack)
				LoadZones();
			ZoneName = rblZone.SelectedValue;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parentItem = SelectedItem;
			IContentTypeManager manager = Zeus.Context.Current.Resolve<IContentTypeManager>();
			ContentType contentType = manager.GetContentType(parentItem.GetType());
			lsvChildTypes.DataSource = manager.GetAllowedChildren(contentType, Request.QueryString["zoneName"], User);
			lsvChildTypes.DataBind();
		}

		protected CreationPosition GetCreationPosition()
		{
			if (rblPosition.SelectedIndex == 0)
				return CreationPosition.Before;
			else if (rblPosition.SelectedIndex == 2)
				return CreationPosition.After;
			else
				return CreationPosition.Below;
		}

		protected string GetEditUrl(ContentType contentType)
		{
			return Engine.AdminManager.GetEditNewPageUrl(SelectedItem, contentType, ZoneName, GetCreationPosition());
		}

		protected void rblPosition_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			ParentItemDefinition = Engine.ContentTypes.GetContentType(ActualItem.GetType());
			LoadZones();
			ZoneName = rblZone.SelectedValue;
		}

		protected void rblZone_OnSelectedIndexChanged(object sender, EventArgs args)
		{
			ZoneName = rblZone.SelectedValue;
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.new.css");
			LoadAllowedTypes();
			base.OnPreRender(e);
		}

		private void LoadAllowedTypes()
		{
			int allowedChildrenCount = ParentItemDefinition.AllowedChildren.Count;
			IList<ContentType> allowedChildren = Engine.ContentTypes.GetAllowedChildren(ParentItemDefinition, ZoneName, User);

			if (allowedChildrenCount == 0)
			{
				Title = string.Format("No item is allowed below an item of type '{0}'", ParentItemDefinition.Title);
			}
			else if (allowedChildrenCount == 1 && allowedChildren.Count == 1)
			{
				Response.Redirect(GetEditUrl(allowedChildren[0]));
			}
			else
			{
				Title = string.Format("Select type of item below '{0}'", ActualItem.Title);

				lsvChildTypes.DataSource = allowedChildren;
				lsvChildTypes.DataBind();
			}
		}

		private void LoadZones()
		{
			string selectedZone = rblZone.SelectedValue;
			ListItem initialItem = rblZone.Items[0];

			rblZone.Items.Clear();
			rblZone.Items.Insert(0, initialItem);
			foreach (AvailableZoneAttribute zone in ParentItemDefinition.AvailableZones)
			{
				string title = zone.Title;
				rblZone.Items.Add(new ListItem(title, zone.ZoneName));
			}

			string z = IsPostBack ? selectedZone : Request.QueryString["zoneName"];
			if (rblZone.Items.FindByValue(z) != null)
				rblZone.SelectedValue = z;
		}
	}
}
