using System;
using System.Reflection;
using Isis.Reflection;
using System.Web.UI;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	internal class ConditionalViewFakeType : TypeDelegator
	{
		private Type formViewType;

		public ConditionalViewFakeType(Type dataItemType)
			: base(typeof(ConditionalView))
		{
			this.formViewType = typeof(ConditionalViewItem<>).MakeGenericType(dataItemType);
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
