using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Spark.Web.Mvc.Descriptors;

namespace Zeus.Web.Mvc.Descriptors
{
	/// <summary>
	/// Allows a mobile device-specific view to be used. For example, an iPhone-specific view.
	/// Based on documentation at http://www.sparkviewengine.com/documentation/viewlocations.
	/// If you have a view in /Views/Home/Index.spark, then you could create an iPhone version
	/// in /Views/Home/Mobile/iPhone/Index.spark. The actual folder name is taken from the DeviceFolders
	/// property, defined in the class below.
	/// 
	/// Code also borrowed from
	/// http://code.msdn.microsoft.com/WebAppToolkitMobile/Release/ProjectReleases.aspx?ReleaseId=3259.
	/// </summary>
	public class MobileDeviceDescriptorFilter : DescriptorFilterBase
	{
		public MobileDeviceDescriptorFilter()
		{
			DeviceFolders = new StringDictionary
			{
				{ "Pocket IE", "WindowsMobile" },
				{ "AppleMAC-Safari", "iPhone" }
			};
		}

		public StringDictionary DeviceFolders { get; private set; }

		public override void ExtraParameters(ControllerContext context, IDictionary<string, object> extra)
		{
			if (context.HttpContext.Request.Browser.IsMobileDevice)
			{
				extra["mobile"] = true;
				extra["device"] = RetrieveDeviceFolderName(context.HttpContext.Request.Browser.Browser);
			}
		}

		/// <summary>
		/// Get the device folder associated with the name of the browser.
		/// </summary>
		/// <param name="browser">Name of the browser.</param>
		/// <returns>The associated folder name.</returns>
		private string RetrieveDeviceFolderName(string browser)
		{
			if (DeviceFolders.ContainsKey(browser))
				return DeviceFolders[browser.Trim()];
			return "unknown";
		}

		public override IEnumerable<string> PotentialLocations(IEnumerable<string> locations, IDictionary<string, object> extra)
		{
			if (!extra.ContainsKey("mobile") || !((bool)extra["mobile"]))
				return locations;

			// The locations collection contains items of this form:
			// Home/Index.spark
			// We need to insert the mobile stuff between the final "/", and the filename.
			string device = extra["device"] as string;
			var locations1 = locations.Select(x => InsertMobileStuffInViewLocation(x, "Mobile\\" + device + "\\"));
			var locations2 = locations.Select(x => InsertMobileStuffInViewLocation(x, "Mobile\\"));
			return locations1.Concat(locations2).Concat(locations);
		}

		private static string InsertMobileStuffInViewLocation(string location, string mobileStuff)
		{
			if (!location.Contains("\\"))
				return location;

			return location.Substring(0, location.LastIndexOf('\\') + 1)
			       + mobileStuff
			       + location.Substring(location.LastIndexOf('\\') + 1);
		}
	}
}