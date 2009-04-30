using System;
using System.Collections.Generic;
using System.Security.Principal;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	public interface ITypeDefinition
	{
		Type ItemType { get; }
		IEditorContainer RootContainer { get; }

		IEnumerable<IDisplayer> Displayers { get; }
		IList<IEditor> GetEditors(IPrincipal user);
		string Title { get; }
	}
}