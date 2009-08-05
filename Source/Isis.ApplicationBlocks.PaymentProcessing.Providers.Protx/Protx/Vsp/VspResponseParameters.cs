using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;

namespace Protx.Vsp
{
	internal class VspResponseParameters
	{
		private readonly NameValueCollection _parameters;

		internal VspResponseParameters(Stream stream)
		{
			_parameters = new NameValueCollection(5);
			try
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					string str;
					while ((str = reader.ReadLine()) != null)
					{
						string[] strArray = str.Split(new[] {'='}, 2);
						if (strArray.Length == 2)
						{
							_parameters.Add(strArray[0], strArray[1]);
						}
					}
					reader.Close();
				}
				stream.Close();
			}
			finally
			{
				stream.Dispose();
			}
		}

		internal VspResponseParameters(string text, VspParameter[] parameters)
		{
			_parameters = new NameValueCollection(5);
			int length = parameters.Length;
			int[] array = new int[length + 1];
			int index = 0;
			for (int i = 0; i < length; i++)
			{
				string str = VspTransactionParametersCollection.MakeParamName(parameters[i]);
				string str2 = str + "=";
				int startIndex = -1;
				do
				{
					startIndex = text.IndexOf(str2, (startIndex + 1));
				} while ((startIndex > 0) && (text[startIndex - 1] != '&'));
				if (startIndex >= 0)
				{
					if (text.IndexOf("&" + str2, startIndex) >= 0)
					{
						throw new VspException(string.Format(CultureInfo.InvariantCulture, "Invalid response, parameter {0} is repeated",
						                                     new object[] {str}));
					}
					array[index++] = startIndex;
				}
			}
			array[index] = text.Length + 1;
			Array.Sort(array, 0, index + 1);
			for (int j = 0; j < index; j++)
			{
				try
				{
					int num6 = array[j];
					int num7 = (array[j + 1] - 1) - num6;
					string[] strArray = text.Substring(num6, num7).Split(new[] {'='}, 2);
					if (strArray[1].Length > 0)
					{
						_parameters[strArray[0]] = strArray[1];
					}
				}
				catch (Exception exception)
				{
					object[] args = new object[] {(j + 1).ToString(CultureInfo.InvariantCulture)};
					throw new VspException(
						string.Format(CultureInfo.InvariantCulture, "Error parsing response parameter number {0}", args), exception);
				}
			}
		}

		public CheckResult AddressResult
		{
			get { return (CheckResult) Enum.Parse(typeof (CheckResult), AddressResultText, true); }
		}

		public string AddressResultText
		{
			get { return this[VspParameter.AddressResult]; }
		}

		public decimal Amount
		{
			get { return decimal.Parse(this[VspParameter.Amount], CultureInfo.InvariantCulture); }
		}

		public AvsCv2Result AvsCv2
		{
			get { return VspProtocol.ConvertToAvsCv2(AvsCv2Text); }
		}

		public string AvsCv2Text
		{
			get { return this[VspParameter.AVSCV2]; }
		}

		public string CAVV
		{
			get { return this[VspParameter.CAVV]; }
		}

		public CheckResult Cv2Result
		{
			get { return (CheckResult) Enum.Parse(typeof (CheckResult), Cv2ResultText, true); }
		}

		public string Cv2ResultText
		{
			get { return this[VspParameter.CV2Result]; }
		}

		public bool GiftAid
		{
			get
			{
				string str = this[VspParameter.GiftAid];
				return ((str != null) && (str == "1"));
			}
		}

		protected string this[VspParameter name]
		{
			get
			{
				string str = _parameters[VspTransactionParametersCollection.MakeParamName(name)];
				if (str == null)
				{
					throw new VspException(string.Format("Response parameter not set [{0}]", name));
				}
				return str;
			}
		}

		public string NextURL
		{
			get { return this[VspParameter.NextURL]; }
		}

		public CheckResult PostCodeResult
		{
			get { return (CheckResult) Enum.Parse(typeof (CheckResult), PostCodeResultText, true); }
		}

		public string PostCodeResultText
		{
			get { return this[VspParameter.PostCodeResult]; }
		}

		public string SecurityKey
		{
			get { return this[VspParameter.SecurityKey]; }
		}

		public VspStatusType Status
		{
			get { return (VspStatusType) Enum.Parse(typeof (VspStatusType), StatusText, true); }
		}

		public string StatusDetail
		{
			get { return this[VspParameter.StatusDetail]; }
		}

		public string StatusText
		{
			get { return this[VspParameter.Status]; }
		}

		public ThreeDSecureStatus ThreeDSecureStatus
		{
			get { return (ThreeDSecureStatus) Enum.Parse(typeof (ThreeDSecureStatus), ThreeDSecureStatusText, true); }
		}

		public string ThreeDSecureStatusText
		{
			get { return this[VspParameter._3DSecureStatus]; }
		}

		public string TxAuthNo
		{
			get { return this[VspParameter.TxAuthNo]; }
		}

		public string VendorTxCode
		{
			get { return this[VspParameter.VendorTxCode]; }
		}

		public string VPSProtocol
		{
			get { return this[VspParameter.VPSProtocol]; }
		}

		public string VPSTxID
		{
			get { return this[VspParameter.VPSTxId]; }
		}
	}
}