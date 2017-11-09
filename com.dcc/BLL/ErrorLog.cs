using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcc.BLL
{
    public class ErrorLog
    {
       
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void log(string error)
        {
            Log.Debug(error);
        }
        
    }
}