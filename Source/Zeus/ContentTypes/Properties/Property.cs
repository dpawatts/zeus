using System;
using System.Reflection;
using Isis.ExtensionMethods.Reflection;

namespace Zeus.ContentTypes.Properties
{
	public class Property
	{
		#region Constructor

		public Property(PropertyInfo underlyingProperty)
		{
			this.Name = underlyingProperty.Name;
			this.UnderlyingProperty = underlyingProperty;
		}

		#endregion

		#region Properties

		public string Name
		{
			get;
			private set;
		}

		public PropertyInfo UnderlyingProperty
		{
			get;
			private set;
		}

		public bool HasEditor
		{
			get { return (this.Editor != null); }
		}

		public IEditor Editor
		{
			get
			{
				IEditor editor = this.UnderlyingProperty.GetCustomAttribute<IEditor>(false, false);
				if (editor != null)
					editor.Name = this.Name;
				return editor;
			}
		}

		public bool HasDisplayer
		{
			get { return (this.Displayer != null); }
		}

		public IDisplayer Displayer
		{
			get { return this.UnderlyingProperty.GetCustomAttribute<IDisplayer>(false, false); }
		}

		#endregion
	}
}
