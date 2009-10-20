using System.ComponentModel;
using System.Web.UI;
using Coolite.Ext.Web;

namespace Coolite.Ext.UX
{
	public class StoreGridCommand : GridCommand
	{
		private StoreCommandMenu _menu;

		[ClientConfig("menu", JsonMode.Object)]
		[Category("Config Options")]
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[ViewStateMember]
		public override CommandMenu Menu
		{
			get
			{
				if (_menu == null)
					_menu = new StoreCommandMenu();
				return _menu;
			}
		}
	}

	//[ClientScript(Type = typeof(StoreMenu), WebResource = "Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu-min.js", FilePath = "ux/extensions/storemenu/storemenu.js")]
	//[Xtype("storemenu")]
	//[InstanceOf(ClassName = "Ext.ux.menu.StoreMenu")]
	public class StoreCommandMenu : CommandMenu
	{
		[ClientConfig]
		[Category("Config Options")]
		[DefaultValue("")]
		public virtual string Xtype
		{
			get { return "storemenu"; }
		}

		[ClientConfig]
		[Category("Config Options")]
		[DefaultValue("")]
		[Description("The URL to fetch the menu items from")]
		[NotifyParentProperty(true)]
		public virtual string Url
		{
			get
			{
				return (string)this.ViewState["Url"] ?? "";
			}
			set
			{
				this.ViewState["Url"] = value;
			}
		}

		private FakeParameterCollection _baseParams;

		[ClientConfig("baseParams", JsonMode.Object)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public FakeParameterCollection BaseParams
		{
			get
			{
				if (_baseParams == null)
					_baseParams = new FakeParameterCollection();

				return _baseParams;
			}
		}
	}

	public class FakeParameterCollection : StateManagedItem
	{
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[ClientConfig]
		public string Node
		{
			get
			{
				return (string)this.ViewState["Node"] ?? "";
			}
			set
			{
				this.ViewState["Node"] = value;
			}
		}
	}
}