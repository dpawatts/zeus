using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spark;

namespace Zeus.Web.Mvc.ViewModels
{
    public class SignalData {
        public bool ChangeSignalFired { get; set; }
        public CacheSignal _allDataSignal;

        public ICacheSignal GetSignalForContentID
        {
            get
            {
                return _allDataSignal;
            }
        }
    }
}
