using System.IO;

namespace Protx.Vsp
{
	public class VspNewTransactionResponse : VspResponse
	{
		internal VspNewTransactionResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}

		public string VPSTxID
		{
			get { return base.Parameters.VPSTxID; }
		}
	}
}