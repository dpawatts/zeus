using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Bermedia.Gibbons.Web.Items;
using SoundInTheory.DynamicImage;
using SoundInTheory.PaymentProcessing;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Net.Mail;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CheckoutConfirmation : SecurePage<Web.Items.CheckoutConfirmation>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			bool success = false;

			// Get the POST message from FAC
			string responseString = Server.UrlDecode(new StreamReader(this.Request.InputStream).ReadToEnd());
			if (responseString.Contains("OrderID="))
			{
				try
				{
					// Allow the payment processor to handle the response.
					PaymentProcessingResponse authResponse = PaymentManager.ProcessAuthorizationResponse(responseString);

					Order order = Zeus.Context.Persister.Get<Order>(Convert.ToInt32(authResponse.OrderID));
					order.Status = OrderStatus.New;
					Zeus.Context.Persister.Save(order);

					lblConfirmationText.Text = "Your order has been successfully placed, and your order number is " + authResponse.OrderID + ".";

					success = true;

					try
					{
						string customerEmail = Membership.GetUser(User.Identity.Name).Email;
						SendOrderPlacementConfirmation(order, customerEmail);
						lblConfirmationText.Text += "<br><br>An e-mail confirming this order has been sent to " + customerEmail + ".";
					}
					catch (Exception ex)
					{
						lblConfirmationText.Text += "<br><br>We are sorry, but an e-mail confirming this order could not be sent to you at this time. Your e-mail address may be incorrect or we may be experiencing technical difficulties.";
						if (Convert.ToBoolean(ConfigurationManager.AppSettings["VerboseErrors"]))
							lblConfirmationText.Text += "<br><br>" + ex.ToString().Replace(Environment.NewLine, "<br>");
						lblConfirmationText.CssClass = "error";
					}

					h2SuccessMessage.Visible = true;
				}
				catch (PaymentProcessingException ex)
				{
					// Payment problem
					if (ex.Result == 2)
						lblConfirmationText.Text = "Your order was not placed as your payment was declined.";
					else
						lblConfirmationText.Text = "Your order was not placed as your payment details were incorrect or an error occurred.";
					lblConfirmationText.Text += "<br><br>Bank response: (" + ex.ErrorCode.ToString() + ") " + ex.Message;
					lblConfirmationText.CssClass = "error";
				}
				catch (Exception ex)
				{
					// General problem
					lblConfirmationText.Text = "Your order was not placed as a technical problem occurred.";
					lblConfirmationText.Text += "<br><br>Problem detail: " + ex.Message;
					lblConfirmationText.CssClass = "error";
				};
			}
			else
			{
				// No response?
				lblConfirmationText.Text = "Your order was not placed as a technical problem occurred.";
				lblConfirmationText.Text += "<br><br>Problem detail: No payment response to process.";
				lblConfirmationText.CssClass = "error";
			}

			if (!success)
				h2SuccessMessage.Visible = false;

			ClearShoppingCart();
		}

		private void SendOrderPlacementConfirmation(Order order, string customerEmail)
		{
			string messageText = string.Format(@"Thanks for ordering online at gibbons.bm!

This email has been sent to confirm that your order totalling {0:C2} has been successfully placed.

Your order confirmation number is {1}.

Your order will be ready for pick up or delivery, as you have selected, within 24 hours. Orders
placed during public holidays, Sundays or after 5pm on Fridays will be ready 24 hours from the
following working day.

Thanks for your interest and thanks for shopping at gibbons.bm.
http://www.gibbons.bm", order.TotalPrice, order.ID);

			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send("autoresponse@gibbons.bm",
				customerEmail,
				"Gibbons.bm Order Confirmation",
				messageText);
		}
	}
}
