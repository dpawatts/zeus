using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeus.ContentTypes.Properties;
using System.IO;
using System.Web;
using System.Diagnostics;

namespace Zeus.ContentTypes
{
	public class ContentTypeManager : IContentTypeManager
	{
		#region Fields

		private string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^SoundInTheory\\.NMigration";
		private IDictionary<Type, ContentType> _definitions;

		#endregion

		#region Properties

		public ContentType this[Type type]
		{
			get
			{
				if (_definitions.ContainsKey(type))
					return _definitions[type];
				else
					return null;
			}
		}

		public ContentType this[string discriminator]
		{
			get
			{
				foreach (ContentType definition in _definitions.Values)
					if (definition.Discriminator == discriminator)
						return definition;
				return null;
			}
		}

		#endregion

		#region Constructor

		public ContentTypeManager()
		{
			FindDefinitions();
		}

		#endregion

		#region Methods

		public T CreateInstance<T>(ContentItem parentItem)
			where T : ContentItem
		{
			T item = Activator.CreateInstance<T>();
			OnItemCreating(item, parentItem);
			return item;
		}

		/// <summary>Creates an instance of a certain type of item. It's good practice to create new items through this method so the item's dependencies can be injected by the engine.</summary>
		/// <returns>A new instance of an item.</returns>
		public ContentItem CreateInstance(Type itemType, ContentItem parentItem)
		{
			ContentItem item = Activator.CreateInstance(itemType) as ContentItem;
			OnItemCreating(item, parentItem);
			return item;
		}

		protected virtual void OnItemCreating(ContentItem item, ContentItem parentItem)
		{
			item.Parent = parentItem;
		}

		public ICollection<ContentType> GetDefinitions()
		{
			return _definitions.Values;
		}

		private void FindDefinitions()
		{
			// Find definitions.
			IList<ContentType> definitions = new List<ContentType>();
			Type requestedType = typeof(ContentItem);
			foreach (Assembly assembly in GetAssemblies())
			{
				try
				{
					foreach (Type type in assembly.GetTypes())
					{
						if (requestedType.IsAssignableFrom(type))
							if (!type.IsAbstract)
							{
								ContentType itemDefinition = new ContentType(type);
								itemDefinition.Properties = itemDefinition.ItemType.GetProperties().Select(pi => new Property(pi)).ToArray();
								definitions.Add(itemDefinition);
							}
					}
				}
				catch (ReflectionTypeLoadException ex)
				{
					string loaderErrors = string.Empty;
					foreach (Exception loaderEx in ex.LoaderExceptions)
					{
						Trace.TraceError(loaderEx.ToString());
						loaderErrors += ", " + loaderEx.Message;
					}

					throw new ZeusException("Error getting types from assembly " + assembly.FullName + loaderErrors, ex);
				}
			}

			// Refine.
			foreach (ContentType definition in definitions)
				foreach (IDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IDefinitionRefiner), false))
					refiner.Refine(definition);
			foreach (ContentType definition in definitions)
				foreach (IInheritableDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IInheritableDefinitionRefiner), true))
					refiner.Refine(definition, definitions);

			_definitions = definitions.ToDictionary(d => d.ItemType);
		}

		private IList<Assembly> GetAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
					assemblies.Add(assembly);
			}

			foreach (string dllPath in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/bin"), "*.dll"))
			{
				try
				{
					Assembly assembly = Assembly.ReflectionOnlyLoadFrom(dllPath);
					if (Matches(assembly.FullName) && !assemblies.Any(a => a.FullName == assembly.FullName))
					{
						Assembly loadedAssembly = AppDomain.CurrentDomain.Load(assembly.FullName);
						assemblies.Add(loadedAssembly);
					}
				}
				catch (BadImageFormatException ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}

			return assemblies;
		}

		private bool Matches(string assemblyFullName)
		{
			return !Matches(assemblyFullName, _assemblySkipLoadingPattern);
		}

		private bool Matches(string assemblyFullName, string pattern)
		{
			return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		#endregion
	}
}
