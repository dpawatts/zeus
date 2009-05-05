using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class AdminSection : ConfigurationSection
	{
		[ConfigurationProperty("installer")]
		public InstallerElement Installer
		{
			get { return (InstallerElement) base["installer"]; }
			set { base["installer"] = value; }
		}

		[ConfigurationProperty("recycleBin")]
		public RecycleBinElement RecycleBin
		{
			get { return (RecycleBinElement) base["recycleBin"]; }
			set { base["recycleBin"] = value; }
		}

		[ConfigurationProperty("versioning")]
		public VersioningElement Versioning
		{
			get { return (VersioningElement) base["versioning"]; }
			set { base["versioning"] = value; }
		}

		[ConfigurationProperty("tree")]
		public TreeElement Tree
		{
			get { return (TreeElement) base["tree"]; }
			set { base["tree"] = value; }
		}

		[ConfigurationProperty("deleteItemUrl", DefaultValue = "~/admin/delete.aspx")]
		public string DeleteItemUrl
		{
			get { return (string) base["deleteItemUrl"]; }
			set { base["deleteItemUrl"] = value; }
		}

		[ConfigurationProperty("editItemUrl", DefaultValue = "~/admin/edit.aspx")]
		public string EditItemUrl
		{
			get { return (string) base["editItemUrl"]; }
			set { base["editItemUrl"] = value; }
		}

		[ConfigurationProperty("enableVersioning", DefaultValue = true)]
		public bool EnableVersioning
		{
			get { return (bool) base["enableVersioning"]; }
			set { base["enableVersioning"] = value; }
		}

		[ConfigurationProperty("newItemUrl", DefaultValue = "~/admin/new.aspx")]
		public string NewItemUrl
		{
			get { return (string) base["newItemUrl"]; }
			set { base["newItemUrl"] = value; }
		}

		[ConfigurationProperty("name", DefaultValue = "[None]")]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("hideBranding", DefaultValue = false)]
		public bool HideBranding
		{
			get { return (bool) base["hideBranding"]; }
			set { base["hideBranding"] = value; }
		}

		[ConfigurationProperty("path", DefaultValue = "admin")]
		public string Path
		{
			get { return (string) base["path"]; }
			set { base["path"] = value; }
		}
	}
}
