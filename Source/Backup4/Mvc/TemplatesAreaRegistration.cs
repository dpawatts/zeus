using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Zeus.Templates.Web.Routing;
using Zeus.Web.Mvc;
using Zeus.Web.Routing;

namespace Zeus.Templates.Mvc
{
	public class TemplatesAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "Templates";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}

		protected override void RegisterArea(AreaRegistrationContext context, Assembly assembly, SparkViewFactory sparkViewFactory)
		{
			context.Routes.Add(new Route("assets/default/{*resource}",
				new RouteValueDictionary(),
				new RouteValueDictionary(),
				new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".Mvc.DefaultTemplate.Assets")));

			context.Routes.Add(new Route("services/templates/bbcode", new BBCodeRouteHandler()));

			var viewFolder = new EmbeddedViewFolder(assembly, assembly.GetName().Name + ".Mvc.DefaultTemplate.Views");

			sparkViewFactory.ViewFolder = sparkViewFactory.ViewFolder
				.Append(viewFolder);
		}
	}
}