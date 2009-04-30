using System;

namespace Zeus.Plugin
{
	public class PluginInitializationException : ZeusException
	{
		public PluginInitializationException(string message, Exception[] innerExceptions)
			: base(message, innerExceptions[0])
		{
			InnerExceptions = innerExceptions;
		}

		public Exception[] InnerExceptions { get; set; }
	}
}