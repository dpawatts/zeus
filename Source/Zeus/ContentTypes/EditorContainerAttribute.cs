using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.UI;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class EditorContainerAttribute : Attribute, IEditorContainer
	{
		private readonly List<IContainable> _contained = new List<IContainable>();

		public EditorContainerAttribute(string name, int sortOrder)
		{
			Name = name;
			SortOrder = sortOrder;
		}

		/// <summary>Gets or sets roles allowed to access this container.</summary>
		public string[] AuthorizedRoles { get; set; }

		public string Name { get; set; }

		public int SortOrder { get; set; }

		public string ContainerName { get; set; }

		public abstract Control AddTo(Control container);

		/// <summary>Find out whether a user has permission to view this container.</summary>
		/// <param name="user">The user to check.</param>
		/// <returns>True if the user has the required permissions.</returns>
		public virtual bool IsAuthorized(IPrincipal user)
		{
			if (AuthorizedRoles == null)
				return true;
			if (user == null)
				return false;

			foreach (string role in AuthorizedRoles)
				if (user.IsInRole(role))
					return true;
			return false;
		}

		public List<IContainable> Contained
		{
			get { return _contained; }
		}

		public IList<IContainable> GetContained(IPrincipal user)
		{
			return _contained.Where(c => c.IsAuthorized(user)).ToList();
		}

		public void AddContained(IContainable containable)
		{
			_contained.Add(containable);
		}

		#region IComparable x 2 Members

		public int CompareTo(IEditorContainer other)
		{
			return SortOrder.CompareTo(other.SortOrder);
		}

		public int CompareTo(IContainable other)
		{
			return SortOrder - other.SortOrder;
		}

		#endregion
	}
}