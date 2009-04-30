using System.Globalization;
using Isis.Web.UI;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Security;

namespace Zeus.Globalization.ContentTypes
{
	[ContentType]
	[ContentTypeAuthorizedRoles(RoleNames.Administrators)]
	[RestrictParents(typeof(LanguageContainer))]
	public class Language : ContentItem
	{
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Language Code", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Enabled", 30)]
		public bool Enabled
		{
			get { return GetDetail("Enabled", true); }
			set { SetDetail("Enabled", value); }
		}

		[ContentProperty("Web Address Prefix", 40)]
		public virtual string UrlSegment
		{
			get { return GetDetail<string>("UrlSegment", null); }
			set { SetDetail("UrlSegment", value); }
		}

		[ContentProperty("Flag Icon", 50), ImageDataUploadEditor("Flag Icon", 50)]
		public virtual ImageData FlagIcon
		{
			get { return GetDetail<ImageData>("FlagIcon", null); }
			set { SetDetail("FlagIcon", value); }
		}

		public override string IconUrl
		{
			get
			{
				if (FlagIcon != null)
					return this.GetImageUrl("FlagIcon");
				return WebResourceUtility.GetUrl(typeof(Language), "Zeus.Web.Resources.Icons.world_link.png");
			}
		}

		public CultureInfo Culture
		{
			get { return CultureInfo.GetCultureInfo(Name); }
		}
	}
}