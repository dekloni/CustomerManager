using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerManager.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// it could be either cash, paypal, or any other way of payment
        /// </summary>
        public string PaymentType { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}