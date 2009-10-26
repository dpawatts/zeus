/// <reference assembly="Coolite.Ext.Web" name="Coolite.Ext.Web.Build.Resources.Coolite.coolite.intellisense.js" />

Ext.namespace('Ext.ux.zeus');

Ext.ux.zeus.DeleteConfirmation = function()
{
	this.showDialog = function(title, subtitle, affectedItemIDs)
	{
		var bodySize = Ext.getBody().getViewSize();
		var width = (bodySize.width < 500) ? bodySize.width - 50 : 500;
		var height = (bodySize.height < 300) ? bodySize.height - 50 : 300;

		var win = new Ext.Window(
		{
			modal: true,
			width: width,
			height: height,
			title: title,
			layout: "fit",
			maximizable: true,
			listeners:
			{
				"maximize":
				{
					fn: function(el)
					{
						var v = Ext.getBody().getViewSize();
						el.setSize(v.width, v.height);
					},
					scope: this
				}
			},
			items: new Ext.form.FormPanel(
			{
				baseCls: "x-plain",
				layout: "absolute",
				defaultType: "label",
				items:
				[
					{
						x: 5,
						y: 5,
						html: '<div class="x-window-dlg"><div class="ext-mb-warning" style="width:32px;height:32px"></div></div>'
					},
					{
						x: 42,
						y: 6,
						html: subtitle
					},
					new Ext.TabPanel(
					{
						x: 0,
						y: 42,
						anchor: '100% 100%',
						autoTabs: true,
						activeTab: 0,
						deferredRender: false,
						border: false,
						items:
						[
							{
								title: 'Affected Items',
								xtype: 'treepanel',
								autoScroll: true,
								rootVisible: false,
								loader: new Ext.tree.TreeLoader(
								{
									dataUrl: '/admin/Plugins/DeleteItem/AffectedItems.ashx',
									listeners:
									{
										load:
										{
											fn: function(loader, node)
											{
												node.getOwnerTree().expandAll();
											}
										}
									}
								}),
								root:
								{
									nodeType: 'async',
									text: "Root",
									id: affectedItemIDs,
									expanded: true
								}
							},
							{
								title: 'Referencing Items',
								tabTip: 'Items referencing the item(s) you\'re deleting',
								xtype: 'treepanel',
								autoScroll: true,
								rootVisible: false,
								loader: new Ext.tree.TreeLoader(
								{
									dataUrl: '/admin/Plugins/DeleteItem/ReferencingItems.ashx'
								}),
								root:
								{
									nodeType: 'async',
									text: "Root",
									id: affectedItemIDs,
									expanded: true
								}
							}
						]
					})
				]
			}),
			buttons:
			[
				{
					text: 'Yes',
					handler: function()
					{
						stbStatusBar.showBusy("Deleting...");
						win.hide();
						Coolite.AjaxMethods.Delete.DeleteItems(affectedItemIDs,
						{
							url: "/admin/default.aspx",
							success: function(result)
							{
								stbStatusBar.setStatus({ text: "Deleted Item(s)", iconCls: '', clear: true });
							}
						});
					}
				},
				{
					text: 'No',
					handler: function()
					{
						win.hide();
					}
				}
			]
		});
		win.show();
	};
}

Ext.ux.zeus.DeleteConfirmation.show = function(affectedItemTitle, affectedItemID, affectedItemIcon)
{
	var dialog = new Ext.ux.zeus.DeleteConfirmation();
	dialog.showDialog("Delete Item",
		'<b>Are you sure you wish to delete this item?</b><br /><img src="' + affectedItemIcon + '" /> ' + affectedItemTitle,
		affectedItemID);
}

Ext.ux.zeus.DeleteConfirmation.showMultiple = function(affectedItemIDs)
{
	var dialog = new Ext.ux.zeus.DeleteConfirmation();
	dialog.showDialog("Delete Items", "Are you sure you wish to delete these items?", affectedItemIDs);
}