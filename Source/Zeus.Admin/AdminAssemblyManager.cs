using System.Reflection;

namespace Zeus.Admin
{
	public class AdminAssemblyManager : IAdminAssemblyManager
	{
		public Assembly Assembly
		{
			get { return typeof(AdminAssemblyManager).Assembly; }
		}
	}
}
