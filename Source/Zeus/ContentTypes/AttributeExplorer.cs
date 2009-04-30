using System;
using System.Collections.Generic;
using System.Reflection;
using Zeus.ContentProperties;
using Zeus.Security;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Finds uniquely named attributes on a type. Attributes defined on the 
	/// class must have their name attribute set, and attributes defined on a
	/// property their name set to the property's name.
	/// </summary>
	/// <typeparam name="T">The type of attribute to find.</typeparam>
	public class AttributeExplorer<T> where T : IUniquelyNamed
	{
		public IList<T> Find(Type typeToExplore)
		{
			List<T> attributes = new List<T>();

			AddEditablesDefinedOnProperties(typeToExplore, attributes);
			AddEditablesDefinedOnClass(typeToExplore, attributes);

			if (attributes.Count > 1 && (attributes[0] is IComparable || attributes[0] is IComparable<T>))
				attributes.Sort();

			return attributes;
		}

		public IDictionary<string, T> Map(Type typeToExplore)
		{
			IList<T> attributes = Find(typeToExplore);
			Dictionary<string, T> map = new Dictionary<string, T>();
			foreach (T a in attributes)
				map[a.Name] = a;
			return map;
		}

		#region Helpers

		private static void AddEditablesDefinedOnProperties(Type exploredType, ICollection<T> attributes)
		{
			foreach (PropertyInfo propertyOnItem in exploredType.GetProperties())
				foreach (T attributeOnProperty in propertyOnItem.GetCustomAttributes(typeof(T), false))
					if (!attributes.Contains(attributeOnProperty))
					{
						attributeOnProperty.Name = propertyOnItem.Name;
						if (attributeOnProperty is ISecurable)
							foreach (PropertyAuthorizedRolesAttribute rolesAttribute in propertyOnItem.GetCustomAttributes(typeof(PropertyAuthorizedRolesAttribute), false))
							{
								ISecurable s = attributeOnProperty as ISecurable;
								s.AuthorizedRoles = rolesAttribute.Roles;
							}
						if (attributeOnProperty is IPropertyAwareAttribute)
						{
							IPropertyAwareAttribute a = attributeOnProperty as IPropertyAwareAttribute;
							a.UnderlyingProperty = propertyOnItem;
						}
						attributes.Add(attributeOnProperty);
					}
		}

		private static void AddEditablesDefinedOnClass(Type exploredType, ICollection<T> attributes)
		{
			foreach (Type t in EnumerateTypeAncestralHierarchy(exploredType))
				foreach (T editableOnClass in t.GetCustomAttributes(typeof(T), true))
					if (!attributes.Contains(editableOnClass))
					{
						if (editableOnClass.Name == null)
							throw new ZeusException(
								"The attribute {0} does not have a Name defined. Since it's defined on the class instead of a property it must have a name.",
								editableOnClass);

						if (!attributes.Contains(editableOnClass))
							attributes.Add(editableOnClass);
					}
		}

		private static IEnumerable<Type> EnumerateTypeAncestralHierarchy(Type type)
		{
			while (type != typeof(object))
			{
				yield return type;
				type = type.BaseType;
			}
		}

		#endregion
	}
}