using System;

namespace Zeus.ContentTypes
{
	/// <summary>This exceptions is thrown when trying to add an item to an unsupported parent item.</summary>
	public class NotAllowedParentException : ZeusException
	{
		public NotAllowedParentException(ContentType contentType, Type parentType)
			: base("The item '{0}' isn't allowed below a destination of type '{1}'.",
			contentType.Title,
			parentType.AssemblyQualifiedName)
		{
			ContentType = contentType;
			ParentType = parentType;
		}

		public ContentType ContentType
		{
			get;
			private set;
		}

		public Type ParentType
		{
			get;
			private set;
		}
	}
}
