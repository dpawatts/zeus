using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SoundInTheory.DynamicImage;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Zeus.Admin.App_Start.DynamicImage), "PreStart")]

namespace Zeus.Admin.App_Start
{
	public static class DynamicImage
	{
		public static void PreStart()
		{
			DynamicModuleUtility.RegisterModule(typeof(DynamicImageModule));
		}
	}
}