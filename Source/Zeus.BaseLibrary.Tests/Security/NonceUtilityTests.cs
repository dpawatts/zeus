using NUnit.Framework;
using Zeus.BaseLibrary.Security;

namespace Zeus.BaseLibrary.Tests.Security
{
	[TestFixture]
	public class NonceUtilityTests
	{
		[Test]
		public void CanGenerateNonce()
		{
			// Act.
			string result = NonceUtility.GenerateNonce();

			// Assert.
			Assert.IsTrue(!string.IsNullOrEmpty(result));
		}
	}
}