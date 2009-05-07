using System;
using System.Collections.Generic;
using System.Linq;
using Isis.ComponentModel;
using Zeus.ContentProperties;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	public class ContentTypeBuilder : IContentTypeBuilder
	{
		#region Fields

		private readonly ITypeFinder _typeFinder;
		private readonly IEditableHierarchyBuilder<IEditor> _hierarchyBuilder;
		private readonly AttributeExplorer<IEditor> _editableExplorer;
		private readonly AttributeExplorer<IDisplayer> _displayableExplorer;
		private readonly AttributeExplorer<IContentProperty> _propertyExplorer;
		private readonly AttributeExplorer<IEditorContainer> _containableExplorer;

		#endregion

		#region Constructor

		public ContentTypeBuilder(ITypeFinder typeFinder, IEditableHierarchyBuilder<IEditor> hierarchyBuilder,
			AttributeExplorer<IDisplayer> displayableExplorer, AttributeExplorer<IEditor> editableExplorer,
			AttributeExplorer<IContentProperty> propertyExplorer, AttributeExplorer<IEditorContainer> containableExplorer)
		{
			_typeFinder = typeFinder;
			_hierarchyBuilder = hierarchyBuilder;
			_editableExplorer = editableExplorer;
			_displayableExplorer = displayableExplorer;
			_propertyExplorer = propertyExplorer;
			_containableExplorer = containableExplorer;
		}

		#endregion

		#region Methods

		public IDictionary<Type, ContentType> GetDefinitions()
		{
			IList<ContentType> definitions = FindDefinitions();
			ExecuteRefiners(definitions);
			return definitions.ToDictionary(ct => ct.ItemType);
		}

		private IList<ContentType> FindDefinitions()
		{
			// Find definitions.
			List<ContentType> definitions = new List<ContentType>();
			foreach (Type type in EnumerateTypes())
			{
				ContentType itemDefinition = new ContentType(type);

				itemDefinition.Properties = _propertyExplorer.Find(itemDefinition.ItemType);
				IList<IEditor> tempEditors = _editableExplorer.Find(itemDefinition.ItemType);

				// Get the "distinct" union of actual editors, and default editors for properties
				List<IEditor> editors = new List<IEditor>();
				foreach (IContentProperty property in itemDefinition.Properties)
				{
					IEditor overrideEditor = tempEditors.SingleOrDefault(e => e.Name == property.Name);
					if (overrideEditor != null)
					{
						overrideEditor.Title = property.Title;
						overrideEditor.SortOrder = property.SortOrder;
						editors.Add(overrideEditor);
					}
					else
					{
						IEditor editor = property.GetDefaultEditor();
						if (editor != null)
							editors.Add(editor);
					}
				}

				foreach (IEditor editor in tempEditors.Where(e => !editors.Any(oe => e.Name == oe.Name)))
					editors.Add(editor);

				editors.Sort();
				itemDefinition.Editors = editors;

				itemDefinition.Displayers = _displayableExplorer.Find(itemDefinition.ItemType);
				itemDefinition.Containers = _containableExplorer.Find(itemDefinition.ItemType);

				itemDefinition.RootContainer = _hierarchyBuilder.Build(itemDefinition.Containers, editors);
				definitions.Add(itemDefinition);
			}
			definitions.Sort();

			return definitions;
		}

		protected void ExecuteRefiners(IList<ContentType> definitions)
		{
			foreach (ContentType definition in definitions)
				foreach (IDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IDefinitionRefiner), false))
					refiner.Refine(definition, definitions);
			foreach (ContentType definition in definitions)
				foreach (IInheritableDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IInheritableDefinitionRefiner), true))
					refiner.Refine(definition, definitions);
		}

		private IEnumerable<Type> EnumerateTypes()
		{
			return _typeFinder.Find(typeof (ContentItem)).Where(t => !t.IsAbstract);
		}

		#endregion
	}
}