using System;
using System.Reflection;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	internal class TypedListViewFakeType : TypeDelegator
	{
		private Type listViewItemType;

		public TypedListViewFakeType(Type dataItemType)
			: base(typeof(TypedListView<>).MakeGenericType(dataItemType))
		{
			this.listViewItemType = typeof(TypedListViewDataItem<>).MakeGenericType(dataItemType);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			PropertyInfo info = base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);

			if (name == "ItemTemplate")
				info = new FakePropertyInfo(info, this.listViewItemType);

			return info;
		}
	}
}