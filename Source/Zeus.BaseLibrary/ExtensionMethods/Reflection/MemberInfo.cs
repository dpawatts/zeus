using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Isis;

namespace Isis.ExtensionMethods.Reflection
{
	public static class MemberInfoExtensionMethods
	{
		public static T GetCustomAttribute<T>(this MemberInfo memberInfo, bool inherit, bool throwOnMissing)
		{
			return GetCustomAttributes<T>(memberInfo, inherit, throwOnMissing).FirstOrDefault();
		}

		public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo memberInfo, bool inherit, bool throwOnMissing)
		{
			IEnumerable<T> attributes = memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
			if (throwOnMissing && !attributes.Any())
				throw new Exception(string.Format("No attribute implementing '{0}' was found on the member '{1}' of type {2}", typeof(T).Name, memberInfo.Name, memberInfo.DeclaringType));
			return attributes;
		}

		public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit, bool throwOnMissing)
		{
			List<T> attributes = new List<T>();
			foreach (Type t in type.EnumerateParentTypes())
				foreach (T attribute in ((MemberInfo) t).GetCustomAttributes<T>(inherit, throwOnMissing))
					if (!attributes.Contains(attribute))
						attributes.Add(attribute);
			return attributes;
		}
	}
}