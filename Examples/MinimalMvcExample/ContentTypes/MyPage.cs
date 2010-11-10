using Zeus;
using Zeus.Web;
using Zeus.Integrity;
using Zeus.Design.Editors;
using System.Collections.Generic;
using Zeus.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("MyPage")]
	[AllowedChildren(typeof(Zeus.FileSystem.Images.Image))]
	[Panel("NewContainer", "All My Types Go in Here!", 100)]
	[FieldSet("ImageContainer", "Main Image", 200, ContainerName = "Content")]
	[FieldSet("AnotherImageContainer", "Secondary Image", 300, ContainerName = "Content")]
	[AdminSiteTreeVisibility(AdminSiteTreeVisibility.Visible | AdminSiteTreeVisibility.ChildrenHidden)]
	public class MyPage : PageContentItem
	{
		[Zeus.Design.Editors.ChildEditor("Image", 100, ContainerName="ImageContainer")]
		public virtual Zeus.FileSystem.Images.Image Image
		{
			get { return GetChild("Image") as Zeus.FileSystem.Images.Image; }
			set
			{
				if (value != null)
				{
					value.Name = "Image";
					value.AddTo(this);
				}
			}
		}

		[Zeus.Design.Editors.ChildEditor("Another Image", 110, ContainerName = "AnotherImageContainer")]
		public virtual Zeus.FileSystem.Images.Image AnotherImage
		{
			get { return GetChild("AnotherImage") as Zeus.FileSystem.Images.Image; }
			set
			{
				if (value != null)
				{
					value.Name = "AnotherImage";
					value.AddTo(this);
				}
			}
		}

        [ContentProperty("Other Images", 200, EditorContainerName = "Content")]
        [ChildrenEditor("Other Images", 200, TypeFilter = typeof(Zeus.FileSystem.Images.Image), ContainerName = "Content")]
        public IEnumerable<Zeus.FileSystem.Images.Image> Images
        {
            get { return GetChildren<Zeus.FileSystem.Images.Image>(); }
        }

		[ChildrenEditor("Test Child Editors", 15, TypeFilter = typeof(MyLittleType), ContainerName = "NewContainer")]
		public virtual IEnumerable<MyLittleType> ListFilters
		{
			get { return GetChildren<MyLittleType>(); }
		}

	}
}
