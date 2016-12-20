using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Spark;
using Spark.FileSystem;

namespace Zeus.Web.TextTemplating
{
	public class DefaultMessageBuilder : MessageBuilder
	{
		private readonly ISparkViewEngine _engine;

		public DefaultMessageBuilder()
		{
			_engine = new SparkViewEngine(new SparkSettings() { PageBaseType = "Spark.Web.Mvc.SparkView" });
		}

		public override void Initialize(Assembly templateAssembly, string templateResourcePath)
		{
			if (_engine.Settings is SparkSettings)
			{
				var parameters = new Dictionary<string, string>
				{
					{ "assembly", templateAssembly.ToString() },
					{ "resourcePath", templateResourcePath }
				};
				((SparkSettings) _engine.Settings).AddViewFolder(ViewFolderType.EmbeddedResource, parameters);
			}
		}

		protected override void Transform(string templateName, object data, TextWriter output)
		{
			var descriptor = new SparkViewDescriptor().AddTemplate(templateName + ".spark");

			var view = (Spark.Web.Mvc.SparkView) _engine.CreateInstance(descriptor);
			try
			{
				view.ViewData = new ViewDataDictionary(data);
				view.RenderView(output);
			}
			finally
			{
				_engine.ReleaseInstance(view);
			}
		}
	}
}