using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ext.Net;
using Ext.Net.Utilities;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Integrity;
using Zeus.Web.Hosting;

namespace Zeus
{
	/// <summary>
	/// Mixed utility functions used by Zeus.
	/// </summary>
	public static class Utility
	{
		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		public static object Convert(object value, Type destinationType)
		{
			if (value != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
					return converter.ConvertFrom(value);
				converter = TypeDescriptor.GetConverter(value.GetType());
				if (converter != null && converter.CanConvertTo(destinationType))
					return converter.ConvertTo(value, destinationType);
				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);
				if (!destinationType.IsAssignableFrom(value.GetType()))
				{
					if (!(value is IConvertible))
						throw new ZeusException("Cannot convert object of type '{0}' because it does not implement IConvertible", value.GetType());
					if (destinationType.IsNullable())
					{
						NullableConverter nullableConverter = new NullableConverter(destinationType);
						destinationType = nullableConverter.UnderlyingType;
					}
					return System.Convert.ChangeType(value, destinationType);
				}
			}
			return value;
		}

		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T Convert<T>(object value)
		{
			return (T) Convert(value, typeof(T));
		}

		public static Func<DateTime> CurrentTime = () => DateTime.Now;

		/// <summary>Tries to find a property matching the supplied expression, returns null if no property is found with the first part of the expression.</summary>
		/// <param name="item">The object to query.</param>
		/// <param name="expression">The expression to evaluate.</param>
		public static object Evaluate(object item, string expression)
		{
			return item.GetValue(expression);
		}

		/// <summary>Evaluates an expression and applies a format string.</summary>
		/// <param name="item">The object to query.</param>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="format">The format string to apply.</param>
		/// <returns>The formatted result ov the evaluation.</returns>
		public static string Evaluate(object item, string expression, string format)
		{
			return string.Format(format, Evaluate(item, expression));
		}

		/// <summary>Gets a value from a property.</summary>
		/// <param name="instance">The object whose property to get.</param>
		/// <param name="propertyName">The name of the property to get.</param>
		/// <returns>The value of the property.</returns>
		public static object GetProperty(object instance, string propertyName)
		{
			if (instance == null) throw new ArgumentNullException("instance");
			if (propertyName == null) throw new ArgumentNullException("propertyName");

			Type instanceType = instance.GetType();
			PropertyInfo pi = instanceType.GetProperty(propertyName);

			if (pi == null)
				throw new ZeusException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);

