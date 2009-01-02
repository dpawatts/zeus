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
using Isis.Web.UI.WebControls;
using Isis.Reflection;
using System.Reflection;
using Isis.Web.UI.HtmlControls;
using Zeus.Admin;
using System.IO;
using System.Text.RegularExpressions;

namespace Zeus.Web.UI.WebControls
{
	public class ItemGridView : ListView
	{
		private ContentType _contentType;
		private int? _sortCellIndex;

		public ItemGridView()
		{
			this.DataKeyNames = new string[] { "ID" };
		}

		public ContentItem CurrentItem
		{
			get;
			set;
		}

		private ContentType ContentType
		{
			get
			{
				if (_contentType == null)
				{
					IContentTypeManager contentTypeManager = Zeus.Context.Current.ContentTypes;
					if (this.CurrentItem.Children.Any())
						_contentType = contentTypeManager.GetContentType(this.CurrentItem.Children[0].GetType());
				}
				return _contentType;
			}
		}

		protected override void CreateLayoutTemplate()
		{
			this.LayoutTemplate = new LayoutTemplateControl(this, this.Page, this.ContentType);
			this.ItemTemplate = new ItemTemplateControl(this, this.ContentType);

			base.CreateLayoutTemplate();
		}

		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				string commandName = ((CommandEventArgs) e).CommandName;
				if (commandName != null)
				{
					if (commandName == "Delete")
					{
						bool flag = false;
						foreach (ListViewDataItem item in this.Items)
						{
							CheckBox box = item.FindControl("chkDelete") as CheckBox;
							if (box != null)
							{
								flag = true;
								if (box.Checked)
									this.DeleteItem(item.DisplayIndex);
							}
						}
						if (flag)
							return true;
					}
				}
			}
			return base.OnBubbleEvent(source, e);
		}

		protected override void OnItemDataBound(ListViewItemEventArgs e)
		{
			base.OnItemDataBound(e);

			//  if there is no sort expression, don't bother
			if (this.SortExpression.Length > 0)
			{
				//  if this is the first time ItemDataBound has fired, figure out what column
				//  is being sorted by
				if (this._sortCellIndex == null)
				{
					HtmlTableRow header = ((HtmlTable) this.FindControl("dataTable")).Rows[0];

					//  loop through the cells and find the one that contains the linkbutton
					//  with the commandargument of the ListView's current SortExpression
					for (int i = 0; i < header.Cells.Count; i++)
					{
						HtmlTableCell th = header.Cells[i];
						//  find the LinkButton control
						foreach (Control c in th.Controls)
						{
							LinkButton linkButton = c as LinkButton;
							if (linkButton != null && linkButton.CommandArgument == this.SortExpression)
							{
								//  keep track of the cell index
								this._sortCellIndex = i;

								//  add the sort class to this item                        
								string originalHeaderStyle = th.Attributes["class"].Replace("asc", string.Empty).Replace("desc", string.Empty);
								th.Attributes["class"] = string.Format("{0} {1}", originalHeaderStyle, this.SortDirection == SortDirection.Ascending ? "asc" : "desc").Trim();
								break;
							}
						}
					}
				}

				//  set the cells css class as well
				HtmlTableRow tr = (HtmlTableRow) e.Item.FindControl("row");
				HtmlTableCell td = tr.Cells[this._sortCellIndex.Value];
				string originalCellStyle = td.Attributes["class"];
				td.Attributes["class"] = string.Format("{0} {1}", originalCellStyle, "sort").Trim();
			}
		}

		#region LayoutTemplateControl class

		private class LayoutTemplateControl : ITemplate
		{
			private ItemGridView _parent;

			public LayoutTemplateControl(ItemGridView parent, Page currentPage, ContentType contentType)
			{
				_parent = parent;
				this.CurrentPage = currentPage;
				this.ContentType = contentType;
			}

			public Page CurrentPage
			{
				get;
				set;
			}

			public ContentType ContentType
			{
				get;
				set;
			}

			public void InstantiateIn(Control container)
			{
				string layoutTemplateString = Assembly.GetExecutingAssembly().GetStringResource(typeof(ItemGridView).FullName + ".LayoutTemplate.ascx");
				Control layoutTemplate = this.CurrentPage.ParseControl(layoutTemplateString);
				container.Controls.Add(layoutTemplate);

				// Add column headers to layout template.
				if (this.ContentType != null)
				{
					Control columnsPlaceholder = layoutTemplate.Controls[5].Controls[0];
					int index = 1;
					foreach (IDisplayer displayer in this.ContentType.Displayers)
					{
						HtmlTableCell sortableColumnHeader = new HtmlTableCell("th");
						sortableColumnHeader.Attributes["class"] = "data";
						columnsPlaceholder.Controls.AddAt(index++, sortableColumnHeader);

						LinkButton linkButton = new LinkButton { ID = "lnkSort" + displayer.Name, Text = displayer.Title, CommandName = "Sort", CommandArgument = displayer.Name };
						sortableColumnHeader.Controls.Add(linkButton);
					}

					foreach (GridViewPluginAttribute plugin in _parent.GetPlugins())
					{
						HtmlTableCell tdPlugin = new HtmlTableCell("th");
						columnsPlaceholder.Controls.AddAt(index++, tdPlugin);
					}
				}
			}
		}

		#endregion

		#region ItemTemplateControl class

		private class ItemTemplateControl : ITemplate
		{
			private ItemGridView _parent;

			public ItemTemplateControl(ItemGridView parent, ContentType contentType)
			{
				_parent = parent;
				this.ContentType = contentType;
			}

			public ContentType ContentType
			{
				get;
				set;
			}

			public void InstantiateIn(Control container)
			{
				HtmlTableRow tr = new HtmlTableRow { ID = "row" };
				container.Controls.Add(tr);

				HtmlTableCell tdDelete = new HtmlTableCell();
				tdDelete.Attributes["class"] = "check";
				tr.Controls.Add(tdDelete);

				CheckBox chkDelete = new CheckBox { ID = "chkDelete" };
				tdDelete.Controls.Add(chkDelete);

				foreach (IDisplayer displayer in this.ContentType.Displayers)
				{
					HtmlTableCell td = new HtmlTableCell();
					tr.Controls.Add(td);

					PlaceHolder placeHolder = new PlaceHolder { ID = displayer.Name };
					displayer.InstantiateIn(placeHolder);
					placeHolder.DataBinding += new EventHandler(placeHolder_DataBinding);
					td.Controls.Add(placeHolder);
				}

				foreach (GridViewPluginAttribute plugin in _parent.GetPlugins())
				{
					HtmlTableCell tdPlugin = new HtmlTableCell();
					tdPlugin.ID = plugin.Name;
					plugin.AddTo(tdPlugin);
					tdPlugin.DataBinding += new EventHandler(tdPlugin_DataBinding);
					tr.Controls.Add(tdPlugin);
				}

				HtmlTableCell tdEdit = new HtmlTableCell();
				tdEdit.Attributes["class"] = "edit";
				tr.Controls.Add(tdEdit);

				HyperLink editLink = new HyperLink
				{
					ID = "btnShowPopup",
					CssClass = "thickbox",
					Text = "Edit"
				};
				editLink.DataBinding += new EventHandler(editLink_DataBinding);
				tdEdit.Controls.Add(editLink);
			}

			private void tdPlugin_DataBinding(object sender, EventArgs e)
			{
				HtmlTableCell cell = (HtmlTableCell) sender;
				ListViewDataItem listViewDataItem = (ListViewDataItem) cell.NamingContainer;
				foreach (GridViewPluginAttribute plugin in _parent.GetPlugins().Where(p => p.Name == cell.ID))
					plugin.SetValue(cell, (ContentItem) listViewDataItem.DataItem);
			}

			private void placeHolder_DataBinding(object sender, EventArgs e)
			{
				PlaceHolder placeHolder = (PlaceHolder) sender;
				ListViewDataItem listViewDataItem = (ListViewDataItem) placeHolder.NamingContainer;
				IDisplayer displayer = this.ContentType.Displayers.Single(d => d.Name == placeHolder.ID);
				displayer.SetValue(placeHolder, (ContentItem) listViewDataItem.DataItem, displayer.Name);
			}

			private void editLink_DataBinding(object sender, EventArgs e)
			{
				HyperLink link = (HyperLink) sender;
				ListViewDataItem listViewDataItem = (ListViewDataItem) link.NamingContainer;
				link.ToolTip = string.Format("Details for {0}", DataBinder.Eval(listViewDataItem.DataItem, "Title"));
				link.NavigateUrl = string.Format("ViewDetail.aspx?selected={0}&TB_iframe=true&height=400&width=700",
					HttpUtility.UrlEncode(DataBinder.Eval(listViewDataItem.DataItem, "Path").ToString()));
			}
		}

		#endregion

		private IEnumerable<GridViewPluginAttribute> GetPlugins()
		{
			ContentType contentType = Zeus.Context.ContentTypes.GetContentType(this.CurrentItem.GetType());
			Type childType = Zeus.Context.ContentTypes.GetAllowedChildren(contentType, this.Page.User)[0].ItemType;
			List<GridViewPluginAttribute> plugins = new List<GridViewPluginAttribute>();
			foreach (Assembly assembly in GetAssemblies())
				foreach (GridViewPluginAttribute plugin in FindPluginsIn(assembly))
					if (plugin.Type.IsAssignableFrom(childType))
						plugins.Add(plugin);
			return plugins;
		}

		private IEnumerable<GridViewPluginAttribute> FindPluginsIn(Assembly a)
		{
			foreach (GridViewPluginAttribute attribute in a.GetCustomAttributes(typeof(GridViewPluginAttribute), false))
				yield return attribute;
			foreach (Type t in a.GetTypes())
			{
				foreach (GridViewPluginAttribute attribute in t.GetCustomAttributes(typeof(GridViewPluginAttribute), false))
					yield return attribute;
			}
		}

		// Copied from ContentTypeBuilder
		private string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^SoundInTheory\\.NMigration|^SoundInTheory\\.DynamicImage";

		private IList<Assembly> GetAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
					assemblies.Add(assembly);
			}

			foreach (string dllPath in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/bin"), "*.dll"))
			{
				try
				{
					Assembly assembly = Assembly.ReflectionOnlyLoadFrom(dllPath);
					if (Matches(assembly.FullName) && !assemblies.Any(a => a.FullName == assembly.FullName))
					{
						Assembly loadedAssembly = AppDomain.CurrentDomain.Load(assembly.FullName);
						assemblies.Add(loadedAssembly);
					}
				}
				catch (BadImageFormatException)
				{
					//Trace.TraceError(ex.ToString());
				}
			}

			return assemblies;
		}

		private bool Matches(string assemblyFullName)
		{
			return !Matches(assemblyFullName, _assemblySkipLoadingPattern);
		}

		private bool Matches(string assemblyFullName, string pattern)
		{
			return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
	}
}
