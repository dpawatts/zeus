using System.IO;

namespace Protx.Vsp
{
	public class VspTransactedResponse : VspNewTransactionResponse
	{
		internal VspTransactedResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}

		public string TxAuthNo
		{
			get { return base.Parameters.TxAuthNo; }
		}
	}
}