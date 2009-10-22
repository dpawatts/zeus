using System;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Zeus.BaseLibrary.Xml
{
	/// <summary>
	/// Summary description for XmlCachedDocument.
	/// </summary>
	public class CachedXmlDocument
	{
		private XmlDocument m_pDoc;

		public XmlDocument Document
		{
			get { return m_pDoc; }
		}

		public CachedXmlDocument(string sFilename)
		{
			HttpContext pContext = HttpContext.Current;

			string sKey = "Zeus.BaseLibrary.Xml.CachedXmlDocument: " + sFilename.ToLower();

			// see if object is in cache
			object pObject = pContext.Cache.Get(sKey);
			if (pObject != null)
			{
				m_pDoc = (XmlDocument) pObject;
			}
			else
			{
				// place file in cache
				if (sFilename.IndexOf(":\\") > 0)
				{
					// absolute location specified
				}
				else
				{
					sFilename = pContext.Server.MapPath(sFilename);
				}

				m_pDoc = new XmlDocument();
				m_pDoc.Load(sFilename);
				pContext.Cache.Insert(sKey, m_pDoc, new CacheDependency(sFilename));
			}
		}
	}
}