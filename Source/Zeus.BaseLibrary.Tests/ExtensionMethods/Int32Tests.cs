using Isis.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods
{
	[TestClass]
	public class Int32Tests
	{
		[TestMethod]
		public void ToOrdinal_1()
		{
			Assert.AreEqual("1st", 1.ToOrdinal());
		}

		[TestMethod]
		public void ToOrdinal_2()
		{
			Assert.AreEqual("2nd", 2.ToOrdinal());
		}

		[TestMethod]
		public void ToOrdinal_3()
		{
			Assert.AreEqual("3rd", 3.ToOrdinal());
		}

		[TestMethod]
		public void ToOrdinal_4()
		{
			Assert.AreEqual("4th", 4.ToOrdinal());
		}
	}
}