using System.Collections.Generic;
using System.Security.Principal;
using Zeus.ObjectEditor.Editors;

namespace Zeus.ObjectEditor
{
	public interface IEditableObjectDescriptor
	{
		IEnumerable<IEditor> GetEditors(IPrincipal user);
	}
}