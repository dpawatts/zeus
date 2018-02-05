Ext.namespace('Ext.ux.zeus');

Ext.ux.zeus.NewItemGridMenuPlugin = function(title, url)
{
	this.title = title;
	this.url = url;
}

Ext.extend(Ext.ux.zeus.NewItemGridMenuPlugin, Ext.ux.zeus.GridMenuPlugin,
{
	execute: function()
	{
		zeus.reloadContentPanel(this.title, this.url);
	}
});