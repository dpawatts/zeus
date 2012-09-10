using Zeus;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using Zeus.Integrity;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using System;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
    [ContentType("Folder for Discounts")]
    [RestrictParents(typeof(WebsiteNode))]
    [ContentTypeAuthorizedRoles("Administrators")]
    public class DiscountContainer : BaseContentItem
    {
    }
}
