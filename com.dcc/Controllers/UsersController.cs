using com.dcc.BLL;
using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult User()
        {
            ViewBag.State = Setting.GetState();
            return View();
        }

        [HttpPost]
        public ActionResult User(Believer model)
        {
            string MemberId;
            if (ModelState.IsValid)
            {
                MemberId = ProcessUser.InsertUser(model);
                if (MemberId != null)
                {
                    return RedirectToAction("Believers", "Users", new { _MemberId = MemberId });
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

        public ActionResult Believers(string _MemberId)
        {
            var SingleUser = ProcessUser.GetSingleUser(_MemberId);
            //check users dept.
            var GetDeptCount = ProcessUser.CountUserDept(_MemberId);
            if (GetDeptCount == 0)
            {
                ViewBag.DeptDetails = null;
            }
            else
            {
                ViewBag.DeptDetails = ProcessUser.GetMemberDetails(_MemberId);
            }
            return View(SingleUser);
        }

        public ActionResult CreateLoginCredBelievers(string _MemberId,string _DeptId)
        {
            var SingleUser = ProcessUser.GetSingleUser(_MemberId);
            var GetAllRole = CommonBLL.GetAllROle();
            var _AdminRole = GetAllRole.Where(m => m.RoleType == 1).ToList();
            //check users dept.
            var GetDeptCount = ProcessUser.CountUserDept(_MemberId);
            if (GetDeptCount == 0)
            {
                ViewBag.DeptDetails = null;
            }
            else
            {
                ViewBag.DeptDetails = ProcessDept.GetSingleDepartment(_DeptId);
            }
            ViewBag.MemberId = _MemberId;
            ViewBag.Roles = _AdminRole;
            return PartialView(SingleUser);
        }

        public ActionResult CreateLoginCredBelieversDept(string _MemberId, string _DeptId)
        {
            var SingleUser = ProcessUser.GetSingleUser(_MemberId);
            var GetAllRole = CommonBLL.GetAllROle();
            var _AdminRole = GetAllRole.Where(m => m.RoleType != 1).ToList();
            //check users dept.
            var GetDeptCount = ProcessUser.CountUserDept(_MemberId);
            if (GetDeptCount == 0)
            {
                ViewBag.DeptDetails = null;
            }
            else
            {
                ViewBag.DeptDetails = ProcessDept.GetSingleDepartment(_DeptId);
            }
            ViewBag.MemberId = _MemberId;
            ViewBag.Roles = _AdminRole;
            return PartialView("CreateLoginCredBelievers",SingleUser);
        }

        public ActionResult ProcessCreateLoginCredBelievers(UsersAccount Data)
        {
            string outcome = string.Empty;
            if (ModelState.IsValid)
            {
                bool userExist = ProcessUser.CheckUserLoginAccountExist(Data);
                bool userNameExist = ProcessUser.CheckIfUserNameExist(Data);
                if (userExist == true)
                {
                    outcome = "User is already created in this Department";
                }
                else if (userNameExist == true)
                {
                    outcome = "Username Already Exist";
                }
                else
                {
                    ProcessUser.CreateUserLogin(Data);
                    outcome = "Account Created Successfully";
                }
            }
            return Json(outcome, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BelieversList()
        {
            var Users = ProcessUser.GetAllMembers();
            return View(Users);
        }

        public ActionResult EditBeliver(string MemberId)
        {
            ViewBag.State = Setting.GetState();
            var SingleUser = ProcessUser.GetSingleUser(MemberId);
            return View(SingleUser);
        }

        [HttpPost]
        public ActionResult EditBeliever(Believer model)
        {
            string MemberId;
            if (ModelState.IsValid)
            {
                MemberId = ProcessUser.UpdateUser(model);
                if (MemberId != null)
                {
                    return RedirectToAction("Believers", "Users", new { _MemberId = MemberId });
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

        public ActionResult Delete(string MemberId)
        {
            var SingleUser = ProcessUser.DeleteSingleUser(MemberId);
            return RedirectToAction("BelieversList");
        }
    }
}