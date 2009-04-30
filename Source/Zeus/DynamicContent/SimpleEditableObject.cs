using System;
using Isis.ExtensionMethods;
using Zeus.ContentProperties;
using Zeus.ContentTypes;

namespace Zeus.DynamicContent
{
	public class SimpleEditableObject : IEditableObject
	{
		private readonly object _objectToWrap;

		public SimpleEditableObject(object objectToWrap)
		{
			_objectToWrap = objectToWrap;
		}

		public object this[string detailName]
		{
			get { return _objectToWrap.GetValue(detailName); }
			set { _objectToWrap.SetValue(detailName, value, true); }
		}

		public object WrappedObject
		{
			get { return _objectToWrap; }
		}

		public PropertyCollection GetDetailCollection(string name, bool create)
		{
			throw new NotSupportedException();
		}

		public object GetDetail(string name)
		{
			return this[name];
		}

		public void SetDetail(string name, object value)
		{
			this[name] = value;
		}
	}
}