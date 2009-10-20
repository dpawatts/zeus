using System;
using System.Reflection;
using Isis.Reflection;
using System.Web.UI;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	internal class TypedExtendedFormViewFakeType : TypeDelegator
	{
		private Type formViewType;

		public TypedExtendedFormViewFakeType(Type dataItemType)
			: base(typeof(TypedExtendedFormView<>).MakeGenericType(dataItemType))
		{
			this.formViewType = typeof(TypedExtendedFormView<>).MakeGenericType(dataItemType);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			PropertyInfo info = base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);

			if (name == "ItemTemplate" || name == "InsertItemTemplate" || name == "EditItemTemplate" || name == "InsertEditItemTemplate")
				info = new FakePropertyInfo(info, this.formViewType);

			return info;
		}
	}
}
