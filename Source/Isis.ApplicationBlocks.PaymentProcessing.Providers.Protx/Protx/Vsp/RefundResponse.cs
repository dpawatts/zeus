using System.IO;

namespace Protx.Vsp
{
	public class RefundResponse : VspTransactedResponse
	{
		internal RefundResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}