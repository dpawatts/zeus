using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	/// <summary>
	/// Defines an editable child item. The edited item is referenced by the 
	/// property decorated with this attribute. If the property is null a new
	/// item is created and added to the parent item's child collection.
	/// </summary>
	/// <example>
	///		public class ParentItem : Zeus.ContentItem
	///		{
	/// 		[Zeus.Design.Editors.ChildEditor]
	/// 		public virtual ChildItem News
	/// 		{
	/// 			get { return (ChildItem)(GetDetail("ChildItem")); }
	/// 			set { SetDetail("ChildItem", value); }
	/// 		}
	///		}
	/// </example>
	[AttributeUsage(AttributeTargets.Property)]
	public class ChildEditorAttribute : AbstractEditorAttribute
	{
		#region Fields

		#endregion

		#region Constructors

		public ChildEditorAttribute()
		{
		}

		public ChildEditorAttribute(int sortOrder)
			: this(null, null, sortOrder)
		{
		}

		public ChildEditorAttribute(string defaultChildName, int sortOrder)
			: this(defaultChildName, null, sortOrder)
		{
		}

		public ChildEditorAttribute(string defaultChildName, string defaultChildZoneName, int sortOrder)
		{
			DefaultChildName = defaultChildName;
			DefaultChildZoneName = defaultChildZoneName;
			SortOrder = sortOrder;
		}

		#endregion

		#region Properties

		/// <summary>The name that will be assigned to new child items.</summary>
		public string DefaultChildName { get; set; }

		/// <summary>The zone name that will be assigned to new child items.</summary>
		public string DefaultChildZoneName { get; set; }

		protected virtual IContentTypeManager Definitions
		{
			get { return Context.ContentTypes; }
		}

		#endregion

		#region Methods

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			ItemEditView itemEditor = editor as ItemEditView;
			itemEditor.Update();
			ItemUtility.FindInParents<ItemEditView>(editor.Parent).Saved += ((sender, args) => itemEditor.Save());
			return true;
		}

		protected override void DisableEditor(Control editor)
		{
			((ItemEditView) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
		}

		public override Control AddTo(Control container)
		{
			Control panel = AddPanel(container);

			return AddEditor(panel);
		}

		protected override Control AddEditor(Control panel)
		{
			ItemEditView editor = new ItemEditView();
			editor.ID = Name;
			//editor.ZoneName = DefaultChildZoneName;
			editor.Init += OnChildEditorInit;
			panel.Controls.Add(editor);
			return editor;
		}

		protected virtual void OnChildEditorInit(object sender, EventArgs e)
		{
			ItemEditView itemEditor = sender as ItemEditView;
			ItemEditView parentEditor = ItemUtility.FindInParents<ItemEditView>(itemEditor.Parent);
			itemEditor.CurrentItem = GetChild((ContentItem) parentEditor.CurrentItem);
		}

		protected virtual ContentItem GetChild(ContentItem item)
		{
			ContentItem childItem = Utility.GetProperty(item, Name) as ContentItem;
			if (childItem == null)
			{
				PropertyInfo pi = item.GetType().GetProperty(Name);

				if (pi == null)
					throw new ZeusException("The item should have had a property named '{0}'", Name);
				childItem = CreateChild(item, pi.PropertyType);

				pi.SetValue(item, childItem, null);
			}
			return childItem;
		}

		protected virtual ContentItem CreateChild(ContentItem item, Type childItemType)
		{
			ContentItem child;
			try
			{
				child = Definitions.CreateInstance(childItemType, item);
			}
			catch (KeyNotFoundException ex)
			{
				Trace.TraceWarning(
					"EditableItemAttribute.CreateChild: No item of the type {0} was found among the item definitions.", childItemType);
				throw new ZeusException(
					string.Format(
						"No item of the type {0} was found among item definitions. This could happen if the referenced item type an abstract class or a class that doesn't inherit from Zeus.ContentItem.",
						childItemType),
					ex);
			}
			child.Name = DefaultChildName;
			child.ZoneName = DefaultChildZoneName;
			child.AddTo(item);
			return child;
		}

		#endregion
	}
}