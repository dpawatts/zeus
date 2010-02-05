using System.Globalization;
using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Security;
using Image = Zeus.FileSystem.Images.Image;

namespace Zeus.Globalization.ContentTypes
{
	[ContentType]
	[ContentTypeAuthorizedRoles(RoleNames.Administrators)]
	[RestrictParents(typeof(LanguageContainer))]
	[Translatable(false)]
	[AllowedChildren(typeof(Image))]
	public class Language : DataContentItem
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

		[ContentProperty("Flag", 50)]
		public Icon FlagIcon
		{
			get { return GetDetail("FlagIcon", Icon.WorldLink); }
			set { SetDetail("FlagIcon", value); }
		}

		protected override Icon Icon
		{
			get { return FlagIcon; }
		}

		public CultureInfo Culture
		{
			get { return CultureInfo.GetCultureInfo(Name); }
		}
	}
}