using System.IO;

namespace Protx.Vsp
{
	public class VspSecuredResponse : VspTransactedResponse
	{
		internal VspSecuredResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}

		public string SecurityKey
		{
			get { return base.Parameters.SecurityKey; }
		}
	}
}