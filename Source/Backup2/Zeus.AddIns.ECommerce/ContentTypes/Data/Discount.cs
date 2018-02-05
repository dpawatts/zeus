using Zeus;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using Zeus.Integrity;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using System;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
    [ContentType("Discount Voucher")]
    [RestrictParents(typeof(DiscountContainer))]
    public class Discount : BaseContentItem
    {
        [Zeus.ContentProperty("Valid From", 100)]
		public virtual DateTime ValidFrom
		{
			get { return GetDetail("ValidFrom", new DateTime(2012, 1,1)); }
			set { SetDetail("ValidFrom", value); }
		}

        [Zeus.ContentProperty("Valid To", 110)]
		public virtual DateTime ValidTo
		{
            get { return GetDetail("ValidTo", new DateTime(2112, 1, 1)); }
			set { SetDetail("ValidTo", value); }
		}

        [Zeus.ContentProperty("Coupon Code", 120)]
		public virtual string CouponCode
		{
			get { return GetDetail("CouponCode", default(string)); }
			set { SetDetail("CouponCode", value); }
		}

        [Zeus.ContentProperty("Discount Is Flat Amount", 130, Description="Leave this box unchecked if the discount is a percentage rather than a flat amount")]
		public virtual bool DiscountIsFlatAmount
		{
			get { return GetDetail("DiscountIsFlatAmount", default(bool)); }
			set { SetDetail("DiscountIsFlatAmount", value); }
		}

        [Zeus.ContentProperty("Flat Discount Amount", 140)]
		public virtual decimal FlatDiscountAmount
		{
			get { return GetDetail("FlatDiscountAmount", default(decimal)); }
			set { SetDetail("FlatDiscountAmount", value); }
		}

        [Zeus.ContentProperty("Percentage Discount Amount", 150)]
        public virtual decimal PercentageDiscountAmount
		{
			get { return GetDetail("PercentageDiscountAmount", default(decimal)); }
			set { SetDetail("PercentageDiscountAmount", value); }
		}

        [Zeus.ContentProperty("Maximum Uses", 160, Description="Leave this as zero if there is no limit")]
		public virtual int MaxGlobalUses
		{
			get { return GetDetail("MaxGlobalUses", default(int)); }
			set { SetDetail("MaxGlobalUses", value); }
        }

        public virtual int UsesSoFar
        {
            get { return GetDetail("UsesSoFar", 0); }
            set { SetDetail("UsesSoFar", value); }
        }

        public decimal ProcessPrice(decimal sourcePrice)
        {
            return sourcePrice - DiscountAmount(sourcePrice) > 0m ? sourcePrice - DiscountAmount(sourcePrice) : 0m;
        }

        public decimal DiscountAmount(decimal sourcePrice)
        {
            if (DiscountIsFlatAmount)
                return FlatDiscountAmount;
            else
                return Math.Round(sourcePrice * ((decimal)PercentageDiscountAmount / (decimal)100), 2);            
        }

        public string DescriptionForAdministrators
        {
            get {
                string res = "Code was " + CouponCode + " which activated Discount named " + Title;
                if (DiscountIsFlatAmount)
                    res += ", a flat discount";
                else
                    res += ", a % discount of " + PercentageDiscountAmount + "%";
                return res;
            }

        }

    }
}
