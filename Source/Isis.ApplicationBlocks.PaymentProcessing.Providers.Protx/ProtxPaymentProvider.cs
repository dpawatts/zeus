using System;
using Protx.Vsp;
using ProtxVsp = Protx.Vsp;

namespace Isis.ApplicationBlocks.PaymentProcessing.Providers.Protx
{
	/// <summary>
	/// Summary description for ProtxPaymentProvider.
	/// </summary>
	public class ProtxPaymentProvider : PaymentProviderBase
	{
		public override PaymentResponse AuthAndBank(PaymentRequest payment)
		{
			#region Construct request

			DirectTransaction lDirectTransaction = new DirectTransaction(payment.OrderID);

			lDirectTransaction.Amount = payment.Amount;
			lDirectTransaction.Currency = payment.CurrencyAlphabeticCode;
			lDirectTransaction.Description = payment.Description;
			lDirectTransaction.CardHolder = payment.CardHolder;
			lDirectTransaction.CardNumber = payment.CardNumber;

			if (payment.StartDate != null)
				lDirectTransaction.StartDate = FormatMonthYear(payment.StartDate.Value);

			lDirectTransaction.ExpiryDate = FormatMonthYear(payment.ExpiryDate);

			if (!string.IsNullOrEmpty(payment.IssueNumber))
				lDirectTransaction.IssueNumber = payment.IssueNumber;

			if (!string.IsNullOrEmpty(payment.CV2))
				lDirectTransaction.CV2 = payment.CV2;

			lDirectTransaction.CardType = MapCardType(payment.CardType);

			lDirectTransaction.BillingAddress = payment.BillingAddress;
			lDirectTransaction.BillingPostCode = payment.BillingPostcode;

			//lDirectTransaction.CustomerName = string.Empty;
			//lDirectTransaction.Basket = null;
			lDirectTransaction.ClientIPAddress = payment.ClientIPAddress;

			#endregion

			#region Send request, and process response

			DirectResponse lDirectResponse = lDirectTransaction.Send();

			PaymentResponse lPaymentRS = new PaymentResponse
			{
				Status = MapStatus(lDirectResponse.Status),
				StatusDetail = lDirectResponse.StatusDetail
			};

			switch (lDirectResponse.Status)
			{
				case VspStatusType.OK:
					lPaymentRS.TransactionID = lDirectResponse.VPSTxID;
					lPaymentRS.AuthNo = lDirectResponse.TxAuthNo;
					lPaymentRS.AvsCv2 = MapAvsCv2Result(lDirectResponse.AvsCv2);
					lPaymentRS.AddressResult = MapCheckResult(lDirectResponse.AddressResult);
					lPaymentRS.PostCodeResult = MapCheckResult(lDirectResponse.PostCodeResult);
					lPaymentRS.Cv2Result = MapCheckResult(lDirectResponse.Cv2Result);
					break;
			}

			return lPaymentRS;

			#endregion
		}

		private static VspCardType MapCardType(CardType cardType)
		{
			switch (cardType)
			{
				case CardType.Visa:
					return VspCardType.VISA;
				case CardType.MasterCard:
					return VspCardType.MC;
				case CardType.VisaDelta:
					return VspCardType.DELTA;
				case CardType.Solo:
					return VspCardType.SOLO;
				case CardType.Maestro:
					return VspCardType.MAESTRO;
				case CardType.AmericanExpress:
					return VspCardType.AMEX;
				case CardType.VisaElectron:
					return VspCardType.UKE;
				case CardType.DinersClub:
					return VspCardType.DC;
				case CardType.JCB:
					return VspCardType.JCB;
				default:
					throw new NotSupportedException("Could not map to VspCardType value: " + cardType);
			}
		}

		private static StatusType MapStatus(VspStatusType eStatus)
		{
			switch (eStatus)
			{
				case VspStatusType.OK:
					return StatusType.OK;
				case VspStatusType.Malformed:
					return StatusType.Malformed;
				case VspStatusType.Invalid:
					return StatusType.Invalid;
				case VspStatusType.Error:
					return StatusType.Undef;
				case VspStatusType.Abort:
					return StatusType.Abort;
				case VspStatusType.NotAuthed:
					return StatusType.NotAuthed;
				case VspStatusType.Rejected:
					return StatusType.Rejected;
				default:
					throw new Exception("Could not map to StatusType value: " + eStatus);
			}
		}

		private static AvsCv2Result MapAvsCv2Result(ProtxVsp.AvsCv2Result eAvsCv2Result)
		{
			switch (eAvsCv2Result)
			{
				case ProtxVsp.AvsCv2Result.None:
					return AvsCv2Result.None;
				case ProtxVsp.AvsCv2Result.SecurityCode:
					return AvsCv2Result.SecurityCode;
				case ProtxVsp.AvsCv2Result.Address:
					return AvsCv2Result.Address;
				case ProtxVsp.AvsCv2Result.All:
					return AvsCv2Result.All;
				case ProtxVsp.AvsCv2Result.Skipped:
					return AvsCv2Result.Skipped;
				default:
					throw new Exception("Could not map to AvsCv2Result value: " + eAvsCv2Result);
			}
		}

		private CheckResult MapCheckResult(ProtxVsp.CheckResult eCheckResult)
		{
			switch (eCheckResult)
			{
				case ProtxVsp.CheckResult.NotProvided:
					return CheckResult.NotProvided;
				case ProtxVsp.CheckResult.NotChecked:
					return CheckResult.NotChecked;
				case ProtxVsp.CheckResult.Matched:
					return CheckResult.Matched;
				case ProtxVsp.CheckResult.NotMatched:
					return CheckResult.NotMatched;
				default:
					throw new Exception("Could not map to CheckResult value: " + eCheckResult);
			}
		}

		private string FormatMonthYear(MonthYear tMonthYear)
		{
			return tMonthYear.Month.ToString().PadLeft(2, '0') + tMonthYear.Year.ToString().Substring(2);
		}
	}
}