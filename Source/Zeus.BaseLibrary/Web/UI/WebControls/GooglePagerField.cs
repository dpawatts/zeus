using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Globalization;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for GoogleDataPager
	/// </summary>
	public class GooglePagerField : DataPagerField
	{
		private int _startRowIndex;
		private int _maximumRows;
		private int _totalRowCount;

		//Next and previous buttons by default are always enabled.
		private bool _showPreviousPage = true;
		private bool _showNextPage = true;

		public GooglePagerField()
		{
		}

		#region Properties

		public string NextPageText
		{
			get { return ViewState["NextPageText"] as string ?? "Next"; }
			set
			{
				if (value != this.NextPageText)
				{
					base.ViewState["NextPageText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		public string PreviousPageText
		{
			get { return ViewState["PreviousPageText"] as string ?? "Previous"; }
			set
			{
				if (value != this.PreviousPageText)
				{
					base.ViewState["PreviousPageText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		public string NextPageImageUrl
		{
			get { return ViewState["NextPageImageUrl"] as string ?? string.Empty; }
			set
			{
				if (value != this.NextPageImageUrl)
				{
					base.ViewState["NextPageImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		public string PreviousPageImageUrl
		{
			get { return ViewState["PreviousPageImageUrl"] as string ?? string.Empty; }
			set
			{
				if (value != this.PreviousPageImageUrl)
				{
					base.ViewState["PreviousPageImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		public bool RenderNonBreakingSpacesBetweenControls
		{
			get { return (bool) (ViewState["RenderNonBreakingSpacesBetweenControls"] ?? true); }
			set
			{
				if (value != this.RenderNonBreakingSpacesBetweenControls)
				{
					base.ViewState["RenderNonBreakingSpacesBetweenControls"] = value;
					this.OnFieldChanged();
				}
			}
		}

		[CssClassProperty]
		public string ButtonCssClass
		{
			get { return ViewState["ButtonCssClass"] as string ?? string.Empty; }
			set
			{
				if (value != this.ButtonCssClass)
				{
					base.ViewState["ButtonCssClass"] = value;
					this.OnFieldChanged();
				}
			}
		}

		private bool EnablePreviousPage
		{
			get { return (this._startRowIndex > 0); }
		}

		private bool EnableNextPage
		{
			get { return ((this._startRowIndex + this._maximumRows) < this._totalRowCount); }
		}

		#endregion

		public override void CreateDataPagers(DataPagerFieldItem container, int startRowIndex, int maximumRows, int totalRowCount, int fieldIndex)
		{
			this._startRowIndex = startRowIndex;
			this._maximumRows = maximumRows;
			this._totalRowCount = totalRowCount;

			if (string.IsNullOrEmpty(base.DataPager.QueryStringField))
				this.CreateDataPagersForCommand(container, fieldIndex);
			else
				this.CreateDataPagersForQueryString(container, fieldIndex);
		}

		protected override DataPagerField CreateField()
		{
			return new GooglePagerField();
		}

		public override void HandleEvent(CommandEventArgs e)
		{
			if (string.Equals(e.CommandName, "UpdatePageSize"))
			{
				base.DataPager.PageSize = Convert.ToInt32(e.CommandArgument.ToString());
				base.DataPager.SetPageProperties(this._startRowIndex, base.DataPager.PageSize, true);
				return;
			}

			if (string.Equals(e.CommandName, "GoToItem"))
			{
				int newStartRowIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
				base.DataPager.SetPageProperties(newStartRowIndex, base.DataPager.PageSize, true);
				return;
			}

			if (string.IsNullOrEmpty(base.DataPager.QueryStringField))
			{
				if (string.Equals(e.CommandName, "Prev"))
				{
					int startRowIndex = this._startRowIndex - base.DataPager.PageSize;
					if (startRowIndex < 0)
						startRowIndex = 0;
					base.DataPager.SetPageProperties(startRowIndex, base.DataPager.PageSize, true);
				}
				else if (string.Equals(e.CommandName, "Next"))
				{
					int nextStartRowIndex = this._startRowIndex + base.DataPager.PageSize;

					if (nextStartRowIndex > this._totalRowCount)
						nextStartRowIndex = this._totalRowCount - base.DataPager.PageSize;

					if (nextStartRowIndex < 0)
						nextStartRowIndex = 0;

					base.DataPager.SetPageProperties(nextStartRowIndex, base.DataPager.PageSize, true);
				}
			}
		}

		private void CreateDataPagersForCommand(DataPagerFieldItem container, int fieldIndex)
		{
			//Goto item texbox
			this.CreateGoToTexBox(container);

			//Control used to set the page size.
			this.CreatePageSizeControl(container);

			//Set of records - total records
			this.CreateLabelRecordControl(container);

			//Previous button
			if (this._showPreviousPage)
			{
				container.Controls.Add(this.CreateControl("Prev", this.PreviousPageText, fieldIndex, this.PreviousPageImageUrl, this._showPreviousPage));
				this.AddNonBreakingSpace(container);
			}

			//Next button
			if (this._showNextPage)
			{
				container.Controls.Add(this.CreateControl("Next", this.NextPageText, fieldIndex, this.NextPageImageUrl, this._showNextPage));
				this.AddNonBreakingSpace(container);
			}
		}

		private Control CreateControl(string commandName, string buttonText, int fieldIndex, string imageUrl, bool enabled)
		{
			IButtonControl control;

			control = new BorderlessImageButton();
			((ImageButton) control).ImageUrl = imageUrl;
			((ImageButton) control).Enabled = enabled;
			((ImageButton) control).AlternateText = HttpUtility.HtmlDecode(buttonText);

			control.Text = buttonText;
			control.CommandName = commandName;
			control.CommandArgument = fieldIndex.ToString(CultureInfo.InvariantCulture);
			WebControl control2 = control as WebControl;
			if ((control2 != null) && !string.IsNullOrEmpty(this.ButtonCssClass))
				control2.CssClass = this.ButtonCssClass;

			return (control as Control);
		}

		private void AddNonBreakingSpace(DataPagerFieldItem container)
		{
			if (this.RenderNonBreakingSpacesBetweenControls)
				container.Controls.Add(new LiteralControl("&nbsp;"));
		}

		private void CreateLabelRecordControl(DataPagerFieldItem container)
		{
			int endRowIndex = this._startRowIndex + base.DataPager.PageSize;

			if (endRowIndex > this._totalRowCount)
				endRowIndex = this._totalRowCount;

			container.Controls.Add(new LiteralControl(string.Format("{0} - {1} of {2}", this._startRowIndex + 1, endRowIndex, this._totalRowCount)));

			this.AddNonBreakingSpace(container);
			this.AddNonBreakingSpace(container);
			this.AddNonBreakingSpace(container);
		}

		private void CreatePageSizeControl(DataPagerFieldItem container)
		{
			container.Controls.Add(new LiteralControl("Show rows: "));

			ButtonDropDownList pageSizeDropDownList = new ButtonDropDownList();

			pageSizeDropDownList.CommandName = "UpdatePageSize";

			pageSizeDropDownList.Items.Add(new ListItem("10", "10"));
			pageSizeDropDownList.Items.Add(new ListItem("25", "25"));
			pageSizeDropDownList.Items.Add(new ListItem("50", "50"));
			pageSizeDropDownList.Items.Add(new ListItem("100", "100"));
			pageSizeDropDownList.Items.Add(new ListItem("250", "250"));
			pageSizeDropDownList.Items.Add(new ListItem("500", "500"));

			ListItem pageSizeItem = pageSizeDropDownList.Items.FindByValue(base.DataPager.PageSize.ToString());

			if (pageSizeItem == null)
			{
				pageSizeItem = new ListItem(base.DataPager.PageSize.ToString(), base.DataPager.PageSize.ToString());
				pageSizeDropDownList.Items.Insert(0, pageSizeItem);
			}

			pageSizeItem.Selected = true;
			container.Controls.Add(pageSizeDropDownList);

			this.AddNonBreakingSpace(container);
			this.AddNonBreakingSpace(container);
		}

		private void CreateGoToTexBox(DataPagerFieldItem container)
		{
			Label label = new Label();
			label.Text = "Go to: ";
			container.Controls.Add(label);

			ButtonTextBox goToTextBox = new ButtonTextBox();

			goToTextBox.CommandName = "GoToItem";
			goToTextBox.Width = new Unit("20px");
			container.Controls.Add(goToTextBox);

			this.AddNonBreakingSpace(container);
			this.AddNonBreakingSpace(container);
		}

		private void CreateDataPagersForQueryString(DataPagerFieldItem container, int fieldIndex)
		{
			bool validPageIndex = false;
			if (!base.QueryStringHandled)
			{
				int num;
				base.QueryStringHandled = true;
				if (int.TryParse(base.QueryStringValue, out num))
				{
					num--;
					int currentPageIndex = this._startRowIndex / this._maximumRows;
					int maxPageIndex = (this._totalRowCount - 1) / this._maximumRows;
					if ((num >= 0) && (num <= maxPageIndex))
					{
						this._startRowIndex = num * this._maximumRows;
						validPageIndex = true;
					}
				}
			}

			//Goto item texbox
			this.CreateGoToTexBox(container);

			//Control used to set the page size.
			this.CreatePageSizeControl(container);

			//Set of records - total records
			this.CreateLabelRecordControl(container);

			if (this._showPreviousPage)
			{
				int pageIndex = (this._startRowIndex / this._maximumRows) - 1;
				container.Controls.Add(this.CreateLink(this.PreviousPageText, pageIndex, this.PreviousPageImageUrl, this.EnablePreviousPage));
				this.AddNonBreakingSpace(container);
			}
			if (this._showNextPage)
			{
				int num4 = (this._startRowIndex + this._maximumRows) / this._maximumRows;
				container.Controls.Add(this.CreateLink(this.NextPageText, num4, this.NextPageImageUrl, this.EnableNextPage));
				this.AddNonBreakingSpace(container);
			}
			if (validPageIndex)
			{
				base.DataPager.SetPageProperties(this._startRowIndex, this._maximumRows, true);
			}
		}

		private HyperLink CreateLink(string buttonText, int pageIndex, string imageUrl, bool enabled)
		{
			int pageNumber = pageIndex + 1;
			HyperLink link = new HyperLink();
			link.Text = buttonText;
			link.NavigateUrl = base.GetQueryStringNavigateUrl(pageNumber);
			link.ImageUrl = imageUrl;
			link.Enabled = enabled;
			if (!string.IsNullOrEmpty(this.ButtonCssClass))
			{
				link.CssClass = this.ButtonCssClass;
			}
			return link;
		}
	}
}
