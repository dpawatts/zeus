using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Zeus.Examples.MinimalMvcExample.Enums
{
    public enum Vegetable
    {
        [Description("Potato")]
        Potato,

        [Description("Runner Beans")]
        RunnerBeans,

        [Description("Red Onions")]
        RedOnions
    }
}
