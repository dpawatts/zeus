using System.IO;

namespace Protx.Vsp
{
	public class VspCardTransaction : VspMoneyTransaction
	{
		protected internal VspCardTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                      string vendorTxCode) : base(version, serviceType, txType, vendorTxCode)
		{
		}

		protected override Stream InternalSend()
		{
			base.Parameters.RequireCardFields();
			return base.InternalSend();
		}

		public string CardHolder
		{
			get { return base.Parameters.CardHolder; }
			set { base.Parameters.CardHolder = value; }
		}

		public string CardNumber
		{
			get { return base.Parameters.CardNumber; }
			set { base.Parameters.CardNumber = value; }
		}

		public VspCardType CardType
		{
			get { return base.Parameters.CardType; }
			set { base.Parameters.CardType = value; }
		}

		public string ExpiryDate
		{
			get { return base.Parameters.ExpiryDate; }
			set { base.Parameters.ExpiryDate = value; }
		}

		public string IssueNumber
		{
			get { return base.Parameters.IssueNumber; }
			set { base.Parameters.IssueNumber = value; }
		}

		public string StartDate
		{
			get { return base.Parameters.StartDate; }
			set { base.Parameters.StartDate = value; }
		}
	}
}