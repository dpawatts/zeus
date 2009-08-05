namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// List of security check results
	/// </summary>
	public enum AvsCv2Result
	{
		/// <summary>
		/// No dta matches
		/// </summary>
		None,

		/// <summary>
		/// Security code match only
		/// </summary>
		SecurityCode,

		/// <summary>
		/// Address match only
		/// </summary>
		Address,

		/// <summary>
		/// All match
		/// </summary>
		All,

		/// <summary>
		/// DATA NOT CHECKED
		/// </summary>
		Skipped,
	}
}
