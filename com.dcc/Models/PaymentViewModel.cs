using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.Models
{
    public class PaymentViewModel
    {
        public string DueType { get; set; }
        public decimal Amount { get; set; }
        public string Month { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionType { get; set; }
        public string Year { get; set; }
        public string PaymentStatus { get; set; }
    }
}