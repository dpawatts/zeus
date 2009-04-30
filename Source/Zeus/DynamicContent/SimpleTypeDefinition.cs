using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.ContentTypes;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.DynamicContent
{
	public class SimpleTypeDefinition : ITypeDefinition
	{
		private readonly object _objectToWrap;
		private readonly IEnumerable<IEditor> _editors;

		public SimpleTypeDefinition(IEditableHierarchyBuilder<IEditor> hierarchyBuilder,
			AttributeExplorer<IEditorContainer> containerExplorer,
			AttributeExplorer<IEditor> editorExplorer,
			object objectToWrap)
		{
			_objectToWrap = objectToWrap;

			IEnumerable<Property> properties = _objectToWrap.GetType().GetProperties().Select(pi => new Property(pi));
			IEnumerable<IEditorContainer> containers = containerExplorer.Find(objectToWrap.GetType());

			_editors = editorExplorer.Find(objectToWrap.GetType());

			RootContainer = hierarchyBuilder.Build(containers, _editors);
		}

		public Type ItemType
		{
			get { return _objectToWrap.GetType(); }
		}

		public IEditorContainer RootContainer { get; private set; }

		public IEnumerable<IDisplayer> Displayers
		{
			get { throw new NotSupportedException(); }
		}

		public IList<IEditor> GetEditors(IPrincipal user)
		{
			return _editors.Where(e => e.IsAuthorized(user)).ToList();
		}

		public string Title
		{
			get { return _objectToWrap.GetType().Name; }
		}
	}
}