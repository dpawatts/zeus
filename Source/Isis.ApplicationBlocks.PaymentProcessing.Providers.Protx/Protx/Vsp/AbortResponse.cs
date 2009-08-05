using System.IO;

namespace Protx.Vsp
{
	public class AbortResponse : VspResponse
	{
		internal AbortResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}