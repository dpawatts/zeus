using System.ComponentModel;

namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public enum PaymentCardType
	{
		None = -1,

		[Description("VISA Credit")]
		VisaCredit,

		[Description("VISA Debit")]
		VisaDebit,

		[Description("VISA Electron")]
		VisaElectron,

		MasterCard,

		Maestro,

		[Description("American Express")]
		AmericanExpress,

		[Description("Diner's Club")]
		DinersClub,

		[Description("JCB Card")]
		JcbCard,

		Laser,

		Solo
	}
}