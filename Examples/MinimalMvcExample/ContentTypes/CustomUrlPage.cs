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
using Zeus.ContentProperties;
using Zeus.Templates.ContentTypes;
using Zeus.FileSystem.Images;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("Custom Url Page")]
    [RestrictParents(typeof(CustomUrlContainer))]
    [Panel("Images", "Images", 340)]
    [AllowedChildren(Types = new[] { typeof(Page), typeof(CroppedImage), typeof(CroppedImage) })]
    public class CustomUrlPage : BasePage, ParentWithCroppedImageValues
	{
        public override bool IgnoreOrderOnSave
        {
            get
            {
                return true;
            }
        }

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
        [ChildEditor("Banner", 200, arg1="400", arg2="200")]
        public Zeus.FileSystem.Images.HiddenCroppedImage Banner
        {
            get { return GetChild("Banner") as Zeus.FileSystem.Images.HiddenCroppedImage; }
            set
            {
                if (value != null)
                {
                    value.Name = "Banner";
                    value.AddTo(this);
                }
            }
        }
        
        [ContentProperty("MyPage", 200)]
        [LinkedItemDropDownListEditor("MyPage", 200, TypeFilter=typeof(MyLittleType))]
        public virtual MyLittleType MyPage
        {
            get { return GetDetail<MyLittleType>("MyPage", (default(MyLittleType))); }
            set { SetDetail("MyPage", value); }
        }

        [ContentProperty("Vegetable", 300)]
        public virtual Vegetable Vegetable
        {
            get { return GetDetail("Vegetable", Vegetable.Potato); }
            set { SetDetail("Vegetable", value); }
        }

        [XhtmlStringContentProperty("Tiny MCE Content", 350)]
        public virtual string TinyMCEContent
        {
            get { return GetDetail("TinyMCEContent", string.Empty); }
            set { SetDetail("TinyMCEContent", value); }
        }

        public override bool UseProgrammableSEOAssets
        {
            get
            {
                return true;
            }
        }

        public override string UseProgrammableSEOAssetsExplanation
        {
            get
            {
                return "no SEO assets cos you suck balls!";
            }
        }

        public override string ProgrammableHtmlTitle
        {
            get
            {
                return "moo";
            }
        }

        public override string ProgrammableMetaDescription
        {
            get
            {
                return "This is a programmatical desc";
            }
        }


        #region ParentWithCroppedImageValues Members

        public int Width
        {
            get { return 300; }
        }

        public int Height
        {
            get { return 200; }
        }

        #endregion
    }
}
