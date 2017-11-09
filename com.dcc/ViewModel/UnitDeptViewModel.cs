using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.ViewModel
{
    public class UnitDeptViewModel
    {
        public UnitDeptViewModel()
        {
            this.Depts = new Department();

            this._Unit = new Unit();
        }
        public Department Depts { get; set; }

        public Unit _Unit { get; set; }
    }

    public class MarkAttendanceViewModel
    {
        public int attendance { get; set; }
        public string group { get; set; }
        public int userId { get; set; }
    }
}