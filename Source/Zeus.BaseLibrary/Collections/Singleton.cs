using System;
using System.Collections.Generic;

namespace Zeus.BaseLibrary.Collections
{
	/// <summary>
	/// Provides access to all "singletons" stored by <see cref="Singleton{T}"/>.
	/// </summary>
	public class Singleton
	{
		static Singleton()
		{
			allSingletons = new Dictionary<Type, object>();
		}

		static readonly IDictionary<Type, object> allSingletons;

		/// <summary>Dictionary of type to singleton instances.</summary>
		public static IDictionary<Type, object> AllSingletons
		{
			get { return allSingletons; }
		}
	}
}