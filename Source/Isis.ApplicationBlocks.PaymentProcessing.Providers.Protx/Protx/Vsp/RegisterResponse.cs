using System.IO;

namespace Protx.Vsp
{
	public class RegisterResponse : VspNewTransactionResponse
	{
		internal RegisterResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}

		public string NextURL
		{
			get { return base.Parameters.NextURL; }
		}

		public string SecurityKey
		{
			get { return base.Parameters.SecurityKey; }
		}
	}
}