using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Zeus.ContentTypes
{
	public interface IEditorContainer : IContainable, IComparable<IEditorContainer>
	{
		IList<IContainable> GetContained(IPrincipal user);
		void AddContained(IContainable containable);
		List<IContainable> Contained { get; }
	}
}