using System;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Admin
{
	public class Navigator
	{
		private readonly IPersister _persister;
		private readonly IHost _host;

		public Navigator(IPersister persister, IHost host)
		{
			_persister = persister;
			_host = host;
		}

		public ContentItem Navigate(ContentItem startingPoint, string path)
		{
			return startingPoint.GetChild(path);
		}

		public ContentItem Navigate(string path)
		{
			if (path == null) throw new ArgumentNullException("path");
			if (!path.StartsWith("/"))
			{
				if (path.StartsWith("~"))
					return Navigate(_persister.Get(_host.CurrentSite.StartPageID), path.Substring(1));
				throw new ArgumentException("The path must start with a slash '/', was '" + path + "'", "path");
			}

			return Navigate(_persister.Get(_host.CurrentSite.RootItemID), path);
		}
	}
}
