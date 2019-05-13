using System.IO;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.Design.Editors;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using Zeus.ContentTypes;
using System;

namespace Zeus.FileSystem.Images
{
    [ContentType("Hidden User Cropped Image")]
    [AdminSiteTreeVisibility(AdminSiteTreeVisibility.Hidden)]
    [ContentTypeAuthorizedRoles("Administrators")]    
    public class HiddenCroppedImage : CroppedImage
    {
        public override string arg1
        {
            get { return GetDetail("arg1", string.Empty); }
            set { SetDetail("arg1", value); }
        }

        public override string arg2
        {
            get { return GetDetail("arg2", string.Empty); }
            set { SetDetail("arg2", value); }
        }
    }
}