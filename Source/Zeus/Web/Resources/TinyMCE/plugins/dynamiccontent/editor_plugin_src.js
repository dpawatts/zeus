/**
 * $Id: editor_plugin_src.js 677 2008-03-07 13:52:41Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2008, Moxiecode Systems AB, All rights reserved.
 */

(function() {
	tinymce.create('tinymce.plugins.DynamicContentPlugin', {
		init: function(ed, url) {
			// Register commands
			ed.addCommand('mceDynamicContent', function() {
				// Internal image object like a flash placeholder
				if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1)
					return;

				ed.windowManager.open({
					file: '/Admin/DynamicContent/default.aspx',
					width: 700,
					height: 450,
					inline: 1
				}, {
					plugin_url: url
				});
			});

			// Register buttons
			ed.addButton('dynamiccontent', {
				title: 'Dynamic Content',
				image: '/admin/assets/images/icons/page_white_code.png',
				cmd: 'mceDynamicContent'
			});

			// Add a node change handler, selects the button in the UI when a image is selected
			ed.onNodeChange.add(function(ed, cm, n) {
				cm.setActive('dynamiccontent', n.nodeName == 'DYNAMICCONTENT');
			});
		},

		getInfo: function() {
			return {
				longname: 'Zeus Dynamic Content',
				author: 'Sound in Theory Ltd',
				authorurl: 'http://www.sitdap.com',
				infourl: 'http://www.sitdap.com',
				version: "0.1"
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('dynamiccontent', tinymce.plugins.DynamicContentPlugin);
})();