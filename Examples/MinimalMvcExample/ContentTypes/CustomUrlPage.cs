using Zeus;
using Zeus.Web;
using Zeus.Integrity;
using Zeus.Design.Editors;
using System.Collections.Generic;
using Zeus.Web.UI;
using Zeus.ContentTypes;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("Custom Url Page")]
    [RestrictParents(typeof(CustomUrlContainer))]
	public class CustomUrlPage : PageContentItem
	{
        public override string Url
        {
            get
            {
                return this.Parent.Url + "/" + "moomooBabyBoo" + "/" + this.Title.ToSafeUrl();;
            }
        }
	}
}
