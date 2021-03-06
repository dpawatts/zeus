/*
  * Ext.ux.menu.StoreMenu  Addon
  *
  * @author    Marco Wienkoop (wm003/lubber)
  * @copyright (c) 2009, Marco Wienkoop (marco.wienkoop@lubber.de) http://www.lubber.de
  *
  * @class Ext.ux.menu.StoreMenu
  * @extends Ext.menu.Menu

* Donations are always welcome :)
* Any amount is greatly appreciated and will help to continue on developing ExtJS Widgets
*
* You can donate via PayPal to donate@lubber.de
*
	This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>
	
 * This Addon requires the ExtJS Library, which is distributed under the terms of the GPL v3 (from V2.1)
 * See http://extjs.com/license for more info


Revision History
v 0.2 [2009/03/04]
- added support for submenu ("menu" has to be delivered such as "handler")

v 0.1 [2009/03/03]
Initial release
*/

Ext.namespace('Ext.ux.menu');

Ext.ux.menu.StoreMenu = function(config) {
	Ext.ux.menu.StoreMenu.superclass.constructor.call(this,config);
	if(!this.store){
//at least url/proxy or data need to be given in config when initiating this component
		this.store = new Ext.data.SimpleStore({
			fields: ['config'],
			url: this.url,
			baseParams: this.baseParams /*,
			proxy:this.proxy,
			data: this.data*/
		});
	}
	this.on('show', this.onMenuLoad, this);
	this.store.on('beforeload', this.onBeforeLoad, this);	
	this.store.on('load', this.onLoad, this);


};

Ext.reg("storemenu", Ext.ux.menu.StoreMenu);

Ext.extend(Ext.ux.menu.StoreMenu, Ext.menu.Menu, {
	enableScrolling: false,
	loadingText: Ext.LoadMask.prototype.msg || 'Loading...',
	loaded: false,

	onMenuLoad: function() {
		if (!this.loaded) {
			//			if(this.options) {
			//				this.store.loadData(this.options);
			//			}
			//			else {
			this.store.load();
			//			}
		}
	},

	updateMenuItems: function(loadedState, records) {
		//var visible = this.isVisible();
		//this.hide(false);

		this.removeAll();
		//to sync the height of the shadow		
		this.el.sync();

		if (loadedState) {

			for (var i = 0, len = records.length; i < len; i++) {
				//create a real function if a handler or menu is given as a string (because a function cannot really be encoded in JSON
				if (records[i].json.handler) {
					eval("records[i].json.handler = " + records[i].json.handler);
					//					records[i].json.handler = new Function(records[i].json.handler);
				}
				if (records[i].json.menu) {
					for (var j = 0, jlen = records[i].json.menu.items.length; j < jlen; j++)
						if (records[i].json.menu.items[j].handler)
						eval("records[i].json.menu.items[j].handler = " + records[i].json.menu.items[j].handler);
					//eval("records[i].json.menu = "+records[i].json.menu);
					//					records[i].json.menu = new Function(records[i].json.menu);
				}

				this.add(records[i].json);
			}
			//			this.hide();
			//			this.show();			


		}
		else {
			this.add('<span class="loading-indicator">' + this.loadingText + '</span>');
		}

		this.loaded = loadedState;
		//if (visible && loadedState) {
		if (loadedState) {
			this.showAt(this.getEl().getXY());
			//this.show(this.getEl().getXY());
			//this.show();
		}
	},

	onBeforeLoad: function(store) {
		this.store.baseParams = this.baseParams;
		this.updateMenuItems(false);
	},

	onLoad: function(store, records) {
		this.updateMenuItems(true, records);
	}
});