using NUnit.Framework;
using Zeus.Web.TextTemplating;

namespace Zeus.Tests.Web.TextTemplating
{
	[TestFixture]
	public class DefaultMessageBuilderTests
	{
		[Test]
		public void CanBuildMessageFromEmbeddedTemplate()
		{
			DefaultMessageBuilder messageBuilder = new DefaultMessageBuilder();
            messageBuilder.Initialize(GetType().Assembly, "Zeus.Tests.Web.TextTemplating.Templates");
			string result = messageBuilder.Transform("TestTransform", new { message = "Hello World." });
			Assert.AreEqual("This is a test transform. Hello World.", result);
		}
	}
}