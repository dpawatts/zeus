using Isis.ExtensionMethods;
using MbUnit.Framework;

namespace Isis.Tests.ExtensionMethods
{
	[TestFixture]
	public class Int32Tests
	{
		[Test]
		public void ToOrdinal_1()
		{
			Assert.AreEqual("1st", 1.ToOrdinal());
		}

		[Test]
		public void ToOrdinal_2()
		{
			Assert.AreEqual("2nd", 2.ToOrdinal());
		}

		[Test]
		public void ToOrdinal_3()
		{
			Assert.AreEqual("3rd", 3.ToOrdinal());
		}

		[Test]
		public void ToOrdinal_4()
		{
			Assert.AreEqual("4th", 4.ToOrdinal());
		}
	}
}
