using System;
using Zeus.Web.UI;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class GiftCardPersonalise : ContentPage<Bermedia.Gibbons.Web.Items.GiftCardPersonalisePage>
	{
		protected void csvAmount_ServerValidate(object sender, ServerValidateEventArgs e)
		{
			decimal result;
			e.IsValid = !string.IsNullOrEmpty(e.Value) && decimal.TryParse(e.Value, out result) && result >= 25;
		}
	}
}
