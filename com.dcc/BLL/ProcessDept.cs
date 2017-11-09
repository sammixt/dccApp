using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.BLL
{
    public class ProcessDept
    {
        public static string InsertDepartmnet(Department model)
        {
            string DepartmentId;
            try
            {
                using (var context = new DCCEntities())
                {
                    context.Departments.Add(model);
                    context.SaveChanges();
                    var id = model.DeptId;
                    DepartmentId = Convert.ToString(id);
                    return DepartmentId;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static Department GetSingleDepartment(string _DepartmentId)
        {
            int DepartmentId = Convert.ToInt32(_DepartmentId);
            Department dept = new Department();
            try
            {
                using (var context = new DCCEntities())
                {
                    dept = context.Departments.Where(m => m.DeptId == DepartmentId).FirstOrDefault();
                    return dept;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Department> GetAllDepartments()
        {
            List<Department> dept = new List<Department>();
            try
            {
                using (var context = new DCCEntities())
                {
                    dept = context.Departments.ToList();
                    return dept;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string UpdateDepartment(Department model)
        {
            string DepartmentId;
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.Departments.Where(m => m.DeptId == model.DeptId).FirstOrDefault();
                    if (query != null)
                    {
                        query.DeptName = model.DeptName;
                        query.DeptDesc = model.DeptDesc;
                        query.ShortCode = model.ShortCode;
                        query.Vision = model.Vision;
                        context.SaveChanges();
                        DepartmentId = Convert.ToString(model.DeptId);
                        return DepartmentId;
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

        internal static string DeleteSingleUser(string DepartmentId)
        {
            DCCEntities _Department = new DCCEntities();
            decimal _Dept = Convert.ToDecimal(DepartmentId);
            var member = _Department.Departments.Where(m => m.DeptId == _Dept).FirstOrDefault();
            if (member != null)
            {
                _Department.Departments.Remove(member);
                _Department.SaveChanges();
                return "success";
            }
            else
            {
                return null;
            }
        }
    }
}