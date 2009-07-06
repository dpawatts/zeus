using System;
using Zeus.Design.Editors;
using Zeus.FileSystem;

namespace Zeus.ContentProperties
{
	[PropertyDataType(typeof(FileData))]
	public class FileDataProperty : BaseFileDataProperty<FileData>
	{
		#region Constuctors

		public FileDataProperty()
		{
		}

		public FileDataProperty(ContentItem containerItem, string name, FileData value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion

		#region Methods

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType, string containerName)
		{
			return new FileDataUploadEditorAttribute(title, sortOrder) { ContainerName = containerName };
		}

		#endregion
	}
}