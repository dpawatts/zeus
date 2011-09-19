using Zeus;
using Zeus.ContentTypes;

namespace SoundInTheory.SkiSquirrel.Web.ContentTypes
{
    [ContentType("Hidden Image", "HiddenImage")]
    [AdminSiteTreeVisibility(AdminSiteTreeVisibility.Hidden)]
    public class HiddenImage : Zeus.FileSystem.Images.Image
    {
    }
}
