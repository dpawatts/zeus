using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Web.Mvc.Modules;

namespace Zeus.Web.Mvc
{
	public class MvcGlobal : Global
	{
		public static void RegisterRoutes(RouteCollection routes, ContentEngine engine)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			string adminPath = Zeus.Context.Current.Resolve<AdminSection>().Path;
			routes.IgnoreRoute(adminPath + "/{*pathInfo}");

			// This route detects content item paths and executes their controller
			routes.Add(new ContentRoute(engine));

			// This controller fallbacks to a controller unrelated to Zeus
			routes.MapRoute(
				 "Default",                                              // Route name
				 "{controller}/{action}/{id}",                           // URL with parameters
				 new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);
		}

		public override void Init()
		{
			// normally the engine is initialized by the initializer module but it can also be initialized this programmatically
			// since we attach programmatically we need to associate the event broker with a http application
			EventBroker.Instance.Attach(this);

			base.Init();
		}

		protected override void OnApplicationStart(EventArgs e)
		{
			ContentEngine engine = Zeus.Context.Initialize(false);

			SparkApplication app = new SparkApplication();
			app.RegisterViewEngine(ViewEngines.Engines);
			app.RegisterPackages(engine, RouteTable.Routes, ViewEngines.Engines);

			ControllerBuilder.Current.SetControllerFactory(engine.Resolve<IControllerFactory>());

			RegisterRoutes(RouteTable.Routes, engine);

			ModelMetadataProviders.Current = new CustomDataAnnotationsModelMetadataProvider();
			//ModelBinders.Binders.DefaultBinder = new FluentValidationModelBinder(new AttributedValidatorFactory());

			base.OnApplicationStart(e);
		}
	}
}