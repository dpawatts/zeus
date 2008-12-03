using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeus.Configuration;

namespace Zeus.Web
{
	public class Host
	{
		public int RootItemID
		{
			get;
			set;
		}

		public Host(HostSection hostSection)
		{
			this.RootItemID = hostSection.RootItemID;
		}
	}
}
