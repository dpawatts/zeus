using System;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.FileSystem.Images
{
	[AutoInitialize]
	public class ImageCachingServiceInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.AddComponent("zeus.fileSystem.images.imageCachingService", typeof(ImageCachingService));
		}
	}
}