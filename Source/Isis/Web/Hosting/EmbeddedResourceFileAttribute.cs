using System;

namespace Isis.Web.Hosting
{
	/// <summary>
	/// Attribute indicating the location of an embedded resource that should be
	/// served by the <see cref="EmbeddedResourcePathProvider"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute is used by the <see cref="EmbeddedResourcePathProvider"/>
	/// module/path provider to retrieve the list of embedded resources from an assembly
	/// that should be considered a part of the virtual filesystem.
	/// </para>
	/// </remarks>
	/// <example>
	/// <para>
	/// Below is an example of what it might look like to embed a web form and a
	/// user control in an assembly to be served up with the <see cref="EmbeddedResourcePathProvider"/>:
	/// </para>
	/// <code lang="C#">
	/// [assembly: EmbeddedResourceFileAttribute("MyNamespace.WebForm1.aspx", "MyNamespace")]
	/// [assembly: EmbeddedResourceFileAttribute("MyNamespace.UserControl1.ascx", "MyNamespace")]
	/// </code>
	/// </example>
	/// <seealso cref="EmbeddedResourcePathProvider"/>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class EmbeddedResourceFileAttribute : Attribute
	{
		
		#region EmbeddedResourceFileAttribute Variables

		#region Static

		/// <summary>
		/// Regular expression indicating the characters that can't start or end a resource.
		/// </summary>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		/// <seealso cref="EmbeddedResourceFileAttribute.RemoveMalformedEndChars" />
		private static System.Text.RegularExpressions.Regex MalformedEndCharExpression = new System.Text.RegularExpressions.Regex("^[. ]*(.*?)[. ]*$", System.Text.RegularExpressions.RegexOptions.Compiled);

		#endregion

		#region Instance

		/// <summary>
		/// Internal storage for the
		/// <see cref="EmbeddedResourceFileAttribute.ResourceNamespace" />
		/// property.
		/// </summary>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		private readonly string _resourceNamespace;

		/// <summary>
		/// Internal storage for the
		/// <see cref="ResourcePath" />
		/// property.
		/// </summary>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		private readonly string _resourcePath;

		#endregion

		#endregion

		#region EmbeddedResourceFileAttribute Properties

		/// <summary>
		/// Gets the namespace for the embedded resource.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the embedded resource namespace.
		/// </value>
		/// <remarks>
		/// <para>
		/// This namespace will be removed from the full <see cref="EmbeddedResourceFileAttribute.ResourcePath"/>
		/// to create the virtual application path for the resource.
		/// </para>
		/// </remarks>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		public string ResourceNamespace
		{
			get { return _resourceNamespace; }
		}

		/// <summary>
		/// Gets the path to the embedded resource.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the full path to an embedded resource in
		/// the associated assembly.
		/// </value>
		/// <remarks>
		/// <para>
		/// This path will be used to retrieve the resource from the assembly and serve
		/// it up.
		/// </para>
		/// </remarks>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		public string ResourcePath
		{
			get { return _resourcePath; }
		}

		#endregion

		#region EmbeddedResourceFileAttribute Implementation

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EmbeddedResourceFileAttribute" /> class.
		/// </summary>
		/// <param name="resourcePath">The path to the embedded resource.  Used to get the resource as a stream from the assembly.</param>
		/// <param name="resourceNamespace">The namespace the resource is in.  This will generally be removed from the full resource path to calculate the "application path" for the embedded resource.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="resourcePath" /> or <paramref name="resourceNamespace" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="resourcePath" /> is <see cref="System.String.Empty" />
		/// or if it only consists of periods and/or spaces.
		/// </exception>
		/// <remarks>
		/// <para>
		/// Both <paramref name="resourcePath" /> and <paramref name="resourceNamespace" />
		/// will be processed to have leading and trailing periods and spaces removed.
		/// If <paramref name="resourcePath" /> ends up being empty, an
		/// <see cref="System.ArgumentOutOfRangeException"/> is thrown.  No exception
		/// is thrown if <paramref name="resourceNamespace" /> turns out empty.
		/// </para>
		/// </remarks>
		/// <example>
		/// <para>
		/// If the <paramref name="resourcePath" /> is <c>RootNS.SubNS.AppRoot.Folder.File.aspx</c>
		/// and the <paramref name="resourceNamespace" /> is <c>RootNS.SubNS.AppRoot</c>,
		/// the virtual "path" to the embedded file will be <c>Folder.File.aspx</c>
		/// (which will be converted by the <see cref="EmbeddedResourcePathProvider"/>
		/// to <c>~/Folder/File.aspx</c>).
		/// </para>
		/// </example>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		public EmbeddedResourceFileAttribute(string resourcePath, string resourceNamespace)
		{
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath");
			}
			if (resourcePath.Length == 0)
			{
				throw new ArgumentOutOfRangeException("resourcePath");
			}
			if (resourceNamespace == null)
			{
				throw new ArgumentNullException("resourceNamespace");
			}

			_resourcePath = RemoveMalformedEndChars(resourcePath);
			if (_resourcePath.Length == 0)
			{
				throw new ArgumentOutOfRangeException("resourcePath", resourcePath, "The resource path is invalid for mapping."); 
			}
			_resourceNamespace = RemoveMalformedEndChars(resourceNamespace);
			if (_resourceNamespace.Length == 0)
			{
				throw new ArgumentOutOfRangeException("resourceNamespace", resourceNamespace, "The resource namespace is invalid for mapping.");
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Removes leading and trailing dots and spaces from a string.
		/// </summary>
		/// <param name="toFix">The <see cref="System.String"/> to be cleaned up.</param>
		/// <returns>A version of <paramref name="toFix" /> with leading and trailing dots and spaces removed.</returns>
		/// <seealso cref="EmbeddedResourceFileAttribute" />
		private static string RemoveMalformedEndChars(string toFix)
		{
			return MalformedEndCharExpression.Replace(toFix, "$1");
		}

		#endregion

		#endregion

	}
}