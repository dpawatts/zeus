using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Isis.ExtensionMethods;
using Isis.Linq;
using NHibernate.Linq;
using Zeus.ContentProperties;
using Zeus.Persistence.Specifications;

namespace Zeus.Persistence.NH
{
	public class ItemFinder : Finder<ContentItem>
	{
		#region Constructor

		public ItemFinder(ISessionProvider sessionProvider)
			: base(sessionProvider)
		{
			
		}

		#endregion

		#region IFinder<ContentItem> Members

		public override IQueryable<TResult> FindBySpecification<TResult>(ISpecification<TResult> specification)
		{
			// Pre-process the expression tree to look for properties marked with the DetailAttribute attribute.
			DetailPropertyModifier detailPropertyModifier = new DetailPropertyModifier();
			Expression<Func<TResult, bool>> modifiedExpression = (Expression<Func<TResult, bool>>) detailPropertyModifier.Modify(specification.Predicate);

			return FindAll<TResult>().Where(modifiedExpression);
		}

		#endregion

		#region DetailPropertyModifier class

		private class DetailPropertyModifier : ExpressionVisitor
		{
			public Expression Modify(Expression expression)
			{
				return Visit(expression);
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
				// ci => ci.Details.OfType<StringDetail>().Any(cd => cd.Name == "MyProperty" && cd.StringValue.StartsWith("Hello"))

				// Look for a binary expression where the LHS is a property call and the property has the
				// DetailAttribute defined.
				if (b.Left is MemberExpression && ((MemberExpression) b.Left).Member.IsDefined(typeof(IContentProperty), true))
				{
					MemberExpression leftMemberExpression = (MemberExpression) b.Left;
					throw new NotImplementedException();
					/*// Get the type that NHibernate uses to map this key/value pair. For example, if c.MyProperty
					// is a string, it will use the StringDetail class.
					Type detailType = ContentDetail.GetDetailType(leftMemberExpression.Type);

					// Make a parameter expression using the detail type.
					ParameterExpression contentDetailParameter = Expression.Parameter(detailType, "cd");

					// Make an expression for a call to the Details property.
					Expression detailsProperty = Expression.Property(leftMemberExpression.Expression, "Details");

					// This is the property that is mapped to the database column. In our case, we store each
					// value type in its own column. In other words the key/value table contains columns for
					// IntValue, DoubleValue, DateTimeValue, etc. We have classes to represent each of these types,
					// which NHibernate knows how to persist. For example, in the StringDetail class, the StringValue
					// property is persisted to the StringValue column in the database. StringValue is an example
					// of the property we are getting with the following expression.
					Expression propertyValueProperty = Expression.Property(contentDetailParameter,
																																 ContentDetail.GetPropertyName(leftMemberExpression.Type));

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
					                       			b.Right,
					                       			false,
					                       			b.Method
					                       			)
					                       		),
					                       	contentDetailParameter
					                       	)
						);*/
				}
				return base.VisitBinary(b);
			}
		}

		#endregion
	}
}
