using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class TypeExtensionMethods
	{
		public static IEnumerable<Type> EnumerateParentTypes(this Type type)
		{
			while (type != typeof(object))
			{
				yield return type;
				type = type.BaseType;
			}
		}

		public static MethodInfo GetGenericMethod(this Type type, string name, Type[] typeArgs,
			Type[] argTypes, BindingFlags flags)
		{
			int typeArity = typeArgs.Length;
			var methods = type.GetMethods()
				.Where(m => m.Name == name)
				.Where(m => m.GetGenericArguments().Length == typeArity)
				.Select(m => m.MakeGenericMethod(typeArgs));

			return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null) as MethodInfo;
		}

		public static bool IsNullable(this Type type)
		{
			return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}

		/// <summary>
		/// If the type is nullable, returns the underlying type, otherwise returns
		/// the original type.
		/// </summary>
		/// <returns></returns>
		public static Type GetTypeOrUnderlyingType(this Type type)
		{
			return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
		}

		public static string GetTypeAndAssemblyName(this Type type)
		{
			return string.Format("{0},{1}", type.AssemblyQualifiedName.Split(','));
		}
	}
}