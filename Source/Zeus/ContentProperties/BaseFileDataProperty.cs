using System;
using Zeus.FileSystem;

namespace Zeus.ContentProperties
{
	public abstract class BaseFileDataProperty<TFileData> : ObjectProperty
		where TFileData : FileData
	{
		#region Constuctors

		protected BaseFileDataProperty()
		{
		}

		protected BaseFileDataProperty(ContentItem containerItem, string name, TFileData value)
			: base(containerItem, name, value)
		{
			
		}

		#endregion

		#region Properties

		public virtual TFileData FileDataValue { get; set; }

		public override object Value
		{
			get { return FileDataValue; }
			set { FileDataValue = (TFileData) value; }
		}

		public override Type ValueType
		{
			get { return typeof(TFileData); }
		}

		#endregion
	}
}