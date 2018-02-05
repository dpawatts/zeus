using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Globalization;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	/// <summary>
	/// Redirects to somewhere else. Used as a placeholder in the menu.
	/// </summary>
	[ContentType("External Redirect", "External Redirect", "Redirects to an external address.", "", 40)]
	[RestrictParents(typeof(PageContentItem))]
	public class ExternalRedirect : BasePage
	{
		public override string Url
		{
			get { return GetRedirectItem(); }
		}

		protected virtual string GetRedirectItem()
		{
			return RedirectAddress;
		}

		[TextBoxEditor("Redirect to", 31, Required = true, ContainerName = "Content", Description="Please make sure you enter the complete address including the http prefix", Shared=true)]
		public virtual string RedirectAddress
		{
			get { return GetDetail("RedirectAddress", string.Empty); }
			set { SetDetail("RedirectAddress", value); }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.DoorOut); }
		}
	}
}