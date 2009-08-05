using System.IO;

namespace Protx.Vsp
{
	public class VspResponse
	{
		private readonly VspResponseParameters _parameters;
		private readonly VspTransaction _tx;

		internal VspResponse(VspTransaction tx, Stream responseStream)
		{
			_tx = tx;
			_parameters = new VspResponseParameters(responseStream);
		}

		internal VspResponseParameters Parameters
		{
			get { return _parameters; }
		}

		public VspStatusType Status
		{
			get { return _parameters.Status; }
		}

		public string StatusDetail
		{
			get { return _parameters.StatusDetail; }
		}

		public string StatusText
		{
			get { return _parameters.StatusText; }
		}

		public VspTransaction Transaction
		{
			get { return _tx; }
		}

		public string VPSProtocol
		{
			get { return _parameters.VPSProtocol; }
		}
	}
}