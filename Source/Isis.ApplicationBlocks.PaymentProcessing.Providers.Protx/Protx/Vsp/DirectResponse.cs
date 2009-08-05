using System.IO;

namespace Protx.Vsp
{
	public class DirectResponse : VspSecuredResponse
	{
		internal DirectResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}

		public CheckResult AddressResult
		{
			get { return base.Parameters.AddressResult; }
		}

		public string AddressResultText
		{
			get { return base.Parameters.AddressResultText; }
		}

		public AvsCv2Result AvsCv2
		{
			get { return base.Parameters.AvsCv2; }
		}

		public string AvsCv2Text
		{
			get { return base.Parameters.AvsCv2Text; }
		}

		public CheckResult Cv2Result
		{
			get { return base.Parameters.Cv2Result; }
		}

		public string Cv2ResultText
		{
			get { return base.Parameters.Cv2ResultText; }
		}

		public CheckResult PostCodeResult
		{
			get { return base.Parameters.PostCodeResult; }
		}

		public string PostCodeResultText
		{
			get { return base.Parameters.PostCodeResultText; }
		}
	}
}