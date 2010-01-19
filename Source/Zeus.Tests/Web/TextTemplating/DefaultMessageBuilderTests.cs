using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.Web.TextTemplating;

namespace Zeus.Tests.Web.TextTemplating
{
	[TestClass]
	public class DefaultMessageBuilderTests
	{
		[TestMethod]
		public void CanBuildMessageFromEmbeddedTemplate()
		{
			DefaultMessageBuilder messageBuilder = new DefaultMessageBuilder();
			string result = messageBuilder.Transform("TestTransform", new { message = "Hello World." });
			Assert.AreEqual("This is a test transform. Hello World.", result);
		}
	}
}