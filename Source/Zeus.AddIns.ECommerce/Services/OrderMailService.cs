using Ninject;
using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Net.Mail;
using Zeus.Web.TextTemplating;

namespace Zeus.AddIns.ECommerce.Services
{
	public class OrderMailService : IOrderMailService, IInitializable
	{
		private readonly IMessageBuilder _messageBuilder;
		private readonly IMailSender _mailSender;

		public OrderMailService(IMessageBuilder messageBuilder, IMailSender mailSender)
		{
			_messageBuilder = messageBuilder;
			_mailSender = mailSender;
		}

		public void SendOrderConfirmationToCustomer(IECommerceConfiguration shop, Order order)
		{
			string orderConfirmationCustomerText = _messageBuilder.Transform("OrderConfirmationCustomer",
					new { order = order, text = shop.ConfirmationEmailText });
			_mailSender.Send(shop.ConfirmationEmailFrom, order.EmailAddress, "Order Confirmation",
				orderConfirmationCustomerText);
		}

		public void SendOrderConfirmationToVendor(IECommerceConfiguration shop, Order order)
		{
			string orderConfirmationVendorText = _messageBuilder.Transform("OrderConfirmationVendor",
					new { order = order });
			_mailSender.Send(shop.ConfirmationEmailFrom, shop.VendorEmail, "Order Confirmation",
				orderConfirmationVendorText);
		}

		public void Initialize()
		{
			_messageBuilder.Initialize(GetType().Assembly, "Zeus.AddIns.ECommerce.Templates");
		}
	}
}