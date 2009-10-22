using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.BaseLibrary.Web;

namespace Zeus.BaseLibrary.Tests.Web
{
	[TestClass]
	public class UrlTests
	{
		[TestMethod]
		public void CanParse_Url()
		{
			Url url = new Url("http://localhost/my/path/to/the/file.aspx?query=nothing#fragment");
			Assert.AreEqual("http", url.Scheme);
			Assert.AreEqual("localhost", url.Authority);
			Assert.AreEqual("/my/path/to/the/file.aspx", url.Path);
			Assert.AreEqual("query=nothing", url.Querystring);
			Assert.AreEqual("fragment", url.Fragment);
			Assert.AreEqual(".aspx", url.Extension);
			Assert.AreEqual("/my/path/to/the/file.aspx?query=nothing", url.PathAndQuery);
		}
	}
}