using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.BaseLibrary.Security;

namespace Zeus.BaseLibrary.Tests.Security
{
	[TestClass]
	public class NonceUtilityTests
	{
		[TestMethod]
		public void CanGenerateNonce()
		{
			// Act.
			string result = NonceUtility.GenerateNonce();

			// Assert.
			Assert.IsTrue(!string.IsNullOrEmpty(result));
		}
	}
}