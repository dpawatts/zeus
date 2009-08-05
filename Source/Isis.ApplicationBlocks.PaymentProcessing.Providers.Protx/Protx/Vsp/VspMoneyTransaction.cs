using System.IO;

namespace Protx.Vsp
{
	public class VspMoneyTransaction : VspTransaction
	{
		protected internal VspMoneyTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                       string vendorTxCode) : base(version, serviceType, txType, vendorTxCode)
		{
		}

		protected override Stream InternalSend()
		{
			base.Parameters.Require(VspParameter.Amount);
			base.Parameters.DefaultCurrency();
			base.Parameters.DefaultDescription();
			return base.InternalSend();
		}

		public decimal Amount
		{
			get { return base.Parameters.Amount; }
			set { base.Parameters.Amount = value; }
		}

		public string Currency
		{
			get { return base.Parameters.Currency; }
			set { base.Parameters.Currency = value; }
		}

		public string Description
		{
			get { return base.Parameters.Description; }
			set { base.Parameters.Description = value; }
		}
	}
}