			return pi.GetValue(instance, null);
		}

		public static string GetSafeName(string value)
		{
			string result = value.ToLower();
			result = Regex.Replace(result, "[ ]+", "-");
			result = Regex.Replace(result, "[^a-zA-Z0-9_-]", string.Empty);
			return result;
		}

		/// <summary>Checks that the destination isn't below the source.</summary>
		private static bool IsDestinationBelowSource(ContentItem source, ContentItem destination)
		{
			if (source == destination)
				return true;
			foreach (ContentItem ancestor in Find.EnumerateParents(destination))
				if (ancestor == source)
					return true;
			return false;
		}

		/// <summary>Inserts an item among a parent item's children using a comparer to determine the location.</summary>
		/// <param name="item">The item to insert.</param>
		/// <param name="newParent">The parent item.</param>
		/// <param name="sortExpression">The sort expression to use.</param>
		/// <returns>The index of the item among it's siblings.</returns>
		public static int Insert(ContentItem item, ContentItem newParent, string sortExpression)
		{
			return Insert(item, newParent, new Collections.ItemComparer(sortExpression));
		}

		/// <summary>Inserts an item among a parent item's children using a comparer to determine the location.</summary>
		/// <param name="item">The item to insert.</param>
		/// <param name="newParent">The parent item.</param>
		/// <param name="comparer">The comparer to use.</param>
		/// <returns>The index of the item among it's siblings.</returns>
		public static int Insert(ContentItem item, ContentItem newParent, IComparer<ContentItem> comparer)
		{
			if (item.Parent != null && item.Parent.Children.Contains(item))
				item.Parent.Children.Remove(item);

			item.Parent = newParent;
			if (newParent != null)
			{
				if (IsDestinationBelowSource(item, newParent))
					throw new DestinationOnOrBelowItselfException(item, newParent);

				IList<ContentItem> siblings = newParent.Children;
				for (int i = 0; i < siblings.Count; i++)
				{
					if (comparer.Compare(item, siblings[i]) < 0)
					{
						siblings.Insert(i, item);
						return i;
					}
				}
				siblings.Add(item);
				return siblings.Count - 1;
			}
			return -1;
		}

		/// <summary>Moves an item in a list to a new index.</summary>
		/// <param name="siblings">A list of items where the item to move is listed.</param>
		/// <param name="itemToMove">The item that should be moved (must be in the list)</param>
		/// <param name="newIndex">The new index onto which to place the item.</param>
		/// <remarks>To persist the new ordering one should call <see cref="Utility.UpdateSortOrder"/> and save the returned items. If the items returned from the <see cref="ContentItem.GetChildren"/> are moved with this method the changes will not be persisted since this is a new list instance.</remarks>
		public static void MoveToIndex(IList<ContentItem> siblings, ContentItem itemToMove, int newIndex)
		{
			siblings.Remove(itemToMove);
			siblings.Insert(newIndex, itemToMove);
		}

		/// <summary>Iterates items and ensures that the item's sort order is ascending.</summary>
		/// <param name="siblings">The items to iterate.</param>
		/// <returns>A list of items whose sort order was changed.</returns>
		public static IEnumerable<ContentItem> UpdateSortOrder(IEnumerable siblings)
		{
			List<ContentItem> updatedItems = new List<ContentItem>();
			int lastSortOrder = int.MinValue;
			foreach (ContentItem sibling in siblings)
			{
				if (sibling.SortOrder <= lastSortOrder)
				{
					sibling.SortOrder = ++lastSortOrder;
					updatedItems.Add(sibling);
				}
				else
					lastSortOrder = sibling.SortOrder;
			}
			return updatedItems;
		}

		/// <summary>Invokes an event and and executes an action unless the event is cancelled.</summary>
		/// <param name="handler">The event handler to signal.</param>
		/// <param name="item">The item affected by this operation.</param>
		/// <param name="sender">The source of the event.</param>
		/// <param name="finalAction">The default action to execute if the event didn't signal cancel.</param>
		public static void InvokeEvent(EventHandler<CancelItemEventArgs> handler, ContentItem item, object sender, Action<ContentItem> finalAction)
		{
			if (handler != null && item.VersionOf == null)
			{
				CancelItemEventArgs args = new CancelItemEventArgs(item, finalAction);

				handler.Invoke(sender, args);

				if (!args.Cancel)
					args.FinalAction(args.AffectedItem);
			}
			else
				finalAction(item);
		}

		/// <summary>Invokes an event and and executes an action unless the event is cancelled.</summary>
		/// <param name="handler">The event handler to signal.</param>
		/// <param name="source">The item affected by this operation.</param>
		/// <param name="destination">The destination of this operation.</param>
		/// <param name="sender">The source of the event.</param>
		/// <param name="finalAction">The default action to execute if the event didn't signal cancel.</param>
		/// <returns>The result of the action (if any).</returns>
		public static ContentItem InvokeEvent(EventHandler<CancelDestinationEventArgs> handler, object sender, ContentItem source, ContentItem destination, Func<ContentItem, ContentItem, ContentItem> finalAction)
		{
			if (handler != null && source.VersionOf == null)
			{
				CancelDestinationEventArgs args = new CancelDestinationEventArgs(source, destination, finalAction);

				handler.Invoke(sender, args);

				if (args.Cancel)
					return null;

				return args.FinalAction(args.AffectedItem, args.Destination);
			}

			return finalAction(source, destination);
		}

		public static string GetUrlSafeString(string value)
		{
			value = value.ToLower().Replace(' ', '-');
			return Regex.Replace(value, @"[^a-zA-Z0-9\-]", string.Empty);
		}

		public static string GetCooliteIconUrl(Icon icon)
		{
			string iconResourceName = string.Format(ResourceManager.ASSEMBLYSLUG + ".icons.{0}", StringUtils.ToCharacterSeparatedFileName(icon.ToString(), '_', "png"));
			return WebResourceUtility.GetUrl(typeof(ResourceManager), iconResourceName);
		}

		public static string GetClientResourceUrl(Assembly assembly, string relativePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetClientResourceUrl(assembly, relativePath);
		}

		public static string GetClientResourceUrl(Type type, string relativePath)
		{
			return GetClientResourceUrl(type.Assembly, relativePath);
		}
	}
}
