using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Globalization;

namespace Zeus.Web
{
	[UI.Panel("Titles", "Page Identification", 10, Collapsible = true)]
	[UI.Panel("Content", "Content", 20, Collapsible = true)]
	[DefaultContainer("Content")]
	[Translatable]
    [System.Serializable]
	public abstract class PageContentItem : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true, Shared = false, ContainerName = "Titles")]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ContentProperty("Page Title", 11, Description = "Used in the &lt;h1&gt; element on the page", EditorContainerName = "Titles")]
		[PageTitleEditor]
		public virtual string PageTitle
		{
			get { return GetDetail("PageTitle", Title); }
			set { SetDetail("PageTitle", value); }
		}

		[NameEditor("URL", 20, Required = true, Shared = false, ContainerName = "Titles")]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Visible in Menu", 25)]
		public override bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Page); }
		}

		public override bool IsPage
		{
			get { return true; }
		}

        public virtual string ProgrammableHtmlTitle { get; set; }
        public virtual string ProgrammableMetaDescription { get; set; }
        public virtual string ProgrammableMetaKeywords { get; set; }

        public virtual bool UseProgrammableSEOAssets { get { return false; } }

        /// <summary>If set to true the code will allow /any_string_here and pass the "any_string_here" part in as a string called "Param" into the action matched by the Url without the "any_string_here" part - allows basic MVC functionality to work</summary>
        public virtual bool AllowParamsOnIndex
        {
            get { return false; }
        }
	}
}