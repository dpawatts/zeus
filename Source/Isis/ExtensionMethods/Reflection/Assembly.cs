using System;
using System.Reflection;
using System.IO;

namespace Isis.ExtensionMethods.Reflection
{
	public static class AssemblyExtensionMethods
	{
		public static string GetStringResource(this Assembly assembly, string resourceName)
		{
			TextReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourceName));
			string result = textReader.ReadToEnd();
			textReader.Close();

			return result;
		}
	}
}