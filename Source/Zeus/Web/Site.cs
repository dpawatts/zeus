using System;
using System.Collections.Specialized;
using Zeus.Configuration;

namespace Zeus.Web
{
	/// <summary>
	/// Represents a site to the Zeus engine. A site defines a start page, a root
	/// page and a host url.
	/// </summary>
	public class Site
	{
		private readonly HostNameCollection _hostNames;

		#region Constructor

		public Site(int rootItemID, int startPageID, HostNameCollection hostNames)
		{
			RootItemID = rootItemID;
			StartPageID = startPageID;
			_hostNames = hostNames;
		}

		#endregion

		#region Properties

		/// <summary>Matches hosts that ends with the site's authority, e.g. match both www.zeus.com and zeus.com.</summary>
		public bool Wildcards { get; set; }

		public int StartPageID { get; set; }
		public int RootItemID { get; set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return base.ToString() + " #" + StartPageID;
		}

		public override bool Equals(object obj)
		{
			if (obj is Site)
			{
				Site other = obj as Site;
				return other.RootItemID == RootItemID
					&& other.StartPageID == StartPageID;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return RootItemID.GetHashCode() + StartPageID.GetHashCode();
			}
		}

		public StringDictionary GetHostLanguageMappings()
		{
			StringDictionary dictionary = new StringDictionary();
			foreach (HostNameElement element in _hostNames)
				if (element.Name != "*")
					dictionary.Add(element.Name, element.Language);
			return dictionary;
		}

		public string GetHostName(string languageCode)
		{
			foreach (HostNameElement element in _hostNames)
				if (element.Name == "*")
					return null;
				else if (string.IsNullOrEmpty(element.Language))
					return element.Name;
				else if (element.Language == languageCode)
					return element.Name;
			return null;
		}

		#endregion
	}
}