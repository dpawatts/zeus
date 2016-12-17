using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Zeus.BaseLibrary.ExtensionMethods.Linq;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods.Linq
{
	[TestFixture]
	public class EnumerableTests
	{
		[Test]
		public void AlternateTest()
		{
			var source = new[] { "The", "quick", "brown", "fox" };
			string result = source.Alternate(Spaces()).Aggregate(string.Empty, (a, b) => a + b);

			Assert.AreEqual("The quick brown fox ", result);
		}

		private static IEnumerable<string> Spaces()
		{
			while (true)
				yield return " ";
		}

		[Test]
		public void AppendTest()
		{
			var ints = new[] { 1, 2, 3 };
			var oneToFour = ints.Append(4);
			CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, oneToFour.ToArray());
		}

		[Test]
		public void PrependTest()
		{
			var ints = new[] { 1, 2, 3 };
			var zeroToThree = ints.Prepend(0);
			CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, zeroToThree.ToArray());
		}
	}
}