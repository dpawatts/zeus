using System;
using System.Collections.Generic;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DefaultContainerAttribute : Attribute, IInheritableDefinitionRefiner
	{
		public DefaultContainerAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			IEditableHierarchyBuilder<IEditor> hierarchyBuilder = Context.Current.Resolve<IEditableHierarchyBuilder<IEditor>>();
			bool updated = false;
			foreach (IEditor editor in currentDefinition.Editors)
				if (string.IsNullOrEmpty(editor.ContainerName))
				{
					editor.ContainerName = Name;
					updated = true;
				}
			if (updated)
				currentDefinition.RootContainer = hierarchyBuilder.Build(currentDefinition.Containers, currentDefinition.Editors);
		}
	}
}