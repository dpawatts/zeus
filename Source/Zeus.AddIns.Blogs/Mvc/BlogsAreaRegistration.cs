using System.Reflection;
using System.Web.Mvc;
using Spark.Web.Mvc;
using Zeus.AddIns.Blogs.Web.Pingback;
using Zeus.AddIns.Blogs.Web.Routing;
using Zeus.AddIns.Blogs.Web.XmlRpc;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.Blogs.Mvc
{
	public class BlogsAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "Blogs";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}

		protected override void RegisterArea(AreaRegistrationContext context, Assembly assembly, SparkViewFactory sparkViewFactory)
		{
			context.Routes.MapXmlRpcHandler<BlogXmlRpcService>("services/metaweblog");
			context.Routes.MapXmlRpcHandler<PingbackXmlRpcService>("services/pingbacks");
		}
	}
}