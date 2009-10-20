Ext.namespace('Ext.ux.zeus');

Ext.ux.zeus.NewItemContextMenuPlugin = function(title, url)
{
	this.title = title;
	this.url = url;
}

Ext.extend(Ext.ux.zeus.NewItemContextMenuPlugin, Ext.ux.zeus.ContextMenuPlugin,
{
	execute: function()
	{
		zeus.reloadContentPanel(this.title, this.url);
	}
});