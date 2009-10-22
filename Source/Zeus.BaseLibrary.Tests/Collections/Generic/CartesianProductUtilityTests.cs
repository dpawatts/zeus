using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.BaseLibrary.Tests.Collections.Generic
{
	[TestClass]
	public class CartesianProductUtilityTests
	{
		[TestMethod]
		public void CanCombine2By2Groups()
		{
			IEnumerable<char> group1 = new[] { 'a', 'b' }, group2 = new[] { 'c', 'd' };
			IEnumerable<IEnumerable<char>> cartesianProduct = CartesianProductUtility.Combinations(group1, group2);
			Assert.AreEqual(4, cartesianProduct.Count());
			CollectionAssert.AreEqual(new[] { 'a', 'c' }, cartesianProduct.ElementAt(0).ToList());
			CollectionAssert.AreEqual(new[] { 'a', 'd' }, cartesianProduct.ElementAt(1).ToList());
			CollectionAssert.AreEqual(new[] { 'b', 'c' }, cartesianProduct.ElementAt(2).ToList());
			CollectionAssert.AreEqual(new[] { 'b', 'd' }, cartesianProduct.ElementAt(3).ToList());
		}

		[TestMethod]
		public void CanCombine2By3Groups()
		{
			IEnumerable<char> group1 = new[] { 'a', 'b', 'c' }, group2 = new[] { 'd', 'e', 'f' };
			IEnumerable<IEnumerable<char>> cartesianProduct = CartesianProductUtility.Combinations(group1, group2);
			Assert.AreEqual(9, cartesianProduct.Count());
			CollectionAssert.AreEqual(new[] { 'a', 'd' }, cartesianProduct.ElementAt(0).ToList());
			CollectionAssert.AreEqual(new[] { 'a', 'e' }, cartesianProduct.ElementAt(1).ToList());
			CollectionAssert.AreEqual(new[] { 'a', 'f' }, cartesianProduct.ElementAt(2).ToList());
			CollectionAssert.AreEqual(new[] { 'b', 'd' }, cartesianProduct.ElementAt(3).ToList());
			CollectionAssert.AreEqual(new[] { 'b', 'e' }, cartesianProduct.ElementAt(4).ToList());
			CollectionAssert.AreEqual(new[] { 'b', 'f' }, cartesianProduct.ElementAt(5).ToList());
			CollectionAssert.AreEqual(new[] { 'c', 'd' }, cartesianProduct.ElementAt(6).ToList());
			CollectionAssert.AreEqual(new[] { 'c', 'e' }, cartesianProduct.ElementAt(7).ToList());
			CollectionAssert.AreEqual(new[] { 'c', 'f' }, cartesianProduct.ElementAt(8).ToList());
		}
	}
}