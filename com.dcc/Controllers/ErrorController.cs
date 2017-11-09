﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    public class ErrorController : Controller
    {
        [HandleError]
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }
        [HandleError]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
    }
}