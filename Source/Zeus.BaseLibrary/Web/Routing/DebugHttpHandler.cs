using System.Web;
using System.Web.Routing;

namespace Zeus.BaseLibrary.Web.Routing
{
	public class DebugHttpHandler : IHttpHandler
	{
		public RequestContext RequestContext { get; set; }

		public void ProcessRequest(HttpContext context)
		{
			string format = "<html>\r\n<head>\r\n    <title>Route Tester</title>\r\n    <style>\r\n        body, td, th {{font-family: verdana; font-size: small;}}\r\n        .message {{font-size: .9em;}}\r\n        caption {{font-weight: bold;}}\r\n        tr.header {{background-color: #ffc;}}\r\n        label {{font-weight: bold; font-size: 1.1em;}}\r\n        .false {{color: #c00;}}\r\n        .true {{color: #0c0;}}\r\n    </style>\r\n</head>\r\n<body>\r\n<h1>Route Tester</h1>\r\n<div id=\"main\">\r\n    <p class=\"message\">\r\n        Type in a url in the address bar to see which defined routes match it. \r\n        A {{*catchall}} route is added to the list of routes automatically in \r\n        case none of your routes match.\r\n    </p>\r\n    <p><label>Route</label>: {1}</p>\r\n    <div style=\"float: left;\">\r\n        <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"300\">\r\n            <caption>Route Data</caption>\r\n            <tr class=\"header\"><th>Key</th><th>Value</th></tr>\r\n            {0}\r\n        </table>\r\n    </div>\r\n    <div style=\"float: left; margin-left: 10px;\">\r\n        <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"300\">\r\n            <caption>Data Tokens</caption>\r\n            <tr class=\"header\"><th>Key</th><th>Value</th></tr>\r\n            {4}\r\n        </table>\r\n    </div>\r\n    <hr style=\"clear: both;\" />\r\n    <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\">\r\n        <caption>All Routes</caption>\r\n        <tr class=\"header\">\r\n            <th>Matches Current Request</th>\r\n            <th>Url</th>\r\n            <th>Defaults</th>\r\n            <th>Constraints</th>\r\n            <th>DataTokens</th>\r\n        </tr>\r\n        {2}\r\n    </table>\r\n    <hr />\r\n    <strong>AppRelativeCurrentExecutionFilePath</strong>: {3}\r\n</div>\r\n</body>\r\n</html>";
			string str2 = string.Empty;
			RouteData routeData = this.RequestContext.RouteData;
			RouteValueDictionary values = routeData.Values;
			RouteBase base2 = routeData.Route;
			string str3 = string.Empty;
			using (RouteTable.Routes.GetReadLock())
			{
				foreach (RouteBase base3 in RouteTable.Routes)
				{
					bool flag = base3.GetRouteData(this.RequestContext.HttpContext) != null;
					string str4 = string.Format("<span class=\"{0}\">{0}</span>", flag);
					string url = "n/a";
					string str6 = "n/a";
					string str7 = "n/a";
					string str8 = "n/a";
					Route route = base3 as Route;
					if (route != null)
					{
						url = route.Url;
						str6 = FormatRouteValueDictionary(route.Defaults);
						str7 = FormatRouteValueDictionary(route.Constraints);
						str8 = FormatRouteValueDictionary(route.DataTokens);
					}
					str3 = str3 + string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{3}</td></tr>", new object[] { str4, url, str6, str7, str8 });
				}
			}
			string str9 = "n/a";
			string str10 = "";
			if (base2 is DebugRoute)
			{
				str9 = "<strong class=\"false\">NO MATCH!</strong>";
			}
			else
			{
				foreach (string str11 in values.Keys)
				{
					str2 = str2 + string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", str11, values[str11]);
				}
				foreach (string str11 in routeData.DataTokens.Keys)
				{
					str10 = str10 + string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", str11, routeData.DataTokens[str11]);
				}
				Route route2 = base2 as Route;
				if (route2 != null)
				{
					str9 = route2.Url;
				}
			}
			context.Response.Write(string.Format(format, new object[] { str2, str9, str3, context.Request.AppRelativeCurrentExecutionFilePath, str10 }));
		}

		private static string FormatRouteValueDictionary(RouteValueDictionary values)
		{
			if (values == null)
			{
				return "(null)";
			}
			string str = string.Empty;
			foreach (string str2 in values.Keys)
			{
				str = str + string.Format("{0} = {1}, ", str2, values[str2]);
			}
			if (str.EndsWith(", "))
			{
				str = str.Substring(0, str.Length - 2);
			}
			return str;
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}