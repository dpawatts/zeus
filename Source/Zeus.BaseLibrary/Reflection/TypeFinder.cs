using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Zeus.BaseLibrary.Reflection
{
	public class TypeFinder : ITypeFinder
	{
		private readonly IAssemblyFinder _assemblyFinder;

		public TypeFinder(IAssemblyFinder assemblyFinder)
		{
			_assemblyFinder = assemblyFinder;
		}

		/// <summary>Finds types assignable from of a certain type in the app domain.</summary>
		/// <param name="requestedType">The type to find.</param>
		/// <returns>A list of types found in the app domain.</returns>
		public IEnumerable<Type> Find(Type requestedType)
		{
			List<Type> types = new List<Type>();
			foreach (Assembly assembly in _assemblyFinder.GetAssemblies())
			{
				try
				{
					foreach (Type type in assembly.GetTypes())
						if (requestedType.IsAssignableFrom(type))
							types.Add(type);
				}
				catch (ReflectionTypeLoadException ex)
				{
					string loaderErrors = string.Empty;
					foreach (Exception loaderEx in ex.LoaderExceptions)
					{
						Trace.TraceError(loaderEx.ToString());
						loaderErrors += ", " + loaderEx.Message;
					}

					throw new Exception(string.Format("Error getting types from assembly " + assembly.FullName + loaderErrors, ex));
				}
			}
			return types;
		}
	}
}