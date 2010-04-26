using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods
{
	[TestClass]
	public class StringTests
	{
		[TestMethod]
		public void Test_Left_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(6);
			Assert.AreEqual("This i", leftPart);
		}

		[TestMethod]
		public void Test_Left_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(300);
			Assert.AreEqual("This is a test string.", leftPart);
		}

		[TestMethod]
		public void Test_Left_EmptyString()
		{
			const string myString = "";
			string leftPart = myString.Left(6);
			Assert.AreEqual("", leftPart);
		}

		[TestMethod]
		public void Test_Right_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string rightPart = myString.Right(6);
			Assert.AreEqual("tring.", rightPart);
		}

		[TestMethod]
		public void Test_Right_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string rightPart = myString.Right(300);
			Assert.AreEqual("This is a test string.", rightPart);
		}

		[TestMethod]
		public void Test_ToPascalCase()
		{
			const string myString = "ThisIsAnIdentifier";
			string result = myString.ToPascalCase();
			Assert.AreEqual("thisIsAnIdentifier", result);
		}

		[TestMethod]
		public void Test_Truncate_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Truncate(6);
			Assert.AreEqual("Thi...", leftPart);
		}

		[TestMethod]
		public void Test_Truncate_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Truncate(300);
			Assert.AreEqual("This is a test string.", leftPart);
		}

		[TestMethod]
		public void CanGetLeftBefore()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.LeftBefore("string");

			// Assert.
			Assert.AreEqual("This is my ", result);
		}

		[TestMethod]
		public void CanGetRightAfter()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.RightAfter("my");

			// Assert.
			Assert.AreEqual(" string.", result);
		}

		[TestMethod]
		public void CanGetRightAfterLast()
		{
			// Arrange.
			const string value = "This is my string.";

			// Act.
			string result = value.RightAfterLast("i");

			// Assert.
			Assert.AreEqual("ng.", result);
		}

		[TestMethod]
		public void CanUrlEncodeString()
		{
			Assert.AreEqual("my-safe_url-is-great", " My  SAFE_Url is-great".ToSafeUrl());
		}

		[TestMethod]
		public void CanUrlEncodeAccentedString()
		{
			Assert.AreEqual("%c3%a1cc%c3%a8nt", "áccènt".ToSafeUrl());
		}
	}
}