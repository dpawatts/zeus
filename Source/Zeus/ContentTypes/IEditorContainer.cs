using System;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	public interface IEditorContainer : IContainable
	{
		IList<IContainable> GetContained();
		void AddContained(IContainable containable);
	}
}
