using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using System;

namespace Zeus.Web.UI.WebControls
{
	public class LinkedItemList : WebControl
	{
		private UpdatePanel _container;

		private Dictionary<Guid, string> AddedControls
		{
			get
			{
				Dictionary<Guid, string> addedControls = ViewState["AddedControls"] as Dictionary<Guid, string>;
				if (addedControls == null)
					ViewState["AddedControls"] = addedControls = new Dictionary<Guid, string>();
				return addedControls;
			}
		}

		public LinkedItemList()
		{
			CssClass = "linkedItemList";
		}

		public void SetSelectedItems(string[] values)
		{
			if (!Page.IsPostBack) // TODO: Remove this
			{
				foreach (string value in values)
					AddedControls.Add(Guid.NewGuid(), value);
				ChildControlsCreated = false;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnsureChildControls();
		}

		public ContentItem[] SelectedContentItems
		{
			get
			{
				return null;
				//return _selectedItems.ToArray();
			}
		}

		public ListItem[] AvailableItems
		{
			get;
			set;
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		protected override void CreateChildControls()
		{
			_container = new UpdatePanel();
			_container.ID = "updatePanel";
			Controls.Add(_container);

			foreach (KeyValuePair<Guid, string> addedControl in AddedControls)
				AddDdl(addedControl);

			ImageButton b = new ImageButton();
			b.ID = "addNew";
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(LinkedItemList), "Zeus.Web.UI.WebControls.Images.add.png");
			b.ToolTip = "Add item";
			b.CausesValidation = false;
			b.Click += AddItemClick;
			b.CssClass = "add";
			_container.ContentTemplateContainer.Controls.Add(b);

			_container.ContentTemplateContainer.Controls.Add(new LiteralControl("<br />"));
		}

		private void AddDdl(KeyValuePair<Guid, string> addedControl)
		{
			CreateDdl(_container, addedControl);
			AddDeleteButton(_container, addedControl);
			_container.ContentTemplateContainer.Controls.Add(new LiteralControl("<br />"));
		}

		private void AddDeleteButton(UpdatePanel updatePanel, KeyValuePair<Guid, string> addedControl)
		{
			ImageButton b = new ImageButton();
			updatePanel.ContentTemplateContainer.Controls.Add(b);
			b.ID = ID + "_d_" + addedControl.Key;
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(LinkedItemList), "Zeus.Web.UI.WebControls.Images.delete.png");
			b.ToolTip = "Delete item";
			b.CommandArgument = addedControl.Key.ToString();
			b.Click += DeleteItemClick;
		}

		private void CreateDdl(UpdatePanel updatePanel, KeyValuePair<Guid, string> addedControl)
		{
			DropDownList ddl = new DropDownList();
			ddl.ID = "_ddl_" + addedControl.Key;
			if (!string.IsNullOrEmpty(addedControl.Value) && !Page.IsPostBack)
				ddl.SelectedValue = addedControl.Value;
			updatePanel.ContentTemplateContainer.Controls.Add(ddl);
			ddl.Items.AddRange(this.AvailableItems);
		}

		private void AddItemClick(object sender, ImageClickEventArgs e)
		{
			KeyValuePair<Guid, string> newItem = new KeyValuePair<Guid,string>(Guid.NewGuid(), null);
			AddedControls.Add(newItem.Key, newItem.Value);
			AddDdl(newItem);
		}

		private void DeleteItemClick(object sender, ImageClickEventArgs e)
		{
			ImageButton b = (ImageButton) sender;
			Guid index = new Guid(b.CommandArgument);
			AddedControls.Remove(index);
			ChildControlsCreated = false;
		}
	}
}
