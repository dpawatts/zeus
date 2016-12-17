using System.Drawing;
using System.IO;
using System.Net;
using NUnit.Framework;
using Zeus.Templates.Mvc.Html;

namespace Zeus.Templates.Tests.Mvc.Html
{
	[TestFixture]
	public class GravatarExtensionsTests
	{
		[Test]
		public void CanGenerateGravatarImageUrl()
		{
			// Arrange.
			WebClient webClient = new WebClient();

			// Act.
			string imageUrl = GravatarExtensions.GravatarImageUrl(null, 50, "test@sitdap.com");
			byte[] imageData = webClient.DownloadData(imageUrl);
			Bitmap image = new Bitmap(new MemoryStream(imageData));

			// Assert.
			Assert.IsNotNull(imageUrl);
			Assert.IsTrue(imageUrl.Length > 0);
			Assert.IsNotNull(image);
			Assert.AreEqual(50, image.Width);
			Assert.AreEqual(50, image.Height);

			// Clean up.
			image.Dispose();
		}
	}
}