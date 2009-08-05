namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// List of specific check results
	/// </summary>
	public enum CheckResult
	{
		/// <summary>
		/// Data was not provided
		/// </summary>
		NotProvided,

		/// <summary>
		/// Data provided, but not checked
		/// </summary>
		NotChecked,

		/// <summary>
		/// Data provided matched
		/// </summary>
		Matched,

		/// <summary>
		/// Data provided did not match
		/// </summary>
		NotMatched,
	}
}
