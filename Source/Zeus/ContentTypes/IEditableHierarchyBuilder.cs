using System.Collections.Generic;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	public interface IEditableHierarchyBuilder<T>
		where T : IContainable
	{
		IEditorContainer Build(IEnumerable<IEditorContainer> containers, IEnumerable<T> editables);
	}
}