using System;
using Isis.ComponentModel;

namespace Zeus.Persistence.NH
{
	public interface ISessionProvider : IService, IDisposable
	{
		SessionContext OpenSession { get; }
	}
}
