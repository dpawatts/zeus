using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// Represents an file entry in the &lt;secureWebPages&gt;
	/// configuration section.
	/// </summary>
	public class SecureWebPageFileSetting : SecureWebPageItemSetting
	{

		#region Constructors

		/// <summary>
		/// Creates an instance of SecureWebPageFileSetting.
		/// </summary>
		public SecureWebPageFileSetting()
			: base()
		{
		}

		/// <summary>
		/// Creates an instance with an initial path value.
		/// </summary>
		/// <param name="path">The relative path to the file.</param>
		public SecureWebPageFileSetting(string path)
			: base(path)
		{
		}

		/// <summary>
		/// Creates an instance with initial values.
		/// </summary>
		/// <param name="path">The relative path to the file.</param>
		/// <param name="secure">The type of security for the file.</param>
		public SecureWebPageFileSetting(string path, SecurityType secure)
			: base(path, secure)
		{
		}

		#endregion

		/// <summary>
		/// Gets or sets the path of this file setting.
		/// </summary>
		[ConfigurationProperty("path", IsRequired = true, IsKey = true), RegexStringValidator(@"^(?:|[\w\-][\w\.\-,]*(?:/[\w\.\-,]+)*)$")]
		public override string Path
		{
			get { return base.Path; }
			set { base.Path = value; }
		}

	}

	/// <summary>
	/// Represents a collection of SecureWebPageFileSetting objects.
	/// </summary>
	public class SecureWebPageFileSettingCollection : SecureWebPageItemSettingCollection
	{

		/// <summary>
		/// Gets the element name for this collection.
		/// </summary>
		protected override string ElementName
		{
			get { return "files"; }
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
		/// <returns>The SecureWebPageFileSetting located at the specified index.</returns>
		public SecureWebPageFileSetting this[int index]
		{
			get { return (SecureWebPageFileSetting) BaseGet(index); }
		}

		/// <summary>
		/// Gets the element with the specified path.
		/// </summary>
		/// <param name="path">The path of the element to retrieve.</param>
		/// <returns>The SecureWebPageFileSetting with the specified path.</returns>
		public new SecureWebPageFileSetting this[string path]
		{
			get
			{
				if (path == null)
					throw new ArgumentNullException("path");
				else
					return (SecureWebPageFileSetting) BaseGet(path.ToLower(CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// Creates a new element for this collection.
		/// </summary>
		/// <returns>A new instance of SecureWebPageFileSetting.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new SecureWebPageFileSetting();
		}

		/// <summary>
		/// Gets the key for the specified element.
		/// </summary>
		/// <param name="element">An element to get a key for.</param>
		/// <returns>A string containing the Path of the SecureWebPageFileSetting.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element != null)
				return ((SecureWebPageFileSetting) element).Path.ToLower(CultureInfo.InvariantCulture);
			else
				return null;
		}
	}
}
