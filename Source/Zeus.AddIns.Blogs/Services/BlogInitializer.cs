using System;
using Ninject;
using Zeus.Engine;

namespace Zeus.AddIns.Blogs.Services
{
	public class BlogInitializer : IStartable
	{
		private readonly EventBroker _eventBroker;

		public BlogInitializer(EventBroker eventBroker)
		{
			_eventBroker = eventBroker;
		}

		public void Start()
		{
			//_eventBroker.b
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}