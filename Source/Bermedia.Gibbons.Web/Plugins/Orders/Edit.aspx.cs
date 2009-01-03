using System;
using Zeus.ContentTypes;
using Zeus.Admin;
using SoundInTheory.PaymentProcessing;
using System.Web.UI.WebControls;
using Isis;

namespace Bermedia.Gibbons.Web.Plugins.Orders
{
	public partial class Edit : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				ReBind();
		}

		private Web.Items.Order SelectedOrder
		{
			get { return (Web.Items.Order) this.SelectedItem; }
		}

		private void ReBind()
		{
			fmvEntity.DataSource = new object[] { this.SelectedItem };
			fmvEntity.DataBind();

			Web.Items.Order order = this.SelectedOrder;
			switch (order.Status)
			{
				case Web.Items.OrderStatus.New:
					if (!order.DeliveryType.RequiresShippingAddress)
						btnCollect.Visible = true;
					else
						btnShip.Visible = true;
					btnDeleteCancel.Visible = true;
					break;
				case Web.Items.OrderStatus.Collected:
				case Web.Items.OrderStatus.Shipped:
					btnDeleteRefund.Visible = true;
					btnRefundSelected.Visible = true;
					break;
			}
		}

		protected bool HasOrderBeenPaidFor()
		{
			return this.SelectedOrder.Status.EqualsAny(Web.Items.OrderStatus.Collected, Web.Items.OrderStatus.Shipped);
		}

		protected void btnUpdateTrackingNumber_Click(object sender, EventArgs e)
		{
			Web.Items.Order order = this.SelectedOrder;
			order.TrackingNumber = txtTrackingNumber.Text;
			Zeus.Context.Persister.Save(order);
			ReBind();
		}

		protected void btnCollect_Click(object sender, EventArgs e)
		{
			DoPaymentAction(delegate(Payment payment, Web.Items.Order order)
			{
				PaymentManager.Bank(payment);
				order.Status = Web.Items.OrderStatus.Collected;
			});
		}

		protected void btnShip_Click(object sender, EventArgs e)
		{
			DoPaymentAction(delegate(Payment payment, Web.Items.Order order)
			{
				PaymentManager.Bank(payment);
				order.Status = Web.Items.OrderStatus.Shipped;
			});
		}

		protected void btnDeleteRefund_Click(object sender, EventArgs e)
		{
			DoPaymentAction(delegate(Payment payment, Web.Items.Order order)
			{
				PaymentManager.Refund(payment);
				order.Status = Web.Items.OrderStatus.Deleted;
			});
		}

		protected void btnDeleteCancel_Click(object sender, EventArgs e)
		{
			DoPaymentAction(delegate(Payment payment, Web.Items.Order order)
				{
					PaymentManager.Reverse(payment);
					order.Status = Web.Items.OrderStatus.Deleted;
				});
		}

		protected void btnRefundSelected_Click(object sender, EventArgs e)
		{
			foreach (RepeaterItem repeaterItem in rptOrderedProducts.Items)
			{
				HiddenField hdnOrderItemID = (HiddenField) repeaterItem.FindControl("hdnOrderItemID");
				CheckBox chkCancel = (CheckBox) repeaterItem.FindControl("chkCancel");
				if (chkCancel.Checked)
				{
					Web.Items.OrderItem orderItem = Zeus.Context.Persister.Get<Web.Items.OrderItem>(Convert.ToInt32(hdnOrderItemID.Value));
					DoPaymentAction(delegate(Payment payment, Web.Items.Order order)
					{
						payment.amount = orderItem.Price;
						PaymentManager.Refund(payment);
					});
				}
			}
		}

		private void DoPaymentAction(Action<Payment, Web.Items.Order> action)
		{
			lblError.Text = string.Empty;

			try
			{
				Web.Items.Order order = this.SelectedOrder;
				Payment payment = new Payment(order.ID.ToString(), order.TotalPrice, 060);
				action(payment, order);
				Zeus.Context.Persister.Save(order);
			}
			catch (PaymentProcessingException ex)
			{
				lblError.Text = "The update could not be performed because payment processing failed:<br/><br/><li>" + ex.ErrorCode + ": " + ex.Message + "</li><br/><br/>Please contact your payment processing administrator for help resolving this problem.<br/><br/>";
			}
			catch (Exception ex)
			{
				lblError.Text = "The update could not be performed because of a technical problem:<br/><br/><li>" + ex.Message + "</li><br/><br/>Please contact your website administrator for help resolving this problem.<br/><br/>";
			}

			ReBind();
		}
	}
}
