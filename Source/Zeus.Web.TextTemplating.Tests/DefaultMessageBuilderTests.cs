using MbUnit.Framework;

namespace Zeus.Web.TextTemplating.Tests
{
	[TestFixture]
	public class DefaultMessageBuilderTests
	{
		[Test]
		public void CanBuildMessageFromEmbeddedTemplate()
		{
			DefaultMessageBuilder messageBuilder = new DefaultMessageBuilder();
			string result = messageBuilder.Transform("TestTransform", new { message = "Hello World." });
			Assert.AreEqual("This is a test transform. Hello World.", result);
		}
	}
}