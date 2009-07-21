using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Isis.ComponentModel;
using Zeus.Web;

namespace Zeus.Engine
{
	/// <summary>
	/// Keeps track of and provides content adapters in the system.
	/// </summary>
	public class ContentAdapterProvider : IContentAdapterProvider, IStartable
	{
		readonly ContentEngine engine;
		private readonly ITypeFinder _typeFinder;
		readonly IAssemblyFinder assemblyFinder;
		IAdapterDescriptor[] adapterDescriptors = new IAdapterDescriptor[0];

		public ContentAdapterProvider(ContentEngine engine, IAssemblyFinder finder, ITypeFinder typeFinder)
		{
			this.engine = engine;
			this.assemblyFinder = finder;
			_typeFinder = typeFinder;
		}

		public IEnumerable<IAdapterDescriptor> AdapterDescriptors
		{
			get { return adapterDescriptors; }
		}

		#region IContentAdapterProvider Members


		/// <summary>Resolves the controller for the current Url.</summary>
		/// <returns>A suitable controller for the given Url.</returns>
		public virtual T ResolveAdapter<T>(PathData path) where T : class, IContentAdapter
		{
			if (path == null || path.IsEmpty()) return null;

			T controller = CreateAdapterInstance<T>(path);
			if (controller == null) return null;

			controller.Path = path;
			controller.Engine = engine;
			return controller;
		}

		/// <summary>Adds an adapter descriptors to the list of descriptors. This is typically auto-wired using the [Controls] attribute.</summary>
		/// <param name="descriptorsToAdd">The adapter descriptors to add.</param>
		public void RegisterAdapter(params IAdapterDescriptor[] descriptorsToAdd)
		{
			lock (this)
			{
				List<IAdapterDescriptor> references = new List<IAdapterDescriptor>(adapterDescriptors);
				references.AddRange(descriptorsToAdd);
				references.Sort();
				adapterDescriptors = references.ToArray();
			}
		}

		#endregion

		protected virtual T CreateAdapterInstance<T>(PathData path) where T : class, IContentAdapter
		{
			Type requestedType = typeof(T);

			foreach (IAdapterDescriptor reference in adapterDescriptors)
				if (requestedType.IsAssignableFrom(reference.AdapterType) && reference.IsAdapterFor(path, requestedType))
					return Activator.CreateInstance(reference.AdapterType) as T;

			throw new ZeusException("Couldn't find an aspect controller '{0}' for the item '{1}' on the path '{2}'.",
				typeof(T).FullName, path.CurrentItem, path.Path);
		}

		#region IStartable Members

		public void Start()
		{
			List<IAdapterDescriptor> references = new List<IAdapterDescriptor>();
			foreach (Type controllerType in _typeFinder.Find(typeof(IContentAdapter)))
			{
				foreach (IAdapterDescriptor reference in controllerType.GetCustomAttributes(typeof(IAdapterDescriptor), false))
				{
					reference.AdapterType = controllerType;
					references.Add(reference);
				}
			}

			//collect [assembly: Controls(..)] attributes
			foreach (ICustomAttributeProvider assembly in assemblyFinder.GetAssemblies())
			{
				foreach (IAdapterDescriptor reference in assembly.GetCustomAttributes(typeof(IAdapterDescriptor), false))
				{
					if (null == reference.ItemType) throw new ZeusException("The assembly '{0}' defines a [assembly: Controls(null)] attribute with no ItemType specified. Please specify this property: [assembly: Controls(typeof(MyItem), AdapterType = typeof(MyAdapter)]", assembly);
					if (null == reference.AdapterType) throw new ZeusException("The assembly '{0}' defines a [assembly: Controls(typeof({1})] attribute with no AdapterType specified. Please specify this property: [assembly: Controls(typeof({1}), AdapterType = typeof(MyAdapter)]", assembly, reference.ItemType.Name);

					references.Add(reference);
				}
			}

			RegisterAdapter(references.ToArray());
		}

		public void Stop()
		{
		}

		#endregion
	}
}