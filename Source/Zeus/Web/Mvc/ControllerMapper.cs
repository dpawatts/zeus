using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Zeus.BaseLibrary.Reflection;
using Zeus.ContentTypes;

namespace Zeus.Web.Mvc
{
	public class ControllerMapper : IControllerMapper
	{
		private readonly IDictionary<Type, string> _controllerMap = new Dictionary<Type, string>();
		private readonly IDictionary<Type, string> _areaMap = new Dictionary<Type, string>();

		public ControllerMapper(ITypeFinder typeFinder, IContentTypeManager definitionManager, IKernel kernel)
		{
			IList<ControlsAttribute> controllerDefinitions = FindControllers(typeFinder);
			foreach (ContentType id in definitionManager.GetContentTypes())
			{
				IAdapterDescriptor controllerDefinition = GetControllerFor(id.ItemType, controllerDefinitions);
				if (controllerDefinition != null)
				{
					ControllerMap[id.ItemType] = controllerDefinition.ControllerName;
					AreaMap[id.ItemType] = controllerDefinition.AreaName;

					kernel.Bind<IController>().To(controllerDefinition.AdapterType)
						.InTransientScope()
						.Named(GetControllerName(controllerDefinition.AdapterType, controllerDefinition.AreaName));

					IList<IPathFinder> finders = PathDictionary.GetFinders(id.ItemType);
					if (0 == finders.Where(f => f is ActionResolver).Count())
					{
						// TODO: Get the list of methods from a list of actions retrieved from somewhere within MVC
						var methods = controllerDefinition.AdapterType.GetMethods().Select(m => m.Name).ToArray();
						var actionResolver = new ActionResolver(this, methods);
						PathDictionary.PrependFinder(id.ItemType, actionResolver);
					}
				}
			}
		}

		private static string GetControllerName(Type type, string areaName)
		{
			string name = type.Name.ToLowerInvariant();

			if (name.EndsWith("controller"))
				name = name.Substring(0, name.IndexOf("controller"));

			if (!string.IsNullOrEmpty(areaName))
				name = areaName.ToLowerInvariant() + "." + name;

			return name;
		}

		public string GetControllerName(Type type)
		{
			string name;
			ControllerMap.TryGetValue(type, out name);
			return name;
		}

		public string GetAreaName(Type type)
		{
			string name;
			AreaMap.TryGetValue(type, out name);
			return name;
		}

		private IDictionary<Type, string> ControllerMap
		{
			get { return _controllerMap; }
		}

		private IDictionary<Type, string> AreaMap
		{
			get { return _areaMap; }
		}

		private static IAdapterDescriptor GetControllerFor(Type itemType, IList<ControlsAttribute> controllerDefinitions)
		{
			List<ControlsAttribute> controllers = new List<ControlsAttribute>();
			foreach (ControlsAttribute controllerDefinition in controllerDefinitions)
				if (controllerDefinition.ItemType.IsAssignableFrom(itemType))
					controllers.Add(controllerDefinition);
			return controllers.OrderByDescending(c => c.Priority).FirstOrDefault();
		}

		private static IList<ControlsAttribute> FindControllers(ITypeFinder typeFinder)
		{
			var controllerDefinitions = new List<ControlsAttribute>();
			foreach (Type controllerType in typeFinder.Find(typeof(IController)))
			{
				foreach (ControlsAttribute attr in controllerType.GetCustomAttributes(typeof(ControlsAttribute), false))
				{
					attr.AdapterType = controllerType;
					controllerDefinitions.Add(attr);
				}
			}
			controllerDefinitions.Sort();
			return controllerDefinitions;
		}
	}
}