using System.Collections.Generic;
using System.Linq;
using Isis.ExtensionMethods.Reflection;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	public class EditableHierarchyBuilder<T> : IEditableHierarchyBuilder<T>
		where T : IContainable
	{
		/// <summary>Build the container hierarchy adding containers and editors to a root container.</summary>
		/// <param name="containers">The containers to add to themselves or the root container.</param>
		/// <param name="editables">The editables to add to the containers or a root container.</param>
		/// <returns>A new root container.</returns>
		public virtual IEditorContainer Build(IEnumerable<IEditorContainer> containers, IEnumerable<T> editables)
		{
			// Clear existing containers, in case this is a rebuild.
			foreach (IEditorContainer container in containers)
				container.Contained.Clear();

			IEditorContainer rootContainer = new RootEditorContainer();
			AddContainersToRootContainer(rootContainer, containers);
			AddEditorsToContainers(rootContainer, containers, editables);
			return rootContainer;
		}

		#region Helpers

		private static void AddContainersToRootContainer(IEditorContainer rootContainer, IEnumerable<IEditorContainer> containers)
		{
			foreach (IEditorContainer container in containers)
			{
				if (container.ContainerName != null)
				{
					if (container.ContainerName == container.Name)
						throw new ZeusException("The container '{0}' cannot reference itself as containing container. Change the ContainerName property.", container.Name);

					IEditorContainer parentContainer = FindContainer(container.ContainerName, containers);

					if (parentContainer == null)
						throw new ZeusException("The container '{0}' references another containing container '{1}' that doesn't seem to be defined. Either add a container with this name or remove the reference to that container.", container.Name, container.ContainerName);

					parentContainer.AddContained(container);
				}
				else
				{
					rootContainer.AddContained(container);
				}
			}
		}

		private static IEditorContainer FindContainer(string name, IEnumerable<IEditorContainer> containers)
		{
			return containers.SingleOrDefault(c => c.Name == name);
		}

		private static void AddEditorsToContainers(IEditorContainer rootContainer, IEnumerable<IEditorContainer> containers, IEnumerable<T> editables)
		{
			foreach (T editable in editables)
			{
				if (editable.ContainerName != null)
				{
					IEditorContainer container = FindContainer(editable.ContainerName, containers);
					if (container == null)
						throw new ZeusException("The editor '{0}' references a container '{1}' that doesn't seem to be defined. Either add a container with this name or remove the reference to that container.", editable.Name, editable.ContainerName);
					container.AddContained(editable);
				}
				else
				{
					rootContainer.AddContained(editable);
				}
			}
		}

		#endregion
	}
}