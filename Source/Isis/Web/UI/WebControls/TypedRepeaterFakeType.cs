using System;
using System.Reflection;
using Isis.Reflection;
using System.Web.UI;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	internal class TypedRepeaterFakeType : TypeDelegator
	{
		private Type repeaterItemType;

		public TypedRepeaterFakeType(Type dataItemType)
			: base(typeof(TypedRepeater<>).MakeGenericType(dataItemType))
		{
			this.repeaterItemType = typeof(TypedRepeaterItem<>).MakeGenericType(dataItemType);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			PropertyInfo info = base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);

			if (name == "ItemTemplate")
				info = new FakePropertyInfo(info, this.repeaterItemType);

			return info;
		}
	}
}
