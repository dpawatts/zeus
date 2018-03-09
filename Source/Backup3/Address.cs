using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeus.AddIns.ECommerce.PaypalExpress
{
    public class Address
    {
        public string PersonTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        private string _addressLine1;
        public string AddressLine1
        {
            get { return _addressLine1 ?? string.Empty; }
            set { _addressLine1 = value; }
        }

        private string _addressLine2;
        public string AddressLine2
        {
            get { return _addressLine2 ?? string.Empty; }
            set { _addressLine2 = value; }
        }

        public string StateRegion { get; set; }
        public string TownCity { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
