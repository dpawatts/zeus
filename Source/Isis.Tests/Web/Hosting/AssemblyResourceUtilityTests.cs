using Isis.Web.Hosting;
using MbUnit.Framework;

namespace Isis.Tests.Web.Hosting
{
	[TestFixture]
	public class AssemblyResourceUtilityTests
	{
		[Test]
		public void CanGetUrl()
		{
			string url = EmbeddedResourceUtility.GetUrl(typeof(AssemblyResourceUtilityTests).Assembly, "This.Is.My.Resource.aspx");
			Assert.AreEqual("/myPrefix/App_Resources/Isis.Tests.dll/This.Is.My.Resource.aspx", url);
		}
	}
}