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
    public class UnitsController : Controller
    {
        // GET: Units
        public ActionResult Index()
        {
            var _units = ProcessUnits.GetAllUnits();
            return View(_units);
        }

        // GET: Units/Details/5
        public ActionResult Details(string id)
        {
            var _units = ProcessUnits.GetSingleUnit(id);
            return View(_units);
        }

        // GET: Units/Create
        public ActionResult Create()
        {
            ViewBag.Dept = ProcessDept.GetAllDepartments();
            return View();
        }

        // POST: Units/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit collection)
        {
            string UnitId;
            if (ModelState.IsValid)
            {
                UnitId = ProcessUnits.InsertUnit(collection);
                if (UnitId != null)
                {
                    return RedirectToAction("Details", "Units", new { id = UnitId });
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

        // GET: Units/Edit/5
        public ActionResult Edit(string id)
        {
            var _units = ProcessUnits.GetSingleUnit(id);
            ViewBag.Dept = ProcessDept.GetAllDepartments();
            return View(_units);
        }

        // POST: Units/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Unit collection)
        {
            string UnitId;
            if (ModelState.IsValid)
            {
                UnitId = ProcessUnits.UpdateUnit(collection);
                if (UnitId != null)
                {
                    return RedirectToAction("Details", "Units", new { id = UnitId });
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
        public ActionResult Delete(string id)
        {
            var SingleUnit = ProcessUnits.DeleteSingleUnit(id);
            return RedirectToAction("Index");
        }
    }
}
