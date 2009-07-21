using System;
using System.Collections.Generic;
using Isis.Collections;

namespace Zeus.Web
{
	/// <summary>
	/// Provides access to a static dictionary of item path finders. These
	/// are used to map a content item to the appropriate template (aspx)
	/// depending on the url action.
	/// </summary>
	public class PathDictionary : SingletonDictionary<Type, IPathFinder[]>
	{
		/// <summary>Looks up path finders from a static cache.</summary>
		/// <param name="itemType">The type of item whose path finders to get.</param>
		/// <returns>A list of path finders.</returns>
		public static IPathFinder[] GetFinders(Type itemType)
		{
			lock (Instance)
			{
				if (Instance.ContainsKey(itemType))
					return Instance[itemType];

				return Instance[itemType] = FindFinders(itemType);
			}
		}

		/// <summary>Adds a path finder to an item.</summary>
		/// <param name="itemType">The type to add the finder to.</param>
		/// <param name="finder">The finder to add.</param>
		public static void PrependFinder(Type itemType, IPathFinder finder)
		{
			lock (Instance)
			{
				List<IPathFinder> newFinders = new List<IPathFinder>(GetFinders(itemType));
				newFinders.Insert(0, finder);
				Instance[itemType] = newFinders.ToArray();
			}
		}

		/// <summary>Adds a path finder to an item.</summary>
		/// <param name="itemType">The type to add the finder to.</param>
		/// <param name="finder">The finder to add.</param>
		public static void AppendFinder(Type itemType, IPathFinder finder)
		{
			lock (Instance)
			{
				List<IPathFinder> newFinders = new List<IPathFinder>(GetFinders(itemType));
				newFinders.Add(finder);
				Instance[itemType] = newFinders.ToArray();
			}
		}

		/// <summary>Adds a path finder to an item.</summary>
		/// <param name="itemType">The type to add the finder to.</param>
		/// <param name="finder">The finder to add.</param>
		public static void RemoveFinder(Type itemType, IPathFinder finder)
		{
			lock (Instance)
			{
				List<IPathFinder> newFinders = new List<IPathFinder>(GetFinders(itemType));
				newFinders.Remove(finder);
				Instance[itemType] = newFinders.ToArray();
			}
		}

		/// <summary>Looks up path finders for a certain type using reflection.</summary>
		/// <param name="itemType">The type of item whose path finders to get.</param>
		/// <returns>A list of path finders that decorates the item class and it's base types.</returns>
		static IPathFinder[] FindFinders(Type itemType)
		{
			object[] attributes = itemType.GetCustomAttributes(typeof(IPathFinder), true);
			List<IPathFinder> pathFinders = new List<IPathFinder>(attributes.Length);
			foreach (IPathFinder finder in attributes)
			{
				pathFinders.Add(finder);
			}
			return pathFinders.ToArray();
		}
	}
}