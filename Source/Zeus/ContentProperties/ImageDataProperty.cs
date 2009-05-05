using System;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;

namespace Zeus.ContentProperties
{
	[PropertyDataType(typeof(ImageData))]
	public class ImageDataProperty : BaseFileDataProperty<ImageData>
	{
		#region Constuctors

		public ImageDataProperty()
		{
		}

		public ImageDataProperty(ContentItem containerItem, string name, ImageData value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion

		#region Methods

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new ImageDataUploadEditorAttribute(title, sortOrder);
		}

		#endregion
	}
}