using System;
using NUnit.Framework;
using Zeus.Tests.Definitions.Items;
using Zeus.Definitions;

namespace Zeus.Tests.Definitions
{
	[TestFixture]
	public class DefinitionTests
	{
		private DefinitionManager _definitionManager;

		[SetUp]
		public void SetUp()
		{
			_definitionManager = new DefinitionManager();
		}

		[Test]
		public void CanCreateNewItemInstance()
		{
			TestTextPage item = _definitionManager.CreateInstance<TestTextPage>(null);
			Assert.IsNotNull(item, "Couldn't create item");
		}
	}
}
