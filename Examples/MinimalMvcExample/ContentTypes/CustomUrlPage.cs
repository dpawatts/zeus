using Zeus;
using Zeus.Web;
using Zeus.Integrity;
using Zeus.Design.Editors;
using System.Collections.Generic;
using Zeus.Web.UI;
using Zeus.ContentTypes;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Examples.MinimalMvcExample.Design.Editors;
using Zeus.Examples.MinimalMvcExample.Enums;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("Custom Url Page")]
    [RestrictParents(typeof(CustomUrlContainer))]
    [FieldSet("BannerFS", "Banner Image", 300, ContainerName = "Content")]
    [AllowedChildren(Types = new[] { typeof(Zeus.FileSystem.Images.Image) })]
	public class CustomUrlPage : PageContentItem
	{
        public override string Url
        {
            get
            {
                return this.Parent.Url + "/" + "moomooBabyBoo" + "/" + this.Title.ToSafeUrl();;
            }
        }

        public override bool HasCustomUrl
        {
            get
            {
                return true;
            }
        }

        [ContentProperty("Selection Of Pages", 110)]
        [PageListBoxEditor("Selection Of Pages", 110)]
        public IList<int> PagesSelection
        {
            get { return GetDetail<IList<int>>("PagesSelection", new List<int>()); }
            set { SetDetail("PagesSelection", value); }
        }


        /// <summary>
        /// Banner image
        /// </summary>
        [ContentProperty("Banner", 200)]
        [ChildEditor("Banner", 200, ContainerName="BannerFS")]
        public Zeus.FileSystem.Images.Image Banner
        {
            get { return GetChild("Banner") as Zeus.FileSystem.Images.Image; }
            set
            {
                if (value != null)
                {
                    value.Name = "Banner";
                    value.AddTo(this);
                }
            }
        }

        [ContentProperty("Vegetable", 300)]
        public virtual Vegetable Vegetable
        {
            get { return GetDetail("Vegetable", Vegetable.Potato); }
            set { SetDetail("Vegetable", value); }
        }
	}
}
