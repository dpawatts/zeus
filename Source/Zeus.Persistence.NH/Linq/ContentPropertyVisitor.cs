using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Linq.Visitors;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentProperties;
using Zeus.BaseLibrary.Linq;

namespace Zeus.Persistence.NH.Linq
{
	internal class ContentPropertyVisitor : Zeus.BaseLibrary.Linq.ExpressionVisitor
	{
		private readonly IContentPropertyManager _contentPropertyManager;

		public ContentPropertyVisitor(IContentPropertyManager contentPropertyManager)
		{
			_contentPropertyManager = contentPropertyManager;
		}

		protected override Expression VisitBinary(BinaryExpression b)
		{
			// Currently, this will only deal with binary expressions, i.e. c => c.MyProperty == "MyValue".
			// It will NOT deal with method calls on properties, i.e. c => c.MyProperty.StartsWith("My").
			// It also expects the property call to be on the left (i.e. c.MyProperty),
			// and the filter value on the right (i.e. "MyValue".

			// As an example, we are looking to replace this:
			// ci => ci.MyProperty == "Hello"
			// with this:
			// ci => ci.Details.OfType<StringDetail>().Any(cd => cd.Name == "MyProperty" && cd.StringValue == "Hello")

			// Look for a binary expression where the LHS is a property call and the property has the
			// DetailAttribute defined.
			if (b.Left is MemberExpression && ((MemberExpression) b.Left).Member.IsDefined(typeof(IContentProperty), true))
			{
				MemberExpression leftMemberExpression = (MemberExpression) b.Left;

				// Get the type that NHibernate uses to map this key/value pair. For example, if c.MyProperty
				// is a string, it will use the StringDetail class.
				Type detailType = _contentPropertyManager.GetDefaultPropertyDataType(leftMemberExpression.Type);

				// Make a parameter expression using the detail type.
				ParameterExpression contentDetailParameter = Expression.Parameter(detailType, "cd");

				// Make an expression for a call to the Details property.
				Expression detailsProperty = Expression.Property(leftMemberExpression.Expression, "DetailsInternal");

				// This is the property that is mapped to the database column. In our case, we store each
				// value type in its own column. In other words the key/value table contains columns for
				// IntValue, DoubleValue, DateTimeValue, etc. We have classes to represent each of these types,
				// which NHibernate knows how to persist. For example, in the StringDetail class, the StringValue
				// property is persisted to the StringValue column in the database. StringValue is an example
				// of the property we are getting with the following expression.
				Expression propertyValueProperty = Expression.Property(contentDetailParameter,
					_contentPropertyManager.CreatePropertyDataObject(leftMemberExpression.Type).ColumnName);

				// Make an expression for a call to the generic Enumerable.OfType method.
				Expression ofTypeCall = Expression.Call(null,
					typeof(Enumerable).GetMethod("OfType").MakeGenericMethod(detailType),
					detailsProperty);

				// Get a instance of the generic Enumerable.Any method, using the relevant type parameters.
				MethodInfo anyMethodInfo = typeof(Enumerable).GetGenericMethod("Any",
					new[] { detailType },
					new[] { typeof(IEnumerable<>).MakeGenericType(detailType), typeof(Func<,>).MakeGenericType(detailType, typeof(bool)) },
					BindingFlags.Static);

				// Create and return the expression that will replace the original expression.
				return Expression.Call(null,
					anyMethodInfo,
					ofTypeCall,
					Expression.Lambda(
						typeof(Func<,>).MakeGenericType(detailType, typeof(bool)),
						Expression.AndAlso(
							// cd.Name == "MyProperty"
							Expression.Equal(
								Expression.Property(contentDetailParameter, "Name"),
								Expression.Constant(leftMemberExpression.Member.Name, typeof(string)),
								false,
								typeof(string).GetMethod("op_Equality")
								),
							// cd.StringValue == "Hello"
							Expression.MakeBinary(
								b.NodeType,
								propertyValueProperty,
								GetDetailExpression(b.Right),
								false,
								b.Method
								)
							),
						contentDetailParameter
						)
					);
			}
			return base.VisitBinary(b);
		}

		/// <summary>
		/// For LinkDetail expressions, returns a property call to the ID property.
		/// For other types, returns whatever is passed in.
		/// </summary>
		/// <returns></returns>
		private static Expression GetDetailExpression(Expression expression)
		{
			return expression;
		}
	}
}