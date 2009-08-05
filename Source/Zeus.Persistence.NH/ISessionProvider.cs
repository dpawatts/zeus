using System;

namespace Zeus.Persistence.NH
{
	public interface ISessionProvider : IDisposable
	{
		SessionContext OpenSession { get; }
	}
}
