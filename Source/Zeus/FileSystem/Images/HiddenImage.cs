using Zeus;
using Zeus.ContentTypes;

namespace Zeus.FileSystem.Images
{
    [ContentType("Hidden Image", "HiddenImage")]
    [AdminSiteTreeVisibility(AdminSiteTreeVisibility.Hidden)]
    public class HiddenImage : Zeus.FileSystem.Images.Image
    {
    }
}
