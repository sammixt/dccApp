using com.dcc.BLL;
using com.dcc.Entities;
using com.dcc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMembers(FormCollection collection)
        {
            if (AttendanceBLL.CheckAttendanceRecordExist == true)
            {
                return RedirectToAction("GetAttendanceRecord");
            }
            else
            {
                var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                string group = collection["Group"];
                string _group = group == "ALL" ? null : group;
                var members = ProcessUser.GetAllMemberInDeptForAttendance(DeptId, _group);
                return View(members);
            }
            
        }

        [HttpPost]
        public ActionResult MarkAttendance(MarkAttendanceViewModel[] collection)
        {
            var AttendanceCount = collection.ToList() ;
            string outcome = AttendanceBLL.InsertAttendanceRecord(AttendanceCount);

            return RedirectToAction("GetAttendanceRecord");
        }

        [HttpPost]
        public JsonResult UpdateAttendance(Attendance model)
        {
            string outcome = AttendanceBLL.UpdateAttendance(model);
            return Json(outcome, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAttendanceRecord(string AttendanceDate = null)
        {
            List<Attendance> AttendanceList = new List<Attendance>();
            if (AttendanceDate == null)
            {
                AttendanceList = AttendanceBLL.GetAttendance(null);
                ViewBag.Date = DateTime.Now.ToLongDateString();
            }
            else
            {
                DateTime? SearchDate = CommonBLL.GetDateFromString(AttendanceDate);
                AttendanceList = AttendanceBLL.GetAttendance(SearchDate);
                ViewBag.Date = SearchDate.Value.ToLongDateString();
            }

            return View(AttendanceList);
        }
    }
}