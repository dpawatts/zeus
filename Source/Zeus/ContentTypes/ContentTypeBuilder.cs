using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Isis.ExtensionMethods.Reflection;
using Zeus.ContentTypes.Properties;

namespace Zeus.ContentTypes
{
	public class ContentTypeBuilder : IContentTypeBuilder
	{
		private string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^SoundInTheory\\.NMigration|^SoundInTheory\\.DynamicImage";

		public ContentTypeBuilder()
		{
			
		}

		public IDictionary<Type, ContentType> GetDefinitions()
		{
			IList<ContentType> definitions = FindDefinitions();
			Refine(definitions);
			return definitions.ToDictionary(d => d.ItemType);
		}

		private IList<ContentType> FindDefinitions()
		{
			// Find definitions.
			IList<ContentType> definitions = new List<ContentType>();
			foreach (Type type in EnumerateTypes())
			{
				ContentType itemDefinition = new ContentType(type);
				itemDefinition.Properties = itemDefinition.ItemType.GetProperties().Select(pi => new Property(pi)).ToArray();

				itemDefinition.Editors = GetAttributes<IEditor>(itemDefinition.ItemType).OrderBy(e => e.SortOrder);
				foreach (IEditor editor in itemDefinition.Editors)
					if (editor is IEditorRefiner)
						((IEditorRefiner) editor).Refine(itemDefinition.Properties.Single(p => p.Name == editor.Name).UnderlyingProperty.PropertyType);

				itemDefinition.Displayers = GetAttributes<IDisplayer>(itemDefinition.ItemType);
				itemDefinition.Containers = itemDefinition.ItemType.GetCustomAttributes<IEditorContainer>(true, false).OrderBy(c => c.SortOrder);
				itemDefinition.RootContainer = BuildContainerHierarchy(itemDefinition, itemDefinition.Containers, itemDefinition.Editors);
				definitions.Add(itemDefinition);
			}

			return definitions;
		}

		private IEnumerable<T> GetAttributes<T>(Type type)
			where T : class, IUniquelyNamed
		{
			List<T> attributes = new List<T>();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				List<T> propertyAttributes = propertyInfo.GetCustomAttributes<T>(false, false).ToList();
				propertyAttributes.ForEach(a => a.Name = propertyInfo.Name);
				attributes.AddRange(propertyAttributes);
			}
			return attributes;
		}

		private IEditorContainer BuildContainerHierarchy(ContentType contentType, IEnumerable<IEditorContainer> containers, IEnumerable<IEditor> editors)
		{
			RootEditorContainer rootContainer = new RootEditorContainer();

			// Add containers to root container.
			foreach (IEditorContainer container in containers)
			{
				IEditorContainer parentContainer = FindContainer(container.ContainerName, rootContainer, containers);
				parentContainer.AddContained(container);
			}

			// Add editors to containers.
			foreach (IEditor editor in editors)
			{
				IEditorContainer container = FindContainer(editor.ContainerName, rootContainer, containers);
				if (container == null)
					throw new ZeusException("The container '{0}' referenced by editor '{1}' in content type '{2}' could not be found.", editor.ContainerName, editor.Name, contentType.ContentTypeAttribute.Title);
				container.AddContained(editor);
			}

			return rootContainer;
		}

		private static IEditorContainer FindContainer(string name, RootEditorContainer rootContainer, IEnumerable<IEditorContainer> containers)
		{
			if (name == null)
				return rootContainer;
			else
				return containers.SingleOrDefault(c => c.Name == name);
		}

		private static void Refine(IList<ContentType> definitions)
		{
			// Refine.
			foreach (ContentType definition in definitions)
				foreach (IDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IDefinitionRefiner), false))
					refiner.Refine(definition);
			foreach (ContentType definition in definitions)
				foreach (IInheritableDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IInheritableDefinitionRefiner), true))
					refiner.Refine(definition, definitions);
		}

		private IEnumerable<Type> EnumerateTypes()
		{
			return FindTypes(typeof(ContentItem)).Where(t => !t.IsAbstract);
		}

		private IEnumerable<Type> FindTypes(Type requestedType)
		{
			List<Type> types = new List<Type>();
			foreach (Assembly assembly in GetAssemblies())
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

					throw new ZeusException("Error getting types from assembly " + assembly.FullName + loaderErrors, ex);
				}
			}
			return types;
		}

		private IList<Assembly> GetAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
					assemblies.Add(assembly);
			}

			if (HttpContext.Current != null)
			{
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
	}
}
