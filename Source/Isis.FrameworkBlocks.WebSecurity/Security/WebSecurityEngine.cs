using Isis.Collections;
using Isis.FrameworkBlocks.DependencyInjection;
using Isis.Reflection;

namespace Isis.Web.Security
{
	public class WebSecurityEngine
	{
		private readonly DependencyInjectionManager _dependencyInjectionManager;

		public WebSecurityEngine()
		{
			_dependencyInjectionManager = new DependencyInjectionManager();
			_dependencyInjectionManager.Bind<IAssemblyFinder, AssemblyFinder>();
			_dependencyInjectionManager.Bind<ITypeFinder, TypeFinder>();
		}

		public static WebSecurityEngine Current
		{
			get
			{
				if (Singleton<WebSecurityEngine>.Instance == null)
					Singleton<WebSecurityEngine>.Instance = new WebSecurityEngine();
				return Singleton<WebSecurityEngine>.Instance;
			}
		}

		public TService Get<TService>()
		{
			return _dependencyInjectionManager.Get<TService>();
		}
	}
}