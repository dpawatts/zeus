using System;
using Zeus.Web.UI;
using Zeus;
using System.Web;
using System.Web.Security;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public abstract class SecurePage<T> : OnlineShopPage<T>
		where T : ContentItem
	{
		public Web.Items.Customer Customer
		{
			get { return Zeus.Context.Persister.Get<Web.Items.Customer>((int) Membership.GetUser().ProviderUserKey); }
		}
	}
}
