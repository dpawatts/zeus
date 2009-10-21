using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeus.Web.TextTemplating.Tests
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