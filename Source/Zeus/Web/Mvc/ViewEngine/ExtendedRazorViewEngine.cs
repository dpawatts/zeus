using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Zeus.Web.Mvc.ViewEngine
{
    public class ExtendedRazorViewEngine : RazorViewEngine
    {
        public ExtendedRazorViewEngine()
        {
            this.AreaMasterLocationFormats = this.AreaPartialViewLocationFormats = this.AreaViewLocationFormats = this.AreaViewLocationFormats.Union(
                new string[]
                {
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                }).ToArray();
        }
    }
}