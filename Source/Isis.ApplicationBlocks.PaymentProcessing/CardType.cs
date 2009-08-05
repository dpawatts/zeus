namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// List of recognised card types
	/// </summary>
	public enum CardType
	{
		/// <summary>
		/// Visa credit or debit card
		/// </summary>
		Visa,

		/// <summary>
		/// Master card
		/// </summary>
		MasterCard,

		/// <summary>
		/// Delta debit card
		/// </summary>
		VisaDelta,

		/// <summary>
		/// Solo card
		/// </summary>
		Solo,

		/// <summary>
		/// Maestro Debit Card
		/// </summary>
		Maestro,

		/// <summary>
		/// American Express Card
		/// </summary>
		AmericanExpress,

		/// <summary>
		/// UKE Card
		/// </summary>
		VisaElectron,

		/// <summary>
		/// Diners Club Card
		/// </summary>
		DinersClub,

		/// <summary>
		/// JCB Card
		/// </summary>
		JCB
	}
}
