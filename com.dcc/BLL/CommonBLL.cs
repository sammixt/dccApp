using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.BLL
{
    public class CommonBLL
    {
        public static List<Post> GetAllPost()
        {
            List<Post> post = new List<Post>();
            try
            {
                using (var context = new DCCEntities())
                {
                    post = context.Posts.ToList();
                    return post;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Role> GetAllROle()
        {
            List<Role> role = new List<Role>();
            try
            {
                using (var context = new DCCEntities())
                {
                    role = context.Roles.ToList();
                    return role;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static string CountBelivers
        {
            get
            {
                using(var context = new DCCEntities())
                {
                    int TotalBelievers = context.Believers.Count();
                    string _TotalBelievers = Convert.ToString(TotalBelievers);
                    return _TotalBelievers;
                }
            }
        }

        public static string CountMembers
        {
            get
            {
                using (var context = new DCCEntities())
                {
                    int TotalMembers = context.Members.Select(b => b.BelieverId).Distinct().Count();
                    string _TotalMembers = Convert.ToString(TotalMembers);
                    return _TotalMembers;
                }
            }
        }

        public static string CountDepts
        {
            get
            {
                using (var context = new DCCEntities())
                {
                    int TotalDepts = context.Departments.Count();
                    string _TotalDepts = Convert.ToString(TotalDepts);
                    return _TotalDepts;
                }
            }
        }

        public static string CountUnit
        {
            get
            {
                using (var context = new DCCEntities())
                {
                    int TotalUnits = context.Units.Count();
                    string _TotalUnits = Convert.ToString(TotalUnits);
                    return _TotalUnits;
                }
            }
        }

       /**Count Members and Units with department ID**/

        public static string CountMembersInDept(string DeptId)
        {
            int _DeptId = Convert.ToInt32(DeptId);
                using (var context = new DCCEntities())
                {
                    int TotalMembers = context.Members.Where(m => m.DeptId == _DeptId).Count();
                    string _TotalMembers = Convert.ToString(TotalMembers);
                    return _TotalMembers;
                }    
        }

        public static string CountUnitInDept(string DeptId)
        {
            int _DeptId = Convert.ToInt32(DeptId);
            using (var context = new DCCEntities())
            {
                int TotalUnits = context.Units.Where(m => m.DeptID == _DeptId).Count();
                string _TotalUnits = Convert.ToString(TotalUnits);
                return _TotalUnits;
            }
            
        }

        public static List<Month> GetAllMonths()
        {
            var context = new DCCEntities();
            List<Month> _Months = new List<Month>();
            try
            {
                _Months = context.Months.OrderBy(m => m.MonthId).ToList();
                
            }
            catch(Exception ex)
            {
                ErrorLog.log("An Error Occurred" + ex.Message + " Stack Trace: " + ex.StackTrace);
                _Months = null;
            }
            finally
            {
                context.Dispose();
            }
            return _Months;
        }

        public static DateTime? GetDateFromString(string InputDate)
        {
            Nullable<DateTime> MyNullableDate = null;
            if (InputDate != null || InputDate != "")
            {
                string day = null;
                string month = null;
                string year = null;
                string fullDate = null;
                year = InputDate.Substring(0, 4);
                month = InputDate.Substring(5, 2);
                day = InputDate.Substring(7, 3);
                fullDate = month +  day + "/" + year;
                DateTime? date = DateTime.ParseExact(fullDate, "MM/dd/yyyy", null);

                return date.Value.Date;
            }
            else
            {
                return MyNullableDate;
            }
        }
         
    }
}