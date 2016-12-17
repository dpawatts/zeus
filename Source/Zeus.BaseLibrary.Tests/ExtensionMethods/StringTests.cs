using System.Diagnostics;
using System.Globalization;
using NUnit.Framework;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods
{
	[TestFixture]
	public class StringTests
	{
		[Test]
		public void Test_Left_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(6);
			Assert.AreEqual("This i", leftPart);
		}

		[Test]
		public void Test_Left_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(300);
			Assert.AreEqual("This is a test string.", leftPart);
		}

		[Test]
		public void Test_Left_EmptyString()
		{
			const string myString = "";
			string leftPart = myString.Left(6);
			Assert.AreEqual("", leftPart);
		}

		[Test]
		public void Test_Right_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string rightPart = myString.Right(6);
			Assert.AreEqual("tring.", rightPart);
		}

		[Test]
		public void Test_Right_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string rightPart = myString.Right(300);
			Assert.AreEqual("This is a test string.", rightPart);
		}

		[Test]
		public void Test_ToPascalCase()
		{
			const string myString = "ThisIsAnIdentifier";
			string result = myString.ToPascalCase();
			Assert.AreEqual("thisIsAnIdentifier", result);
		}

		[Test]
		public void Test_Truncate_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Truncate(6);
			Assert.AreEqual("Thi...", leftPart);
		}

		[Test]
		public void Test_Truncate_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Truncate(300);
			Assert.AreEqual("This is a test string.", leftPart);
		}

		[Test]
		public void CanGetLeftBefore()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.LeftBefore("string");

			// Assert.
			Assert.AreEqual("This is my ", result);
		}

		[Test]
		public void CanGetRightAfter()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.RightAfter("my");

			// Assert.
			Assert.AreEqual(" string.", result);
		}

		[Test]
		public void CanGetRightAfterLast()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.RightAfterLast("i");

			// Assert.
			Assert.AreEqual("ng.", result);
		}

		[Test]
		public void CanUrlEncodeString()
		{
			Assert.AreEqual("my-safe_url-is-great", " My  SAFE_Url is-great".ToSafeUrl());
		}

		[Test]
		public void CanUrlEncodeAccentedString()
		{
			Assert.AreEqual("%c3%a1cc%c3%a8nt", "áccènt".ToSafeUrl());
		}
	}
}