using com.dcc.BLL;
using com.dcc.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            ViewBag.Dept = ProcessDept.GetAllDepartments();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersAccount model, string urlReturn)
        {
            //var pass = AuthenticationService.Validate(model.UserName, model.Password);

            var profile = AccountBLL.AutenticateUser(model);
            if (profile != null)
            {
                var claims = new List<Claim>();
                
                
                claims.Add(new Claim(ClaimTypes.Name, profile.UserName));
                claims.Add(new Claim(ClaimTypes.GivenName, (profile.Believer.FirstName + " "+ profile.Believer.LastName)));
                claims.Add(new Claim(ClaimTypes.SerialNumber, Convert.ToString(profile.BelieverId)));
                claims.Add(new Claim(ClaimTypes.StateOrProvince, profile.Department.DeptName));
                claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(profile.DeptId)));
                claims.Add(new Claim(ClaimTypes.Role, Convert.ToString(profile.Role.Role_Name)));
                claims.Add(new Claim(ClaimTypes.Rsa, Convert.ToString(profile.RoleId)));

                SignInAsync(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

                return RedirectToLocal(urlReturn, profile.Role.Role_Name);
            }
            ViewBag.ErrorMessage = "Invalid username or password combination for department";
            ViewBag.Dept = ProcessDept.GetAllDepartments();
            return View(model);
        }

           private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void SignInAsync(ClaimsIdentity id)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(id);
        }

        private ActionResult RedirectToLocal(string returnUrl, string role = null)
        {
            
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {

                switch (role)
                {
                    case "Time Keeper":
                        return RedirectToAction("Index", "Attendance");
                       
                    case "Finance":
                        return RedirectToAction("Index", "Finance");

                    default:
                        return RedirectToAction("Home", "Account");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //[Authorize]
        public ActionResult Home()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Believer = CommonBLL.CountBelivers;
            ViewBag.Members = CommonBLL.CountMembers;
            ViewBag.Departments = CommonBLL.CountDepts;
            ViewBag.Units = CommonBLL.CountUnit;

            ViewBag.MembersInDept = CommonBLL.CountMembersInDept(DeptId);
            ViewBag.UnitsInDept = CommonBLL.CountUnitInDept(DeptId);
            return View();
        }

    }
}