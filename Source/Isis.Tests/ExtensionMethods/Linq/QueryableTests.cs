using System.Linq;
using Isis.ExtensionMethods.Linq;
using MbUnit.Framework;

namespace Isis.Tests.ExtensionMethods.Linq
{
	[TestFixture]
	public class QueryableTests
	{
		[Test]
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
