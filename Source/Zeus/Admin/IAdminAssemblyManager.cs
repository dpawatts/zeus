using System.Reflection;
using Isis.ComponentModel;

namespace Zeus.Admin
{
	public interface IAdminAssemblyManager : IService
	{
		Assembly Assembly { get; }
	}
}
