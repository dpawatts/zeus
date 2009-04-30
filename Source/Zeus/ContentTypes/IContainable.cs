﻿using System;
using System.Security.Principal;
using System.Web.UI;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Classes implementing this interface can add a graphical representation to 
	/// a control hierarchy.
	/// </summary>
	public interface IContainable : IUniquelyNamed, IComparable<IContainable>
	{
		string ContainerName { get; set; }

		/// <summary>The order of this container compared to other containers and editors. Editors within the container are sorted according to their sort order.</summary>
		int SortOrder { get; set; }

		/// <summary>Adds a containable control to a container and returns it.</summary>
		/// <param name="container">The container onto which to add the containable control.</param>
		/// <returns>The newly added control.</returns>
		Control AddTo(Control container);

		/// <summary>Find out whether a user has permission to edit this detail.</summary>
		/// <param name="user">The user to check.</param>
		/// <returns>True if the user has the required permissions.</returns>
		bool IsAuthorized(IPrincipal user);
	}
}