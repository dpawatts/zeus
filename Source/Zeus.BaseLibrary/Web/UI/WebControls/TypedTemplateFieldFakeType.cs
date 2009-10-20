using System;
using System.Reflection;

namespace Isis.Web.UI.WebControls
{
	internal class TypedTemplateFieldFakeType : TypeDelegator
	{
		private Type formViewType;

		public TypedTemplateFieldFakeType(Type dataItemType)
			: base(typeof(TypedTemplateField))
		{
			this.formViewType = typeof(TypedGridViewRow<>).MakeGenericType(dataItemType);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			PropertyInfo info = base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);

			if (name == "ItemTemplate")
				info = new FakePropertyInfo(info, this.formViewType);

			return info;
		}
	}
}