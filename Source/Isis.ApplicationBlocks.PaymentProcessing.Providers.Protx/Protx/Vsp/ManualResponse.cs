using System.IO;

namespace Protx.Vsp
{
	public class ManualResponse : VspSecuredResponse
	{
		internal ManualResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}