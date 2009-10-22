using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Persistence;
using Zeus.Persistence.NH;
using Zeus.ContentTypes;
using Zeus.Web;
using System.Configuration;
using Zeus.Configuration;

namespace Zeus.Tests.Persistence.NH
{
	[TestClass]
	public class ItemFinderTests
	{
		[TestMethod]
		public void Can_Query_Detail()
		{
			IAssemblyFinder assemblyFinder = new AssemblyFinder();
			ITypeFinder typeFinder = new TypeFinder(assemblyFinder);
			IContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(typeFinder, null, null, null, null, null);
			IItemNotifier itemNotifier = new ItemNotifier();
			IContentTypeManager contentTypeManager = new ContentTypeManager(contentTypeBuilder, itemNotifier);
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder(contentTypeManager, ConfigurationManager.GetSection("zeus/database") as DatabaseSection);
			ISessionProvider sessionProvider = new SessionProvider(configurationBuilder, new NotifyingInterceptor(new ItemNotifier()), new ThreadContext());

			/*var results = itemFinder.Find<NewsContainer>(
				//ci => ci.Details.Values.OfType<StringDetail>().Any(cd => cd.Name == "Text" && cd.StringValue == "sdfds")
				//ci => ci.Details.OfType<StringDetail>().Where(cd => cd.Name == "DetailName").SingleOrDefault().StringValue == "Hello"
				ci => ci.Text == "sdfds" //&& ci.MyProperty.StartsWith("Hell")
			);
			Assert.GreaterThanOrEqualTo(Enumerable.Count(results), 1);*/
		}
	}
}
