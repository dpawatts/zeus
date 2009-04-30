using System.Web;
using Zeus.Engine;

namespace Zeus.Web
{
	/// <summary>
	/// A HttpModule that ensures that the Zeus engine is initialized with a web 
	/// context.
	/// </summary>
	public class InitializerModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			EventBroker.Instance.Attach(context);
			Context.Initialize(false);
		}

		public void Dispose()
		{
		}
	}
}
