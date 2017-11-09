using com.dcc.Entities;
using com.dcc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace com.dcc.BLL
{
    public class ProcessUnits
    {
        public static string InsertUnit(Unit model)
        {
            string UnitId;
            try
            {
                using (var context = new DCCEntities())
                {
                    context.Units.Add(model);
                    context.SaveChanges();
                    var id = model.UnitId;
                    UnitId = Convert.ToString(id);
                    return UnitId;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static Unit GetSingleUnit(string UnitId)
        {
            int _UnitId = Convert.ToInt32(UnitId);
            Unit unit = new Unit();
            try
            {
                using (var context = new DCCEntities())
                {
                    unit = context.Units.Include(p => p.Department).Where(m => m.UnitId == _UnitId).FirstOrDefault();
                    return unit;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Unit> GetAllUnits()
        {
            List<Unit> unit = new List<Unit>();
            try
            {
                using (var context = new DCCEntities())
                {
                    unit = context.Units.Include(d => d.Department).ToList();
                    return unit;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Unit> GetAllUnitsInDept(string DeptId )
        {
            int _deptId = Convert.ToInt32(DeptId);
            List<Unit> unit = new List<Unit>();
            try
            {
                var context = new DCCEntities();

                unit = context.Units.Where(d => d.DeptID == _deptId).ToList();
                return unit;
                
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string UpdateUnit(Unit model)
        {
            string UnitId;
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.Units.Where(m => m.UnitId == model.UnitId).FirstOrDefault();
                    if (query != null)
                    {
                        query.DeptID = model.DeptID;
                        query.UnitFunction = model.UnitFunction;
                        query.UnitName = model.UnitName;
                        query.UnitShortCode = model.UnitShortCode;
                        context.SaveChanges();
                        UnitId = Convert.ToString(model.UnitId);
                        return UnitId;
                    }
                    return null;

                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string DeleteSingleUnit(string UnitId)
        {
            DCCEntities _Unit = new DCCEntities();
            decimal _UnitId = Convert.ToDecimal(UnitId);
            var unit = _Unit.Units.Where(m => m.UnitId == _UnitId).FirstOrDefault();
            if (unit != null)
            {
                _Unit.Units.Remove(unit);
                _Unit.SaveChanges();
                return "success";
            }
            else
            {
                return null;
            }
        }
    }
}