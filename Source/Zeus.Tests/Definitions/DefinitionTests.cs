using NUnit.Framework;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Persistence;
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
			IAssemblyFinder assemblyFinder = new AssemblyFinder();
			ITypeFinder typeFinder = new TypeFinder(assemblyFinder);
			ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(
                typeFinder,
                new EditableHierarchyBuilder<Design.Editors.IEditor>(),
                new AttributeExplorer<Design.Displayers.IDisplayer>(),
                new AttributeExplorer<Design.Editors.IEditor>(),
                new AttributeExplorer<ContentProperties.IContentProperty>(),
                new AttributeExplorer<IEditorContainer>()
             );
			_definitionManager = new ContentTypeManager(contentTypeBuilder, new ItemNotifier());
		}

		[Test]
		public void CanCreateNewItemInstance()
		{
			TestTextPage item = _definitionManager.CreateInstance<TestTextPage>(null);
			Assert.IsNotNull(item, "Couldn't create item");
		}
	}
}
