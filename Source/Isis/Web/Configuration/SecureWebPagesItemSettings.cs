using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// Indicates the type of security for a file or directory.
	/// </summary>
	public enum SecurityType
	{

		/// <summary>
		/// The item should be secure.
		/// </summary>
		Secure,

		/// <summary>
		/// The item should be insecure.
		/// </summary>
		Insecure,

		/// <summary>
		/// The item should be ignored.
		/// </summary>
		Ignore

	}

	/// <summary>
	/// Represents an file or directory entry in the &lt;secureWebPages&gt;
	/// configuration section.
	/// </summary>
	public abstract class SecureWebPageItemSetting : ConfigurationElement
	{

		#region Constructors

		/// <summary>
		/// Creates an instance of SecureWebPageItemSetting.
		/// </summary>
		protected SecureWebPageItemSetting()
			: base()
		{
		}

		/// <summary>
		/// Creates an instance with an initial path value.
		/// </summary>
		/// <param name="path">The relative path to the file.</param>
		protected SecureWebPageItemSetting(string path)
			: this()
		{
			Path = path;
		}

		/// <summary>
		/// Creates an instance with initial values.
		/// </summary>
		/// <param name="path">The relative path to the file.</param>
		/// <param name="secure">The type of security for the file.</param>
		protected SecureWebPageItemSetting(string path, SecurityType secure)
			: this(path)
		{
			Secure = secure;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the path of this item.
		/// </summary>
		[ConfigurationProperty("path", IsRequired = true, IsKey = true)]
		public virtual string Path
		{
			get { return (string) this["path"]; }
			set { this["path"] = value; }
		}

		/// <summary>
		/// Gets or sets the type of security for this item.
		/// </summary>
		[ConfigurationProperty("secure", DefaultValue = SecurityType.Secure)]
		public SecurityType Secure
		{
			get { return (SecurityType) this["secure"]; }
			set { this["secure"] = value; }
		}

		#endregion

	}

	/// <summary>
	/// Represents a collection of SecureWebPageItemSetting objects.
	/// </summary>
	public abstract class SecureWebPageItemSettingCollection : ConfigurationElementCollection
	{

		/// <summary>
		/// Returns the index of a specified item in the collection.
		/// </summary>
		/// <param name="item">The item to find.</param>
		/// <returns>Returns the index of the item.</returns>
		public int IndexOf(SecureWebPageItemSetting item)
		{
			if (item != null)
				return BaseIndexOf(item);
			else
				return -1;
		}

		/// <summary>
		/// Returns the index of an item with the specified path in the collection.
		/// </summary>
		/// <param name="path">The path of the item to find.</param>
		/// <returns>Returns the index of the item with the path.</returns>
		public int IndexOf(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			else
				return this.IndexOf((SecureWebPageItemSetting) BaseGet(path.ToLower(CultureInfo.InvariantCulture)));
		}

	}
}
