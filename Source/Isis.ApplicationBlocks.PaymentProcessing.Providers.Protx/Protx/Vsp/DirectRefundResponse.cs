using System.IO;

namespace Protx.Vsp
{
	public class DirectRefundResponse : VspTransactedResponse
	{
		internal DirectRefundResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}