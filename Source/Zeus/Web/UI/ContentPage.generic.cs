using System;
using Isis.Web.Security;

namespace Zeus.Web.UI
{
	public abstract class ContentPage<TPage> : System.Web.UI.Page, IContentTemplate, IContentItemContainer
		where TPage : ContentItem
	{
		public TPage CurrentItem
		{
			get;
			set;
		}

		ContentItem IContentTemplate.CurrentItem
		{
			get { return CurrentItem; }
			set { CurrentItem = (TPage) value; }
		}

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentItem; }
		}

		public IUser CurrentUser
		{
			get
			{
				WebPrincipal webPrincipal = User as WebPrincipal;
				return (webPrincipal != null) ? webPrincipal.MembershipUser : null;
			}
		}
	}
}
