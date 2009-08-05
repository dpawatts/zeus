namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// Status values returned from a transaction
	/// </summary>
	public enum StatusType
	{
		/// <summary>
		/// Process executed without error
		/// </summary>
		OK,

		/// <summary>
		/// Input message was malformed – normally will only occur 
		/// during development and vendor integration. StatusDetail 
		/// will give more information
		/// </summary>
		Malformed,

		/// <summary>
		/// Unable to authenticate the vendor or a non code-related 
		/// problem occurred registering the transaction
		/// </summary>
		Invalid,

		/// <summary>
		/// A code-related error occurred which prevented the process 
		/// from executing successfully
		/// </summary>
		Error,

		/// <summary>
		/// Error, none of the above. Should never occur but can be 
		/// synonymous to ERROR
		/// </summary>
		Undef,

		/// <summary>
		/// The Transaction could not be completed due to a timeout 
		/// or a user action which caused the Transaction to be 
		/// prematurely terminated (e.g. a cancellation prior to 
		/// authorisation)
		/// </summary>
		Abort,

		/// <summary>
		/// The payment provider could not authorise the transaction because the 
		/// details provided by the Customer were incorrect, not 
		/// authenticated or could not support the Transaction
		/// </summary>
		NotAuthed,

		/// <summary>
		/// The payment provider rejected the transaction because of
		/// the rules you have set on your account.
		/// </summary>
		Rejected
	}
}
