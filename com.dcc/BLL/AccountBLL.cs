using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace com.dcc.BLL
{
    public class AccountBLL
    {
        internal static UsersAccount AutenticateUser(Entities.UsersAccount model)
        {
            UsersAccount UserInfo = new UsersAccount();
            try
            {
                var context = new DCCEntities();
                
                    var query = context.UsersAccounts.Where(m => m.UserName == model.UserName && m.Password == model.Password && m.DeptId == model.DeptId).FirstOrDefault();
                    return query;
                
            }
            catch(Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace );
                return null;
            }
        }
    }
}