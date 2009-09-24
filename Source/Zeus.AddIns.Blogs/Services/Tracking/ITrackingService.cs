using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	public interface ITrackingService
	{
		void SendNotifications(Blog blog, Post post);
	}
}