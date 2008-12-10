using System;
using System.Web.UI;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class EditorContainerAttribute : Attribute, IEditorContainer
	{
		private List<IContainable> _contained = new List<IContainable>();

		public EditorContainerAttribute(string name, int sortOrder)
		{
			this.Name = name;
			this.SortOrder = sortOrder;
		}

		public string Name
		{
			get;
			set;
		}

		public int SortOrder
		{
			get;
			set;
		}

		public string ContainerName
		{
			get;
			set;
		}

		public abstract Control AddTo(Control container);

		public IList<IContainable> GetContained()
		{
			return _contained;
		}

		public void AddContained(IContainable containable)
		{
			_contained.Add(containable);
		}
	}
}
