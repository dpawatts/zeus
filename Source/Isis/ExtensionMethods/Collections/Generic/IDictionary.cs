using System;
using System.Collections.Generic;

namespace Isis.ExtensionMethods.Collections.Generic
{
	public static class IDictionaryExtensionMethods
	{
		public static TActualValue GetOrAdd<TKey, TDictionaryValue, TActualValue>(
			this IDictionary<TKey, TDictionaryValue> dictionary, TKey key, Func<TActualValue> newValue)
			where TActualValue : TDictionaryValue
		{
			TDictionaryValue value;
			if (!dictionary.TryGetValue(key, out value))
			{
				value = newValue.Invoke();
				dictionary.Add(key, value);
			}
			return (TActualValue)value;
		}
	}
}