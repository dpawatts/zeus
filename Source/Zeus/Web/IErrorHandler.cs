using System;

namespace Zeus.Web
{
	public interface IErrorHandler
	{
		void Notify(Exception ex);
	}
}