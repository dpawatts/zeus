using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;
using Zeus.FileSystem;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Start Page")]
	[RestrictParents(typeof(RootItem))]
	public class StartPage : Page, IFileSystemContainer
	{
		protected override string IconName
		{
			get { return "house"; }
		}
	}
}
