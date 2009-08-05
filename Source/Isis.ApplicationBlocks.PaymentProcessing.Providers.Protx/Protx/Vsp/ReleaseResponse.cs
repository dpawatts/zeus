using System.IO;

namespace Protx.Vsp
{
	public class ReleaseResponse : VspResponse
	{
		internal ReleaseResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}