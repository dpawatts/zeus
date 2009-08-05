namespace Protx.Vsp
{
	public enum VspTransactionType
	{
		Payment,
		Deferred,
		PreAuth,
		Release,
		Abort,
		Refund,
		Repeat,
		RepeatDeferred,
		Void,
		Manual,
		DirectRefund
	}
}