using System;
using NHibernate.Cfg;
using Zeus.ContentTypes;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Zeus.Configuration;
using System.Configuration;

namespace Zeus.Persistence
{
	public class ConfigurationBuilder : IConfigurationBuilder
	{
		private IContentTypeManager _definitions;
		private NHibernate.Cfg.Configuration _configuration;
		private string _mappingFormat = @"<?xml version=""1.0"" encoding=""utf-16""?>
<hibernate-mapping xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:nhibernate-mapping-2.2"">
{0}
</hibernate-mapping>
";
		private string _classFormat = @"<subclass name=""{0}"" extends=""{1}"" discriminator-value=""{2}"" lazy=""false""/>";

		public NHibernate.Cfg.Configuration Configuration
		{
			get { return _configuration; }
		}

		public ConfigurationBuilder(IContentTypeManager definitions, DatabaseSection databaseSectionConfig)
		{
			_definitions = definitions;

			if (databaseSectionConfig == null)
				databaseSectionConfig = new DatabaseSection();

			IDictionary<string, string> properties = new Dictionary<string, string>();
			properties["dialect"] = "NHibernate.Dialect.MsSql2005Dialect";
			properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
			properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
			properties["connection.connection_string"] = ConfigurationManager.ConnectionStrings[databaseSectionConfig.ConnectionStringName].ConnectionString;
			properties["cache.use_second_level_cache"] = databaseSectionConfig.CacheEnabled.ToString();
			properties["cache.use_query_cache"] = databaseSectionConfig.CacheEnabled.ToString();
			properties["cache.provider_class"] = databaseSectionConfig.CacheProviderClass;

			_configuration = new NHibernate.Cfg.Configuration();

			foreach (KeyValuePair<string, string> pair in properties)
				_configuration.SetProperty(pair.Key, pair.Value);

			_configuration.AddAssembly(Assembly.GetExecutingAssembly());

			// For each definition, add a <subclass> element to mapping file.
			StringBuilder mappings = new StringBuilder();
			foreach (Type type in EnumerateDefinedTypes())
				mappings.AppendFormat(_classFormat, GetName(type), GetName(type.BaseType), GetDiscriminator(type));
			_configuration.AddXml(string.Format(_mappingFormat, mappings.ToString()));
		}

		private IEnumerable<Type> EnumerateDefinedTypes()
		{
			List<Type> types = new List<Type>();
			foreach (ContentType definition in _definitions.GetContentTypes())
				foreach (Type t in EnumerateBaseTypes(definition.ItemType))
				{
					if (t.IsSubclassOf(typeof(ContentItem)) && !types.Contains(t))
					{
						int index = types.IndexOf(t.BaseType);
						types.Insert(index + 1, t);
					}
				}
			return types;
		}

		/// <summary>Enumerates base type chain of the supplied type.</summary>
		/// <param name="t">The type whose base types will be enumerated.</param>
		/// <returns>An enumerator of the supplied item and all it's base types.</returns>
		private static IEnumerable<Type> EnumerateBaseTypes(Type t)
		{
			if (t != null)
			{
				foreach (Type baseType in EnumerateBaseTypes(t.BaseType))
					yield return baseType;
				yield return t;
			}
		}

		private static string GetName(Type t)
		{
			return t.FullName + ", " + t.Assembly.FullName.Split(',')[0];
		}

		private string GetDiscriminator(Type itemType)
		{
			ContentType definition = _definitions[itemType];
			if (definition != null)
				return definition.Discriminator;
			else
				return itemType.Name;
		}
	}
}
