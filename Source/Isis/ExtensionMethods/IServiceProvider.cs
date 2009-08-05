using System;

namespace Isis.ExtensionMethods
{
	public static class IServiceProviderExtensionMethods
	{
		public static TService GetService<TService>(this IServiceProvider serviceProvider)
		{
			return (TService) serviceProvider.GetService(typeof(TService));
		}
	}
}