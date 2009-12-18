var WindowManager = Ext.extend(
	function(editor)
	{
		WindowManager.superclass.constructor.call(this, editor);
	},
	tinymce.WindowManager,
	{
		// Override WindowManager methods
		alert: function(txt, cb, s)
		{
			Ext.MessageBox.alert("", txt, function() { cb.call(this); }, s);
		},

		confirm: function(txt, cb, s)
		{
			Ext.MessageBox.confirm("", txt, function(btn) { cb.call(this, btn == "yes"); }, s);
		},

		open: function(s, p)
		{

			s = s || {};
			p = p || {};

			if (!s.type)
				this.bookmark = this.editor.selection.getBookmark('simple');

			s.width = parseInt(s.width || 320);
			s.height = parseInt(s.height || 240) + (tinymce.isIE ? 8 : 0);
			s.min_width = parseInt(s.min_width || 150);
			s.min_height = parseInt(s.min_height || 100);
			s.max_width = parseInt(s.max_width || 2000);
			s.max_height = parseInt(s.max_height || 2000);
			s.movable = s.resizable = true;
			p.mce_width = s.width;
			p.mce_height = s.height;
			p.mce_inline = true;

			this.features = s;
			this.params = p;

			var win = new Ext.Window(
			{
				title: s.name,
				width: s.width,
				height: s.height,
				minWidth: s.min_width,
				minHeight: s.min_height,
				resizable: true,
				maximizable: s.maximizable == true,
				minimizable: s.minimizable == true,
				modal: true,
				layout: "fit",
				items: [
				{
					xtype: "iframepanel",
					defaultSrc: s.url || s.file
				}
				]
			});

			p.mce_window_id = win.getId();

			win.show(null,
				function()
				{
					if (s.left && s.top)
						win.setPagePosition(s.left, s.top);
					var pos = win.getPosition();
					s.left = pos[0];
					s.top = pos[1];
					this.onOpen.dispatch(this, s, p);
				},
				this
			);

			return win;
		},

		close: function(win)
		{

			// Probably not inline
			if (!win.tinyMCEPopup || !win.tinyMCEPopup.id)
			{
				WindowManager.superclass.close.call(this, win);
				return;
			}

			var w = Ext.getCmp(win.tinyMCEPopup.id);
			if (w)
			{
				this.onClose.dispatch(this);
				w.close();
			}
		},

		setTitle: function(win, ti)
		{

			// Probably not inline
			if (!win.tinyMCEPopup || !win.tinyMCEPopup.id)
			{
				WindowManager.superclass.setTitle.call(this, win, ti);
				return;
			}

			var w = Ext.getCmp(win.tinyMCEPopup.id);
			if (w) w.setTitle(ti);
		},

		resizeBy: function(dw, dh, id)
		{

			var w = Ext.getCmp(id);
			if (w)
			{
				var size = w.getSize();
				w.setSize(size.width + dw, size.height + dh);
			}
		},

		focus: function(id)
		{
			var w = Ext.getCmp(id);
			if (w) w.setActive(true);
		}
	}
);

var freeTextArea_settings = {
	mode: 'exact',
	theme: 'advanced',
	plugins: 'style,layer,table,advimage,advlink,iespell,media,searchreplace,print,contextmenu,paste,fullscreen,noneditable,inlinepopups,dynamiccontent',
	theme_advanced_buttons1_add_before: '',
	theme_advanced_buttons1_add: 'styleprops,sup,|,print,fullscreen,|,search,replace,iespell,|,forecolorpicker,|,dynamiccontent',
	theme_advanced_buttons2_add_before: 'cut,copy,paste,pastetext,pasteword,|',
	theme_advanced_buttons2_add: '|,table,media,insertlayer,inlinepopups',
	theme_advanced_buttons3: '',
	theme_advanced_buttons3_add_before: '',
	theme_advanced_buttons3_add: '',
	theme_advanced_buttons4: '',
	theme_advanced_toolbar_location: 'top',
	theme_advanced_toolbar_align: 'left',
	theme_advanced_path_location: 'bottom',
	extended_valid_elements: 'hr[class|width|size|noshade],span[class|align|style],pre[class],code[class],iframe[src|width|height|name|align],dynamiccontent[state]',
	file_browser_callback: 'fileBrowserCallBack',
	theme_advanced_resize_horizontal: false,
	theme_advanced_resizing: true,
	theme_advanced_disable: 'help,fontselect,fontsizeselect,forecolor,backcolor',
	relative_urls: false,
	noneditable_noneditable_class: 'nonEditable',
	custom_elements: '~dynamiccontent', // Notice the ~ prefix to force a span element for the element
	width: "500px",
	height: "400px",
	setup: function(ed)
	{
		ed.onPostRender.add(function(ed, cm)
		{
			// Change window manager
			ed.windowManager = new WindowManager(ed);
		});
	}
};
		
var fileBrowserUrl;
var srcField;

function fileBrowserCallBack(field_name, url, destinationType, win)
{
	srcField = win.document.forms[0].elements[field_name];
	var fileSelectorWindow = window.open(fileBrowserUrl + '&availableModes=All&tbid=' + srcField.id + '&destinationType=' + destinationType + '&selectedUrl=' + encodeURIComponent(url), 'FileBrowser', 'height=600,width=400,resizable=yes,status=yes,scrollbars=yes');
	fileSelectorWindow.focus();
}

function onFileSelected(selectedUrl)
{
	srcField.value = selectedUrl;
}

function htmlEditor_init(fileBrowser, overrides)
{
	fileBrowserUrl = fileBrowser;
	jQuery.extend(freeTextArea_settings, overrides);
	tinyMCE.init(freeTextArea_settings);
}