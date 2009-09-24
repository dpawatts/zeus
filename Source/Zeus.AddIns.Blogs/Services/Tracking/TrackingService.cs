using System.Threading;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	public class TrackingService : ITrackingService
	{
		private readonly IPingbackService _pingbackService;
		private readonly IWebContext _webContext;

		public TrackingService(IPingbackService pingbackService, IWebContext webContext)
		{
			_pingbackService = pingbackService;
			_webContext = webContext;
		}

		public void SendNotifications(Blog blog, Post post)
		{
			// Setup notifier object.
			Notifier notifier = new Notifier
			{
				PingbackService = _pingbackService,
				FullyQualifiedUrl = _webContext.GetFullyQualifiedUrl(blog.Url),
				BlogName = blog.Title,
				Title = post.Title,
				PostUrl = _webContext.GetFullyQualifiedUrl(post.Url),
				Description = post.GetExcerpt(),
				Text = post.Text
			};

			// This could take a while, so do it on another thread.
			ThreadPool.QueueUserWorkItem(notifier.Notify);
		}
	}
}