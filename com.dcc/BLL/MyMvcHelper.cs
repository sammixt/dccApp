using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace com.dcc.BLL
{
    public static class MyMvcHelper
    {
        public static MvcHtmlString GetAjaxPath(this HtmlHelper helper)
        {
            string _path = ConfigurationManager.AppSettings["PathForAjaxCall"].ToString();
            return new MvcHtmlString(_path);
        }
    }
}