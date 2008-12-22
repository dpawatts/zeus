using System;
using System.Linq;
using Isis.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.ContentTypes.Properties;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace Zeus.Web.UI.WebControls
{
	public class ItemGridView : CompositeControl
	{
		private GridView _innerGridView;
		private Literal _totalRecords;

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
				_totalRecords = new Literal();
				this.Controls.Add(_totalRecords);

				_innerGridView = new GridView();
				_innerGridView.ID = "innerGridView";
				_innerGridView.CssClass = "tb";
				_innerGridView.HeaderStyle.CssClass = "titles";

				_innerGridView.AllowPaging = true;
				_innerGridView.PageSize = 25;
				_innerGridView.PagerSettings.Mode = PagerButtons.Numeric;
				_innerGridView.PageIndexChanging += new GridViewPageEventHandler(_innerGridView_PageIndexChanging);

				_innerGridView.AllowSorting = true;
				_innerGridView.Sorting += new GridViewSortEventHandler(_innerGridView_Sorting);

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
					displayField.SortExpression = displayer.Name;
					_innerGridView.Columns.Add(displayField);
				}

				TemplateField editField = new TemplateField();
				editField.ItemStyle.Width = Unit.Pixel(50);
				editField.HeaderStyle.CssClass = "edit";
				editField.ItemStyle.CssClass = "edit";
				editField.ItemTemplate = new EditButtonTemplate();
				_innerGridView.Columns.Add(editField);

				this.Controls.Add(_innerGridView);

				_innerGridView.DataSource = this.CurrentItem.Children;
				ReBind();
			}
		}

		private void _innerGridView_Sorting(object sender, GridViewSortEventArgs e)
		{
			IList<ContentItem> dataSource = _innerGridView.DataSource as IList<ContentItem>;
			Array typedDataSource = Array.CreateInstance(dataSource.First().GetType(), dataSource.Count);
			Array.Copy(dataSource.ToArray(), typedDataSource, dataSource.Count);

			if (dataSource != null)
			{
				_innerGridView.DataSource = typedDataSource.AsQueryable().OrderBy(e.SortExpression + " " + e.SortDirection).Cast<ContentItem>().ToList();
				ReBind();
			}
		}

		private void _innerGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			_innerGridView.PageIndex = e.NewPageIndex;
			ReBind();
		}

		private void ReBind()
		{
			_innerGridView.DataBind();
			_totalRecords.Text = string.Format("<p>There are {0} total records.</p>", _innerGridView.Rows.Count);
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
