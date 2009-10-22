using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web;
using Zeus.BaseLibrary.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Engine;
using Zeus.Globalization.ContentTypes;
using Zeus.Linq;
using Zeus.Security;
using System.Web.UI.HtmlControls;
using System.Web;
using Zeus.FileSystem.Images;
using TreeNode=Zeus.Web.UI.WebControls.TreeNode;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class Tree : Control
	{
		private ContentItem _selectedItem = null, _rootItem = null;

		public ContentItem SelectedItem
		{
			get { return _selectedItem ?? (_selectedItem = Find.CurrentPage ?? Find.StartPage); }
			set { _selectedItem = value; }
		}

		public virtual ContentItem RootNode
		{
			get { return _rootItem ?? Find.RootItem; }
			set { _rootItem = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			string path = Page.Request.GetOptionalString("selected");
			_selectedItem = (!string.IsNullOrEmpty(path)) ? Zeus.Context.Current.Resolve<Navigator>().Navigate(path) : RootNode;
			_selectedItem = _selectedItem.TranslationOf ?? _selectedItem;

			//if (Page.User.Identity.Name != "administrator")
			//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);
			TreeNode treeNode = Zeus.Web.Tree.Between(_selectedItem, RootNode, true)
				.OpenTo(_selectedItem)
				.Filter(items => items.Authorized(Page.User, Zeus.Context.SecurityManager, Operations.Read))
				.LinkProvider(BuildLink)
				.ToControl();

			AppendExpanderNodeRecursive(treeNode);

			treeNode.LiClass = "root";

			Controls.Add(treeNode);
		}

		public static void AppendExpanderNodeRecursive(Control tree)
		{
			TreeNode tn = tree as TreeNode;
			if (tn != null)
			{
				foreach (Control child in tn.Controls)
					AppendExpanderNodeRecursive(child);
				if (tn.Controls.Count == 0 && tn.Node.GetChildren().Count() > 0)
					AppendExpanderNode(tn);
			}
		}

		public static void AppendExpanderNode(TreeNode tn)
		{
			HtmlGenericControl li = new HtmlGenericControl("li");
			li.InnerText = "{url:/Admin/Navigation/TreeLoader.ashx?selected=" + HttpUtility.UrlEncode(tn.Node.Path) + "}";

			tn.UlClass = "ajax";
			tn.Controls.Add(li);
		}

		private Control BuildLink(ContentItem node)
		{
			return BuildLink(node, _selectedItem);
		}

		internal static Control BuildLink(ContentItem node, ContentItem selectedItem)
		{
			ContentItem translatedItem;
			TranslationStatus translationStatus = GetTranslationStatus(node, out translatedItem);

			HtmlAnchor anchor = new HtmlAnchor();
			anchor.HRef = ((INode) translatedItem).PreviewUrl;
			anchor.Target = "preview";
			anchor.Attributes["data-url"] = Url.ToAbsolute(((INode) translatedItem).PreviewUrl);
			string cssClass = ((INode) translatedItem).ClassNames;
			if (translationStatus == TranslationStatus.NotAvailableInSelectedLanguage)
				cssClass += " notAvailableInSelectedLanguage";
			anchor.Attributes["class"] = cssClass;

			if (Zeus.Context.AdminManager.TreeTooltipsEnabled)
			{
				string tooltip = string.Format("{0}`{1}`{2}`{3}", Zeus.Context.ContentTypes.GetContentType(node.GetType()).Title, node.Created, node.Updated, node.ID);
				anchor.Attributes["data-title"] = tooltip;
			}

			HtmlImage image = new HtmlImage();
			image.Src = translatedItem.IconUrl;
			anchor.Controls.Add(image);
			anchor.Controls.Add(new LiteralControl(((INode) translatedItem).Contents));

			HtmlGenericControl span = new HtmlGenericControl("span");
			span.ID = "span" + translatedItem.ID;
			span.Attributes["data-id"] = translatedItem.ID.ToString();
	
			span.Attributes["data-path"] = node.Path;
			span.Attributes["data-type"] = node.GetType().Name;
			if (IsSelectedItem(translatedItem, selectedItem))
				span.Attributes["class"] = "active";
			span.Controls.Add(anchor);

			if (translationStatus == TranslationStatus.NotAvailableInSelectedLanguage || translationStatus == TranslationStatus.DisplayedInAnotherLanguage)
			{
				span.Controls.Add(new LiteralControl("&nbsp;"));
				System.Web.UI.WebControls.Image translationImage = new System.Web.UI.WebControls.Image();
				switch (translationStatus)
				{
					case TranslationStatus.NotAvailableInSelectedLanguage:
						translationImage.ImageUrl = WebResourceUtility.GetUrl(typeof(Tree), "Zeus.Admin.Assets.Images.Icons.LanguageMissing.gif");
						translationImage.ToolTip = "Page is missing for the current language and will not be displayed.";
						break;
					case TranslationStatus.DisplayedInAnotherLanguage :
						Language language = Zeus.Context.Current.LanguageManager.GetLanguage(translatedItem.Language);
						translationImage.ImageUrl = language.FlagIcon.Url;
						translationImage.ToolTip = "Page is displayed in another language (" + language.Title + ").";
						break;
				}
				span.Controls.Add(translationImage);
			}

			return span;
		}

		private static bool IsSelectedItem(ContentItem node, ContentItem selectedItem)
		{
			return node == selectedItem;
		}

		private static TranslationStatus GetTranslationStatus(ContentItem contentItem, out ContentItem translatedItem)
		{
			translatedItem = contentItem;

			if (contentItem == null)
				return TranslationStatus.Available;

			if (!Zeus.Context.Current.LanguageManager.CanBeTranslated(contentItem))
				return TranslationStatus.Available;

			if (string.IsNullOrEmpty(contentItem.Language))
				return TranslationStatus.Available;

			string languageCode = Zeus.Context.AdminManager.CurrentAdminLanguageBranch;
			ContentItem testTranslatedItem = Zeus.Context.Current.LanguageManager.GetTranslation(contentItem, languageCode);
			if (testTranslatedItem != null)
				translatedItem = testTranslatedItem;

			if (Zeus.Context.Current.LanguageManager.TranslationExists(contentItem, languageCode))
				return TranslationStatus.Available;

			if (testTranslatedItem == null)
				return TranslationStatus.NotAvailableInSelectedLanguage;

			if (translatedItem.Language != languageCode)
				return TranslationStatus.DisplayedInAnotherLanguage;

			return TranslationStatus.Available;
		}

		private enum TranslationStatus
		{
			Available,

			/// <summary>
			/// Page is missing for the current language and will not be displayed.
			/// </summary>
			NotAvailableInSelectedLanguage,

			/// <summary>
			/// Page is displayed in another language (English).
			/// </summary>
			DisplayedInAnotherLanguage
		}
	}
}
