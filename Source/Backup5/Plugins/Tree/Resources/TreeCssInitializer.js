Ext.util.CSS.createStyleSheet(".x-tree-node .x-tree-node-inline-icon { height: auto !important; vertical-align: middle; } .x-tree-node-anchor { vertical-align: middle; }");

var keyUp = function(el, e) {
	var tree = stpNavigation;
	var text = this.getRawValue();

	if (e.getKey() === 40) {
		tree.getRootNode().select();
	}

	if (Ext.isEmpty(text, false)) {
		clearFilter(el);
	}

	if (text.length < 2) {
		return;
	}

	tree.clearFilter();

	if (Ext.isEmpty(text, false)) {
		return;
	}

	el.triggers[0].show();

	if (e.getKey() === Ext.EventObject.ESC) {
		clearFilter(el);
	} else {
		var re = new RegExp(".*" + text + ".*", "i");

		tree.expandAll();

		tree.filterBy(function(node) {
			return re.test(node.text.replace(/<span>&nbsp;<\/span>/g, ""));
		});
	}
};

var clearFilter = function(el, trigger, index, e) {
	var tree = stpNavigation;

	el.setValue("");
	el.triggers[0].hide();
	tree.clearFilter();
	tree.getRootNode().collapseChildNodes(true);
};