using Zeus.BaseLibrary.DependencyInjection;

namespace Zeus.Web.Security
{
	public static class WebSecurityEngine
	{
		public static DependencyInjectionManager DependencyInjectionManager { get; set; }

		public static TService Get<TService>()
		{
			return DependencyInjectionManager.Get<TService>();
		}
	}
}