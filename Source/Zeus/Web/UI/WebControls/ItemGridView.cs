using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.ContentTypes.Properties;
using System.Web.UI.HtmlControls;
using System.Web;

namespace Zeus.Web.UI.WebControls
{
	public class ItemGridView : CompositeControl
	{
		private GridView _innerGridView;

		public ContentItem CurrentItem
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
			base.CreateChildControls();

			if (this.CurrentItem.Children.Count > 0)
			{
				_innerGridView = new GridView();
				_innerGridView.CssClass = "tb";
				_innerGridView.HeaderStyle.CssClass = "titles";

				_innerGridView.AutoGenerateColumns = false;

				IContentTypeManager contentTypeManager = Zeus.Context.Current.ContentTypes;
				ContentType contentType = contentTypeManager.GetContentType(this.CurrentItem.Children[0].GetType());

				TemplateField deleteField = new TemplateField();
				deleteField.ItemStyle.Width = Unit.Pixel(20);
				deleteField.HeaderImageUrl = "/admin/assets/images/littleTick.gif";
				deleteField.HeaderStyle.CssClass = "check";
				deleteField.ItemStyle.CssClass = "check";
				deleteField.ItemTemplate = new DeleteButtonTemplate();
				_innerGridView.Columns.Add(deleteField);

				ImageField iconField = new ImageField();
				iconField.ItemStyle.Width = Unit.Pixel(20);
				iconField.DataImageUrlField = "IconUrl";
				_innerGridView.Columns.Add(iconField);

				foreach (IDisplayer displayer in contentType.Displayers)
				{
					TemplateField displayField = new TemplateField();
					displayField.ItemTemplate = new DisplayTemplate(displayer);
					displayField.HeaderText = displayer.Title;
					_innerGridView.Columns.Add(displayField);
				}

				TemplateField editField = new TemplateField();
				editField.ItemStyle.Width = Unit.Pixel(50);
				editField.HeaderStyle.CssClass = "edit";
				editField.ItemStyle.CssClass = "edit";
				editField.ItemTemplate = new EditButtonTemplate();
				_innerGridView.Columns.Add(editField);

				_innerGridView.DataSource = this.CurrentItem.Children;
				_innerGridView.DataBind();

				this.Controls.Add(_innerGridView);
			}
		}

		private class DeleteButtonTemplate : ITemplate
		{
			public void InstantiateIn(System.Web.UI.Control container)
			{
				container.Controls.Add(new CheckBox { ID = "chkDelete", CssClass = "orangeChk" });
			}
		}

		private class DisplayTemplate : ITemplate
		{
			private IDisplayer _displayer;

			public DisplayTemplate(IDisplayer displayer)
			{
				_displayer = displayer;
			}

			public void InstantiateIn(System.Web.UI.Control container)
			{
				PlaceHolder placeHolder = new PlaceHolder();
				placeHolder.DataBinding += new EventHandler(placeHolder_DataBinding);
				container.Controls.Add(placeHolder);
			}

			private void placeHolder_DataBinding(object sender, EventArgs e)
			{
				PlaceHolder placeHolder = (PlaceHolder) sender;
				GridViewRow row = (GridViewRow) placeHolder.NamingContainer;
				_displayer.AddTo(placeHolder, (ContentItem) row.DataItem, _displayer.Name);
			}
		}

		private class EditButtonTemplate : ITemplate
		{
			public int ID
			{
				get;
				set;
			}

			public string Title
			{
				get;
				set;
			}

			public void InstantiateIn(System.Web.UI.Control container)
			{
				HyperLink link = new HyperLink
				{
					ID = "btnShowPopup",
					CssClass = "thickbox",
					Text = "Edit"
				};
				link.DataBinding += new EventHandler(link_DataBinding);

				container.Controls.Add(link);
			}

			private void link_DataBinding(object sender, EventArgs e)
			{
				HyperLink link = (HyperLink) sender;
				GridViewRow row = (GridViewRow) link.NamingContainer;
				link.ToolTip = string.Format("Details for {0}", DataBinder.Eval(row.DataItem, "Title"));
				link.NavigateUrl = string.Format("ViewDetail.aspx?selected={0}&TB_iframe=true&height=400&width=700", HttpUtility.UrlEncode(DataBinder.Eval(row.DataItem, "Path").ToString()));
			}
		}
	}
}
