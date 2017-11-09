using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.ViewModel
{
    public class DuesReportViewModel
    {
        public DuesReportViewModel()
        {
            this._MONTHS = new List<Month>();
            this._MEMBERS = new List<Member>();
            this._PAYMENTS = new List<Payment>();
        }
        public List<Month> _MONTHS { get; set; }
        public List<Member> _MEMBERS { get; set; }
        public List<Payment> _PAYMENTS { get; set; }
    }
}