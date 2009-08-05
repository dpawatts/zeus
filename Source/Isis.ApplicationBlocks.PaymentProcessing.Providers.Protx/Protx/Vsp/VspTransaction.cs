using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Protx.Vsp
{
	public class VspTransaction
	{
		private readonly VspTransactionParametersCollection _parameters = new VspTransactionParametersCollection();
		private readonly VspServiceType _serviceType;

		protected internal VspTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                  string vendorTxCode)
		{
			_parameters.VPSProtocol = version;
			_parameters.TxType = txType;
			_parameters.Vendor = VspConfiguration.GetConfig().Vendor;
			_parameters.VendorTxCode = vendorTxCode;
			_serviceType = serviceType;
		}

		protected virtual Stream InternalSend()
		{
			Uri vspDirectUrl;
			VspConfiguration config = VspConfiguration.GetConfig();
			if (_serviceType == VspServiceType.VspDirectTX)
			{
				vspDirectUrl = config.ServerInfo.VspDirectUrl;
			}
			else
			{
				vspDirectUrl = new Uri(config.ServerInfo.VspServerUrl + "?Service=" + _serviceType.ToString("G"));
			}
			int num = config.Timeout*0x3e8;
			WebRequest request = WebRequest.Create(vspDirectUrl);
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";
			request.Timeout = num;
			Encoding e = Encoding.GetEncoding(0x4e4);
			using (Stream stream = request.GetRequestStream())
			{
				using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
				{
					bool flag = true;
					foreach (string str in _parameters.AllKeys)
					{
						if (flag)
						{
							flag = false;
						}
						else
						{
							writer.Write('&');
						}
						writer.Write(HttpUtility.UrlEncode(str, e));
						writer.Write('=');
						writer.Write(HttpUtility.UrlEncode(_parameters[str], e));
					}
				}
			}
			return request.GetResponse().GetResponseStream();
		}

		protected static void ValidateTranactionType(VspTransactionType txType, params VspTransactionType[] allowed)
		{
			for (int i = 0; i < allowed.Length; i++)
			{
				if (txType == allowed[i])
				{
					return;
				}
			}
			throw new ArgumentOutOfRangeException("txType", txType, "Invalid transaction type");
		}

		internal VspTransactionParametersCollection Parameters
		{
			get { return _parameters; }
		}

		public VspTransactionType TxType
		{
			get { return _parameters.TxType; }
		}

		public string Vendor
		{
			get { return _parameters.Vendor; }
		}

		public string VendorTxCode
		{
			get { return _parameters.VendorTxCode; }
			set
			{
				if (((value == null) || (value.Length == 0)) || (value.Length > 40))
				{
					throw new ArgumentException("value");
				}
				_parameters.VendorTxCode = value;
			}
		}

		public string VPSProtocol
		{
			get { return _parameters.VPSProtocol; }
		}
	}
}