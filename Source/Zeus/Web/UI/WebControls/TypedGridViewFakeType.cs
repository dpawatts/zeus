using System;
using System.Reflection;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	internal class TypedGridViewFakeType : TypeDelegator
	{
		public TypedGridViewFakeType(Type dataItemType)
			: base(typeof(RealTypedGridView<>).MakeGenericType(dataItemType))
		{
			
		}
	}
}