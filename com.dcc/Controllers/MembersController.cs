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
    public class MembersController : Controller
    {
        // GET: Members
        public ActionResult Index()
        {
            var Members = ProcessUser.GetAllMemberDetails();
            return View(Members);
        }

        // GET: Members/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Members/Create
        public ActionResult Create(string BelieverId = null)
        {
            ViewBag.Believer = ProcessUser.GetSingleUser(BelieverId);
            ViewBag.dept = ProcessDept.GetAllDepartments();
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string BelieverId,Member collection)
        {
            ViewBag.Believer = ProcessUser.GetSingleUser(BelieverId);
            ViewBag.dept = ProcessDept.GetAllDepartments();
            string MemberId;
            if (ModelState.IsValid)
            {
                MemberId = ProcessUser.InsertMember(collection);
                if (MemberId != null)
                {
                    return RedirectToAction("Believers", "Users", new { _MemberId = BelieverId });
                }
                else
                {
                    ViewBag.Error = "Oops an Error Occurred";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Oops an Error Occurred";
                return View();
            }

        }

        // GET: Members/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Members/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Members/Delete/5
        public ActionResult Delete(string id)
        {
           
            var status = ProcessUser.DeleteSingleMember(id);
            return Json(status,JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFromUnit(string id)
        {
            var status = ProcessUser.DeleteSingleMemberFromUnit(id);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        

        
    }
}
