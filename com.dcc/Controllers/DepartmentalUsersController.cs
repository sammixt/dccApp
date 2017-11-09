using com.dcc.BLL;
using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    [Authorize]
    public class DepartmentalUsersController : Controller
    {
        #region members
        // GET: DepartmentalUsers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MembersInDept()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Dept = ProcessDept.GetSingleDepartment(DeptId);
            var _member = ProcessUser.GetAllMemberInDept(DeptId);
            return View(_member);
        }

        public ActionResult MemberDetail(string MemberId)
        {
            if (MemberId == null)
            {
                RedirectToAction("MemberDetail", "DepartmentalUsers");
            }

            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            var GetUnitCount = ProcessUser.CountUserUnit(MemberId, DeptId);
            if (GetUnitCount == 0)
            {
                ViewBag.DeptDetails = null;
            }
            else
            {
                int _deptId = Convert.ToInt32(DeptId);
                var membersDepts = ProcessUser.GetMemberDetails(MemberId);
                ViewBag.DeptDetails = membersDepts.Where(m => m.DeptId == _deptId).ToList();
            }
            var SingleUser = ProcessUser.GetSingleUser(MemberId);
            //check users dept.
            return View(SingleUser);
        }

        public ActionResult EditMember(string MemberId = null)
        {
            if (MemberId == null)
            {
                return RedirectToAction("MembersInDept");
            }
            ViewBag.State = Setting.GetState();
            var SingleUser = ProcessUser.GetSingleUser(MemberId);
            return View(SingleUser);
        }

        [HttpPost]
        public ActionResult EditMember(Believer model)
        {
            string MemberId;
            if (ModelState.IsValid)
            {
                MemberId = ProcessUser.UpdateUser(model);
                if (MemberId != null)
                {
                    return RedirectToAction("MemberDetail", "DepartmentalUsers", new { MemberId = MemberId });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        #endregion members

        #region units
        public ActionResult UnitsInDept()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Dept = ProcessDept.GetSingleDepartment(DeptId);
            var _units = ProcessUnits.GetAllUnitsInDept(DeptId);
            return View(_units);
        }

        // GET: Units/Edit/5
        public ActionResult EditUnit(string id = null)
        {
            if (id == null)
            {
                return RedirectToAction("UnitsInDept");
            }
            var _units = ProcessUnits.GetSingleUnit(id);
            ViewBag.Dept = ProcessDept.GetAllDepartments();
            return View(_units);
        }

        // POST: Units/Edit/5
        [HttpPost]
        public ActionResult EditUnit(int id, Unit collection)
        {
            string UnitId;
            if (ModelState.IsValid)
            {
                UnitId = ProcessUnits.UpdateUnit(collection);
                if (UnitId != null)
                {
                    return RedirectToAction("UnitsInDept", "DepartmentalUsers");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Units/Delete/5
        public ActionResult DeleteUnit(string id)
        {
            var SingleUnit = ProcessUnits.DeleteSingleUnit(id);
            return RedirectToAction("UnitsInDept", "DepartmentalUsers");
        }

        public ActionResult MembersInUnit(string id)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.UnitName = ProcessUnits.GetSingleUnit(id).UnitName;
            var _member = ProcessUser.GetAllMemberInUnit(id, DeptId);
            return View(_member);
        }

        public ActionResult AssignToUnit(string BelieverId)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Believer = ProcessUser.GetSingleUser(BelieverId);
            ViewBag.Post = CommonBLL.GetAllPost();
            var _member = ProcessUser.GetSingleMemberInUnit(BelieverId, DeptId);
            ViewBag.Units = ProcessUnits.GetAllUnitsInDept(DeptId);
            return View(_member);
        }

        [HttpPost]
        public ActionResult AssignToUnit(Member collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Believer = ProcessUser.GetSingleUser(Convert.ToString(collection.BelieverId));
            ViewBag.Post = CommonBLL.GetAllPost();
            var _member = ProcessUser.GetSingleMemberInUnit(Convert.ToString(collection.BelieverId), DeptId);
            ViewBag.Units = ProcessUnits.GetAllUnitsInDept(DeptId);
            string UnitId;
            if (ModelState.IsValid)
            {
                UnitId = ProcessUser.AssignUserToUnit(collection);
                if (UnitId != null)
                {
                    return RedirectToAction("MemberDetail", "DepartmentalUsers", new { MemberId = UnitId });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        #endregion units
    }
}