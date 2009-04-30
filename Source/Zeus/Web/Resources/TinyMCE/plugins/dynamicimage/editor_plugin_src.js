/**
 * $Id: editor_plugin_src.js 201 2007-02-12 15:56:56Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2008, Moxiecode Systems AB, All rights reserved.
 */

(function() {
	tinymce.create('tinymce.plugins.DynamicImagePlugin', {
		init : function(ed, url) {
			var t = this;

			ed.onBeforeSetContent.add(function(ed, o) {
			o.content = t['_dynamicimage2html'](o.content);
			});

			ed.onPostProcess.add(function(ed, o) {
				if (o.set)
					o.content = t['_dynamicimage2html'](o.content);

				if (o.get)
					o.content = t['_html2dynamicimage'](o.content);
			});
		},

		getInfo : function() {
			return {
				longname : 'DynamicImage Plugin',
				author : 'Sound in Theory Ltd',
				authorurl : 'http://www.sitdap.com',
				infourl : 'http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/dynamicimage',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		},

		// Private methods

		// HTML -> BBCode in PunBB dialect
		_html2dynamicimage : function(s) {
			s = tinymce.trim(s);

			function rep(re, str) {
				s = s.replace(re, str);
			};

			// example: <strong> to [b]
			rep(/<img.*?src=\"(.*?)\".*?width=\"(.*?)\".*?height=\"(.*?)\".*?\/>/gi,"[img width=$2 height=$3]$1[/img]");

			return s; 
		},

		// BBCode -> HTML from PunBB dialect
		_dynamicimage2html: function(s) {
			s = tinymce.trim(s);

			function rep(re, str) {
				s = s.replace(re, str);
			};

			// example: [b] to <strong>
			rep(/\[img width=(.*?) height=(.*?)\](.*?)\[\/img\]/gi, "<img src=\"$3\" width=\"$1\" height=\"$2\" />");

			return s; 
		}
	});

	// Register plugin
	tinymce.PluginManager.add('dynamicimage', tinymce.plugins.DynamicImagePlugin);
})();