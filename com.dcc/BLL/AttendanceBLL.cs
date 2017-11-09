using com.dcc.Entities;
using com.dcc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace com.dcc.BLL
{
    public class AttendanceBLL
    {
        public static string InsertAttendanceRecord(List<MarkAttendanceViewModel> model)
        {
            Attendance _attendance = new Attendance();
            try
            {
               
                using (var context = new DCCEntities())
                {
                    foreach(var item in model)
                    {
                        _attendance.DateOfDate = DateTime.Now;
                        _attendance.WorkerId = item.userId;
                        _attendance.DeptGroup = item.group;
                        _attendance.PresentAbsent = Convert.ToBoolean(item.attendance);
                        context.Attendances.Add(_attendance);
                        context.SaveChanges();
                    }
                    return "success";  
                }
            }
            catch(Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Attendance> GetAttendance(DateTime? date)
        {
            List<Attendance> AttendanceList = new List<Attendance>();
            var context = new DCCEntities();
            try
            {
                if (date == null)
                {
                    AttendanceList = context.Attendances.Include(m => m.Believer).Where(d => d.DateOfDate == DateTime.Today).ToList();
                }
                else
                {
                    AttendanceList = context.Attendances.Include(m => m.Believer).Where(d => d.DateOfDate == date).ToList();
                }
                return AttendanceList;
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
    
        }

        public static bool CheckAttendanceRecordExist
        {
            get
            {
                var context = new DCCEntities();
                int AttendanceRecord = context.Attendances.Where(d => d.DateOfDate == DateTime.Today).Count();
                if (AttendanceRecord > 0)
                    return true;
                else return false;
            }
            
        }

        internal static string UpdateAttendance(Attendance model)
        {
           string outcome = string.Empty;
            var context = new DCCEntities();
            try
            {
                var query = context.Attendances.Where(m => m.AttendanceId == model.AttendanceId).FirstOrDefault();
                query.PresentAbsent = model.PresentAbsent;
                context.SaveChanges();
                outcome = "success";
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                outcome = "failed";
            }
            return outcome;
        }
    }
}