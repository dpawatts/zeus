using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeus.Edit;

namespace Zeus.Definitions
{
	public class DefinitionManager : IDefinitionManager
	{
		#region Fields

		private string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu";
		private IDictionary<Type, ItemDefinition> _definitions;

		#endregion

		#region Properties

		public ItemDefinition this[Type type]
		{
			get { return _definitions[type]; }
		}

		public ItemDefinition this[string discriminator]
		{
			get
			{
				foreach (ItemDefinition definition in _definitions.Values)
					if (definition.Discriminator == discriminator)
						return definition;
				return null;
			}
		}

		#endregion

		#region Constructor

		public DefinitionManager()
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

		public ICollection<ItemDefinition> GetDefinitions()
		{
			return _definitions.Values;
		}

		private void FindDefinitions()
		{
			// Find definitions.
			_definitions = new Dictionary<Type, ItemDefinition>();
			Type requestedType = typeof(ContentItem);
			foreach (Assembly assembly in GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (requestedType.IsAssignableFrom(type))
						if (!type.IsAbstract)
						{
							ItemDefinition itemDefinition = new ItemDefinition(type);
							itemDefinition.Editables = FindEditableAttributes(itemDefinition.ItemType);
							_definitions.Add(type, itemDefinition);
						}
				}
			}

			// Refine.
			foreach (ItemDefinition definition in _definitions.Values)
				foreach (IDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IDefinitionRefiner), false))
					refiner.Refine(definition);
		}

		private ICollection<IEditable> FindEditableAttributes(Type type)
		{
			List<IEditable> editables = new List<IEditable>();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
				foreach (IEditable editable in propertyInfo.GetCustomAttributes(typeof(IEditable), false))
				{
					editable.Name = propertyInfo.Name;
					editables.Add(editable);
				}
			return editables;
		}

		private IList<Assembly> GetAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
					assemblies.Add(assembly);
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
