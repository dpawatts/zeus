using System;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ImageUploadEditorAttribute : FileUploadEditorAttribute
	{
		public int? MinimumWidth { get; set; }
		public int? MinimumHeight { get; set; }

		/// <summary>Initializes a new instance of the ImageUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public ImageUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		protected override FancyFileUpload CreateEditor()
		{
			return new FancyImageUpload { MinimumWidth = MinimumWidth, MinimumHeight = MinimumHeight };
		}
	}
}