using Zeus;
using Zeus.ContentTypes;

namespace Zeus.FileSystem.Images
{
    [ContentType("Hidden Image", "HiddenImage")]
    [AdminSiteTreeVisibility(AdminSiteTreeVisibility.Hidden)]
    [ContentTypeAuthorizedRoles("Administrators")]    
    public class HiddenImage : Zeus.FileSystem.Images.Image
    {
        public override string IconUrl
        {
            get
            {
                return Utility.GetCooliteIconUrl(Ext.Net.Icon.PictureEdit);
            }
        }
    }
}
