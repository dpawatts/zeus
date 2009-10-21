using System.Linq;
using Isis.ExtensionMethods.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods.Linq
{
	[TestClass]
	public class QueryableTests
	{
		[TestMethod]
		public void CanFilterByType()
		{
			IQueryable myArray = new object[]
			{
				1,
				"myString",
				3.0,
				4.0f,
				"anotherString"
			}.AsQueryable();

			var filteredArray = myArray.OfType(typeof(string));
			Assert.AreEqual(2, filteredArray.Cast<object>().Count());
		}
	}
}