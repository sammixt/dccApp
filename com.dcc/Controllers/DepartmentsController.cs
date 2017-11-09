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
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
            var Dept = ProcessDept.GetAllDepartments();
            return View(Dept);
        }

        // GET: Departments/Details/5
        public ActionResult Details(string _DeptId)
        {
            var Dept = ProcessDept.GetSingleDepartment(_DeptId);
            return View(Dept);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department collection)
        {
            string DeptId;
            if (ModelState.IsValid)
            {
                DeptId = ProcessDept.InsertDepartmnet(collection);
                if (DeptId != null)
                {
                    return RedirectToAction("Details", "Departments", new { _DeptId = DeptId });
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

        // GET: Departments/Edit/5
        public ActionResult Edit(string _DeptId)
        {
            var SingleDept = ProcessDept.GetSingleDepartment(_DeptId);
            return View(SingleDept);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int _DeptId, Department collection)
        {
            string DeptId;
            if (ModelState.IsValid)
            {
                DeptId = ProcessDept.UpdateDepartment(collection);
                if (DeptId != null)
                {
                    return RedirectToAction("Details", "Departments", new { _DeptId = DeptId });
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

        // GET: Departments/Delete/5
        public ActionResult Delete(string _DeptId)
        {
            var SingleUser = ProcessDept.DeleteSingleUser(_DeptId);
            return RedirectToAction("Index");
        }

        public ActionResult MembersInDept(string DeptId)
        {
            ViewBag.DeptName = ProcessDept.GetSingleDepartment(DeptId);
            var _member = ProcessUser.GetAllMemberInDept(DeptId);
            return View(_member);
        }
    }
}
