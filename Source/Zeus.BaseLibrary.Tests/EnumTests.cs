using NUnit.Framework;

namespace Zeus.BaseLibrary.Tests
{
	[TestFixture]
	public class EnumTests
	{
		[Test]
		public void CanGetDescription()
		{
			Assert.AreEqual("First Value", EnumHelper.GetEnumValueDescription(typeof(MyEnum), "FirstValue"));
			Assert.AreEqual("2nd Value", EnumHelper.GetEnumValueDescription(typeof(MyEnum), "SecondValue"));
			Assert.AreEqual("ThirdValue", EnumHelper.GetEnumValueDescription(typeof(MyEnum), "ThirdValue"));
		}

		private enum MyEnum
		{
			[System.ComponentModel.Description("First Value")]
			FirstValue,

			[System.ComponentModel.Description("2nd Value")]
			SecondValue,

			ThirdValue,
		}
	}
}