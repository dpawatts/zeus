using System.IO;
using System.Web.Hosting;
using Isis.Web.Hosting;
using MbUnit.Framework;

namespace Isis.Tests.Web.Hosting
{
	[TestFixture]
	public class AssemblyResourceVirtualFileTests
	{
		[Test]
		public void CanOpenStream()
		{
			VirtualFile virtualFile = new EmbeddedResourceVirtualFile("~/This.Is.My.Resource.aspx", GetType().Assembly, "Isis.Tests.This.Is.My.Resource.aspx");
			Stream stream = virtualFile.Open();
			Assert.GreaterThan(stream.Length, 0);
			stream.Close();
		}
	}
}