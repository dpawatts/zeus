/// <reference assembly="Coolite.Ext.Web" name="Coolite.Ext.Web.Build.Resources.Coolite.coolite.intellisense.js" />

Ext.namespace('Ext.ux.zeus');

Ext.ux.zeus.FileManager = function(windowDialog, view, tree)
{
	this.windowDialog = windowDialog;
	this.view = view;
	this.tree = tree;
};

Ext.ux.zeus.FileManager.prototype = {
	// Cache data by image name for easy lookup
	lookup: {},

	formatData: function(data)
	{
		data.shortName = Ext.util.Format.ellipsis(data.name, 15);
		this.lookup[data.name] = data;
		return data;
	},

	show: function(sourceElement, callback, type)
	{
		this.animateTarget = sourceElement;
		this.callback = callback;
		this.type = type;

		this.reset();
		this.windowDialog.show(this.animateTarget);
	},

	hide: function()
	{
		this.windowDialog.hide();
	},

	doCallback: function()
	{
		var selectedNode = this.view.getSelectedNodes()[0];
		var callback = this.callback;
		var lookup = this.lookup;
		this.windowDialog.hide(this.animateTarget, function()
		{
			if (selectedNode && callback)
			{
				var data = lookup[selectedNode.id];
				callback(data);
			}
		});
	},

	reset: function()
	{
		if (this.windowDialog.rendered)
			this.view.getEl().dom.scrollTop = 0;

		var rootNode = this.tree.getRootNode();
		this.tree.getLoader().load(rootNode);
		rootNode.expand();

		this.view.store.reload({ params: { node: rootNode.attributes.id, type: this.type } });
		this.view.select(0);
	}
};