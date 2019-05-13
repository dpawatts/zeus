using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Zeus.BaseLibrary.Linq;

namespace Zeus.BaseLibrary.Tests.Linq
{
	[TestFixture]
	public class ExtendedEnumerableTests
	{
		[Test]
		public void RangeDescendingTest()
		{
			int start = 2009;
			int count = 5;
			ICollection expected = new[] { 2009, 2008, 2007, 2006, 2005 };
			ICollection actual = ExtendedEnumerable.RangeDescending(start, count).ToList();
			CollectionAssert.AreEqual(expected, actual);
		}
	}
}