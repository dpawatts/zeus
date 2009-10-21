using System.Linq;
using Rhino.Mocks;
using Zeus.Persistence;
using Zeus.Tests.Definitions.Items;

namespace Zeus.Tests.Persistence.Specifications
{
	/*[TestFixture]
	public class SpecificationTests
	{
		[Test]
		public void Can_Specify_Name()
		{
			TestItemFinder finder = new TestItemFinder();
			var results = finder.FindBySpecification(new Specification<TestTextPage>(p => p.Name.StartsWith("test")));
			Assert.GreaterThan(results.Count(), 1);
		}
	}

	public class TestItemFinder : IItemFinder
	{
		public System.Linq.IQueryable<T> FindBySpecification<T>(Zeus.Persistence.Specifications.ISpecification<T> specification) where T : ContentItem
		{
			TestTextPage[] items = new TestTextPage[100];
			for (int i = 0; i < 100; i++)
				items[i] = new TestTextPage {Name = "test" + i};
			return items.Cast<T>().AsQueryable();
		}
	}*/
}
