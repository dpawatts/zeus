using System.IO;

namespace Protx.Vsp
{
	public class RepeatResponse : VspSecuredResponse
	{
		internal RepeatResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}