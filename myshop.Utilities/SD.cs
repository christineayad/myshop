using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Utilities
{
    public static class SD
    {
        public  const string AdminRole = "Admin";
        public static string EditorRole = "Editor";
        public static string CustomerRole = "Customer";

		//Order Header 
		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		//public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "Rejected";

		//3-Add SessionKey
		public const string SessionKey = "SessionShoppingCart";
	}
}
