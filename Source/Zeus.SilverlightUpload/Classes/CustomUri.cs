using System;

namespace Zeus.SilverlightUpload.Classes
{
	public class CustomUri : Uri
	{
		public CustomUri(string uri)
			: base(GetAbsoluteUrl(uri))
		{

		}

		public static string GetAbsoluteUrl(string strRelativePath)
		{
			if (string.IsNullOrEmpty(strRelativePath))
				return strRelativePath;

			string strFullUrl;
			if (strRelativePath.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
				|| strRelativePath.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
				|| strRelativePath.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
			{
				//already absolute
				strFullUrl = strRelativePath;
			}
			else
			{
				//relative, need to convert to absolute
				Uri absoluteUri = System.Windows.Application.Current.Host.Source;
				strFullUrl = absoluteUri.Scheme + "://" + absoluteUri.Host;
				if (absoluteUri.Port != 80)
					strFullUrl += ":" + absoluteUri.Port;
				strFullUrl += strRelativePath;
				/*if (strFullUrl.IndexOf("ClientBin") > 0)
					strFullUrl = strFullUrl.Substring(0, strFullUrl.IndexOf("ClientBin")) + strRelativePath;
				else
					strFullUrl = strFullUrl.Substring(0, strFullUrl.LastIndexOf("/") + 1) + strRelativePath;*/
			}

			return strFullUrl;
		}
	}
}