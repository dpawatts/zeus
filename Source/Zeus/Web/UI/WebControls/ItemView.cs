using System;
using System.Web.UI.WebControls;
using Zeus.Design.Editors;
using Zeus.Engine;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Collections.Generic;
using Zeus.Admin;

namespace Zeus.Web.UI.WebControls
{
	public abstract class ItemView : WebControl, INamingContainer, IEditableObjectEditor, IContentItemContainer
	{
		private ITypeDefinition _currentTypeDefinition;
		private IEditableObject _currentItem;

		public event EventHandler<ItemViewEditableObjectEventArgs> ItemCreating;
		public event EventHandler<ItemViewTypeDefinitionEventArgs> DefinitionCreating;

		#region Properties

		public IDictionary<string, Control> PropertyControls
		{
			get;
			private set;
		}

		/// <summary>Gets or sets the item to edit with this form.</summary>
		public virtual IEditableObject CurrentItem
		{
			get
			{
				if (_currentItem == null)
				{
					ItemViewEditableObjectEventArgs args = new ItemViewEditableObjectEventArgs(null);
					OnItemCreating(args);
					_currentItem = args.AffectedItem;
				}
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				if (value != null)
				{
					ChildControlsCreated = false;
					EnsureChildControls();
					OnCurrentItemChanged(EventArgs.Empty);
				}
			}
		}

		protected virtual void OnItemCreating(ItemViewEditableObjectEventArgs args)
		{
			if (ItemCreating != null)
				ItemCreating(this, args);
		}

		protected virtual void OnDefinitionCreating(ItemViewTypeDefinitionEventArgs args)
		{
			if (DefinitionCreating != null)
				DefinitionCreating(this, args);
		}

		protected virtual void OnCurrentItemChanged(EventArgs eventArgs)
		{
			
		}

		public ITypeDefinition CurrentItemDefinition
		{
			get
			{
				if (_currentTypeDefinition == null)
				{
					if (CurrentItem != null && CurrentItem is ContentItem)
					{
						_currentTypeDefinition = Zeus.Context.ContentTypes.GetContentType(CurrentItem.GetType());
					}
					else
					{
						ItemViewTypeDefinitionEventArgs args = new ItemViewTypeDefinitionEventArgs(null);
						OnDefinitionCreating(args);
						_currentTypeDefinition = args.TypeDefinition;
					}
				}
				return _currentTypeDefinition;
			}
			set
			{
				_currentTypeDefinition = value;
			}
		}

		#endregion

		#region Methods

		/*protected override void OnLoad(EventArgs e)
		{
			EnsureChildControls();
			base.OnLoad(e);
		}*/

		protected override void CreateChildControls()
		{
			// Get ItemDefinition for current type.
			PropertyControls = new Dictionary<string, Control>();
			AddPropertyControls();

			base.CreateChildControls();
		}

		protected abstract void AddPropertyControls();

		#endregion

		#region IContentItemContainer Members

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return (ContentItem) CurrentItem; }
		}

		#endregion
	}
}
