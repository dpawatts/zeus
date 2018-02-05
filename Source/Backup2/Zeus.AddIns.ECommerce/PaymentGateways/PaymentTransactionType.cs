using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeus.AddIns.ECommerce.PaymentGateways
{
    /// <summary>
    /// Payment transaction types
    /// </summary>
    public enum PaymentTransactionType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        None = 0,

        /// <summary>
        /// Payment
        /// </summary>
        Payment,

        /// <summary>
        /// Deferred
        /// </summary>
        Deferred,

        /// <summary>
        /// Repeat
        /// </summary>
        Repeat,

        /// <summary>
        /// Authenticate
        /// </summary>
        Authenticate,

        /// <summary>
        /// Authorise
        /// </summary>
        Authorise,

        /// <summary>
        /// Refund
        /// </summary>
        Refund,

        /// <summary>
        /// Void
        /// </summary>
        Void,

        /// <summary>
        /// Release
        /// </summary>
        Release,

        /// <summary>
        /// Abort
        /// </summary>
        Abort,

        /// <summary>
        /// Cancel
        /// </summary>
        Cancel
    }
}
