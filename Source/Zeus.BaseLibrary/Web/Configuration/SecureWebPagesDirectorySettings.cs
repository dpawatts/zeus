using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// Represents a directory entry in the &lt;secureWebPages&gt;
	/// configuration section.
	/// </summary>
	public class SecureWebPageDirectorySetting : SecureWebPageItemSetting
	{

		#region Constructors

		/// <summary>
		/// Creates an instance of SecureWebPageDirectorySetting.
		/// </summary>
		public SecureWebPageDirectorySetting()
			: base()
		{
		}

		/// <summary>
		/// Creates an instance with an initial path value.
		/// </summary>
		/// <param name="path">The relative path to the directory.</param>
		public SecureWebPageDirectorySetting(string path)
			: base(path)
		{
		}

		/// <summary>
		/// Creates an instance with initial values.
		/// </summary>
		/// <param name="path">The relative path to the directory.</param>
		/// <param name="secure">The type of security for the directory.</param>
		public SecureWebPageDirectorySetting(string path, SecurityType secure)
			: base(path, secure)
		{
		}

		/// <summary>
		/// Creates an instance with initial values.
		/// </summary>
		/// <param name="path">The relative path to the directory or file.</param>
		/// <param name="secure">The type of security for the directory.</param>
		/// <param name="recurse">A flag indicating whether or not to recurse this directory 
		/// when evaluating security.</param>
		public SecureWebPageDirectorySetting(string path, SecurityType secure, bool recurse)
			: this(path, secure)
		{
			Recurse = recurse;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the path of this directory setting.
		/// </summary>
		[ConfigurationProperty("path", IsRequired = true, IsKey = true), RegexStringValidator(@"^(?:|/|[\w\-][\w\.\-,]*(?:/[\w\.\-]+)*/?)$")]
		public override string Path
		{
			get { return base.Path; }
			set { base.Path = CleanPath(value); }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to include all files in any sub-directories 
		/// when evaluating a request.
		/// </summary>
		[ConfigurationProperty("recurse", DefaultValue = false)]
		public bool Recurse
		{
			get { return (bool) this["recurse"]; }
			set { this["recurse"] = value; }
		}

		#endregion

		/// <summary>
		/// Overriden to "clean-up" any inconsistent, yet allowed, input.
		/// </summary>
		protected override void PostDeserialize()
		{
			base.PostDeserialize();
			this["path"] = CleanPath(Path);
		}

		/// <summary>
		/// Cleans the specified path as needed.
		/// </summary>
		/// <param name="path">The path to clean.</param>
		/// <returns>A string containing the cleaned path value.</returns>
		protected string CleanPath(string path)
		{
			// Strip any trailing slash from the path.
			if (path.EndsWith("/"))
				return path.Substring(0, path.Length - 1);
			else
				return path;
		}

	}

	/// <summary>
	/// Represents a collection of SecureWebPageDirectorySetting objects.
	/// </summary>
	public class SecureWebPageDirectorySettingCollection : SecureWebPageItemSettingCollection
	{

		/// <summary>
		/// Gets the element name for this collection.
		/// </summary>
		protected override string ElementName
		{
			get { return "directories"; }
		}

		/// <summary>
		/// Gets a flag indicating an exception should be thrown if a duplicate element 
		/// is added to the collection.
		/// </summary>
		protected override bool ThrowOnDuplicate
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		/// <param name="index">The index to retrieve the element from.</param>
		/// <returns>The SecureWebPageDirectorySetting located at the specified index.</returns>
		public SecureWebPageDirectorySetting this[int index]
		{
			get { return (SecureWebPageDirectorySetting) BaseGet(index); }
		}

		/// <summary>
		/// Gets the element with the specified path.
		/// </summary>
		/// <param name="path">The path of the element to retrieve.</param>
		/// <returns>The SecureWebPageDirectorySetting with the specified path.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public new SecureWebPageDirectorySetting this[string path]
		{
			get
			{
				if (path == null)
					throw new ArgumentNullException("path");
				else
					return (SecureWebPageDirectorySetting) BaseGet(path.ToLower(CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// Creates a new element for this collection.
		/// </summary>
		/// <returns>A new instance of SecureWebPageFileSetting.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new SecureWebPageDirectorySetting();
		}

		/// <summary>
		/// Gets the key for the specified element.
		/// </summary>
		/// <param name="element">An element to get a key for.</param>
		/// <returns>A string containing the Path of the SecureWebPageDirectorySetting.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element != null)
				return ((SecureWebPageDirectorySetting) element).Path.ToLower(CultureInfo.InvariantCulture);
			else
				return null;
		}
	}
}
