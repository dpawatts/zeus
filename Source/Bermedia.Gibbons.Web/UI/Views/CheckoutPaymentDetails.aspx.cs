using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Bermedia.Gibbons.Web.Items;
using SoundInTheory.DynamicImage;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CheckoutPaymentDetails : SecurePage<Web.Items.CheckoutPaymentDetails>
	{
		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			// We ignore cardholder first and last name -
			// it's not used by FAC.
			this.CheckoutData.FirstName = txtFirstName.Text;
			this.CheckoutData.LastName = txtLastName.Text;
			this.CheckoutData.PaymentCardType = ddlCardType.SelectedValue;
			//this.CheckoutData.Currency = PaymentManager.GetCountryIsoCode(txtCardNumber.Text);
			this.CheckoutData.PaymentCardExpiryDate = new DateTime(
				Convert.ToInt32(ddlExpiryDateYear.Text),
				Convert.ToInt32(ddlExpiryDateMonth.Text),
				1);
			this.CheckoutData.PaymentCardNumber = txtCardNumber.Text;
			this.CheckoutData.PaymentCvv2 = txtCVV2.Text;

			if (Request.QueryString["return"] != null)
				Response.Redirect("~/checkout-summary.aspx");
			else
				Response.Redirect("~/checkout-billing-address.aspx");
		}

		protected void btnCorporateNext_Click(object sender, EventArgs e)
		{
			this.ShoppingCart.PaymentMethod = PaymentMethod.Corporate;
			Zeus.Context.Persister.Save(this.ShoppingCart);

			Response.Redirect("~/checkout-summary.aspx");
		}
	}

	[Serializable]
	public class CheckoutData
	{
		public string FirstName;
		public string LastName;
		public string PaymentCardType;
		public int Currency = int.MinValue;

		public DateTime PaymentCardExpiryDate = DateTime.MinValue;
		public string PaymentCardNumber;
		public string PaymentCvv2;

		public string CardDetails
		{
			get
			{
				string result = string.Empty;

				result = this.PaymentCardNumber;
				if (result == null)
					return string.Empty;
				if (result.Length > 4)
					result = new string('X', result.Length - 4) + result.Substring(result.Length - 4);

				result = this.PaymentCardType + " " + result;
				result += ", expires " + this.PaymentCardExpiryDate.ToString("MM/yyyy");

				return result;
			}
		}
	}
}
