using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Isis.ComponentModel;
using Zeus.Web;

namespace Zeus.Engine
{
	/// <summary>
	/// Keeps track of and provides aspect controllers in the system.
	/// </summary>
	public class AspectControllerProvider : IAspectControllerProvider, IStartable
	{
		private readonly ContentEngine engine;
		private readonly IAssemblyFinder _assemblyFinder;
		private readonly ITypeFinder _typeFinder;
		private IControllerDescriptor[] controllerDescriptors = new IControllerDescriptor[0];

		public AspectControllerProvider(ContentEngine engine, IAssemblyFinder assemblyFinder, ITypeFinder typeFinder)
		{
			this.engine = engine;
			_assemblyFinder = assemblyFinder;
			_typeFinder = typeFinder;
		}

		#region IAspectControllerProvider Members

		/// <summary>Resolves the controller for the current Url.</summary>
		/// <returns>A suitable controller for the given Url.</returns>
		public virtual T ResolveAspectController<T>(PathData path) where T : class, IAspectController
		{
			if (path == null || path.IsEmpty()) return null;

			T controller = CreateControllerInstance<T>(path);
			if (controller == null) return null;

			controller.Path = path;
			controller.Engine = engine;
			return controller;
		}

		/// <summary>Adds controller descriptors to the list of descriptors. This is typically auto-wired using the [Controls] attribute.</summary>
		/// <param name="descriptorToAdd">The controller descriptors to add.</param>
		public void RegisterAspectController(params IControllerDescriptor[] descriptorToAdd)
		{
			lock (this)
			{
				List<IControllerDescriptor> references = new List<IControllerDescriptor>(controllerDescriptors);
				references.AddRange(descriptorToAdd);
				references.Sort();
				controllerDescriptors = references.ToArray();
			}
		}

		#endregion

		protected virtual T CreateControllerInstance<T>(PathData path) where T : class, IAspectController
		{
			Type requestedType = typeof(T);

			foreach (IControllerDescriptor reference in controllerDescriptors)
				if (requestedType.IsAssignableFrom(reference.ControllerType) && reference.IsControllerFor(path, requestedType))
					return Activator.CreateInstance(reference.ControllerType) as T;

			throw new ZeusException("Couldn't find an aspect controller '{0}' for the item '{1}' on the path '{2}'.", typeof(T).FullName, path.CurrentItem, path.Path);
		}

		#region IStartable Members

		public void Start()
		{
			List<IControllerDescriptor> references = new List<IControllerDescriptor>();
			foreach (Type controllerType in _typeFinder.Find(typeof(IAspectController)))
				foreach (IControllerDescriptor reference in controllerType.GetCustomAttributes(typeof(IControllerDescriptor), false))
				{
					reference.ControllerType = controllerType;
					references.Add(reference);
				}

			//collect [assembly: Controls(..)] attributes
			foreach (ICustomAttributeProvider assembly in _assemblyFinder.GetAssemblies())
				foreach (IControllerDescriptor reference in assembly.GetCustomAttributes(typeof(IControllerDescriptor), false))
					if (null != reference.ControllerType)
						references.Add(reference);

			RegisterAspectController(references.ToArray());
		}

		public void Stop()
		{
		}

		#endregion
	}
}