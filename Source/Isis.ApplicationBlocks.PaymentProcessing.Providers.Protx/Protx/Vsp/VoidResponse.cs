using System.IO;

namespace Protx.Vsp
{
	public class VoidResponse : VspResponse
	{
		internal VoidResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}