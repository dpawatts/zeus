using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Zeus.BaseLibrary.ExtensionMethods.Collections
{
    public static class NameValueCollectionExtensions
    {
        public static string Join(this NameValueCollection collection, Func<string, string> selector, string separator)
        {
            return String.Join(separator, collection.Cast<string>().Select(e => selector(e)).ToArray());
        }
    }
}
