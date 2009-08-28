using System;
using FluentValidation;
using FluentValidation.Attributes;
using Isis.ExtensionMethods;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	[Validator(typeof(CheckoutPageFormModelValidator))]
	public class CheckoutPageFormModel
	{
		public string BillingTitle { get; set; }
		public string BillingFirstName { get; set; }
		public string BillingSurname { get; set; }
		public string BillingAddressLine1 { get; set; }
		public string BillingAddressLine2 { get; set; }
		public string BillingTownCity { get; set; }
		public string BillingPostcode { get; set; }

		public string DeliveryAddressSameAsBillingAddress { get; set; }

		public bool IsDeliveryAddressSameAsBillingAddress
		{
			get { return DeliveryAddressSameAsBillingAddress == "on"; }
		}

		public string ShippingTitle { get; set; }
		public string ShippingFirstName { get; set; }
		public string ShippingSurname { get; set; }
		public string ShippingAddressLine1 { get; set; }
		public string ShippingAddressLine2 { get; set; }
		public string ShippingTownCity { get; set; }
		public string ShippingPostcode { get; set; }

		public string CardType { get; set; }
		public string CardHolderName { get; set; }
		public string CardNumber { get; set; }

		public int? CardExpiryMonth { get; set; }
		public int? CardExpiryYear { get; set; }

		public DateTime ValidTo
		{
			get
			{
				if (CardExpiryMonth == null || CardExpiryYear == null)
					return DateTime.MinValue;
				return new DateTime(CardExpiryYear.Value, CardExpiryMonth.Value,
					DateTime.DaysInMonth(CardExpiryYear.Value, CardExpiryMonth.Value));
			}
		}

		public int? CardStartMonth { get; set; }
		public int? CardStartYear { get; set; }

		public DateTime ValidFrom
		{
			get
			{
				if (CardStartMonth == null || CardStartYear == null)
					return DateTime.MinValue;
				return new DateTime(CardStartYear.Value, CardStartMonth.Value, 1);
			}
		}

		public string CardIssueNumber { get; set; }
		public string CardVerificationCode { get; set; }

		public string EmailAddress { get; set; }
		public string ConfirmEmailAddress { get; set; }
		public string TelephoneNumber { get; set; }
		public string AcceptTermsAndConditions { get; set; }

		public bool DoesAcceptTermsAndConditions
		{
			get { return AcceptTermsAndConditions == "on"; }
		}
	}

	public class CheckoutPageFormModelValidator : AbstractValidator<CheckoutPageFormModel>
	{
		public CheckoutPageFormModelValidator()
		{
			RuleFor(x => x.BillingTitle).NotEmpty();
			RuleFor(x => x.BillingFirstName).NotEmpty();
			RuleFor(x => x.BillingSurname).NotEmpty();
			RuleFor(x => x.BillingAddressLine1).NotEmpty();
			RuleFor(x => x.BillingAddressLine2).NotEmpty();
			RuleFor(x => x.BillingTownCity).NotEmpty();
			RuleFor(x => x.BillingPostcode).NotEmpty();

			RuleFor(x => x.ShippingTitle).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingFirstName).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingSurname).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingAddressLine1).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingAddressLine2).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingTownCity).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);
			RuleFor(x => x.ShippingPostcode).NotEmpty().When(x => !x.IsDeliveryAddressSameAsBillingAddress);

			RuleFor(x => x.CardType).NotEmpty();
			RuleFor(x => x.CardHolderName).NotEmpty();
			RuleFor(x => x.CardNumber).NotEmpty()
				.Must(c => c.LuhnChecksum() == 0).WithMessage("Card number is not valid.");
			RuleFor(x => x.CardExpiryMonth).NotEmpty();
			RuleFor(x => x.CardExpiryYear).NotEmpty();
			RuleFor(x => x.ValidTo).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Card expiry date must not be in the past.");
			RuleFor(x => x.ValidFrom)
				.LessThanOrEqualTo(DateTime.Now).WithMessage("Card start date must not be in the future.").When(x => x.CardStartMonth != null && x.CardStartYear != null);

			RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
			RuleFor(x => x.ConfirmEmailAddress).Equal(x => x.EmailAddress);
			RuleFor(x => x.TelephoneNumber).NotEmpty();

			RuleFor(x => x.DoesAcceptTermsAndConditions).Equal(true)
				.WithMessage("Please check the box to indicate you have read and understood the Terms & Conditions.");
		}
	}
}