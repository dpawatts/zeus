using System;
using System.Reflection;
using Isis.Reflection;

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
			get { return this.UnderlyingProperty.GetCustomAttribute<IEditor>(false, false); }
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
