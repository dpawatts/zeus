using System;
using NUnit.Framework;
using Zeus.Tests.Definitions.Items;
using Zeus.ContentTypes;

namespace Zeus.Tests.Definitions
{
	[TestFixture]
	public class DefinitionTests
	{
		private ContentTypeManager _definitionManager;

		[SetUp]
		public void SetUp()
		{
			ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder();
			_definitionManager = new ContentTypeManager(contentTypeBuilder);
		}

		[Test]
		public void CanCreateNewItemInstance()
		{
			TestTextPage item = _definitionManager.CreateInstance<TestTextPage>(null);
			Assert.IsNotNull(item, "Couldn't create item");
		}
	}
}
