using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.BLL
{
    public class Setting
    {
        public static List<SelectListItem> GetState()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            using (var context = new DCCEntities())
            {
                var query = context.States.ToList();
                foreach (var item in query)
                {
                    states.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Name
                    });
                }
                return states;
            }
        }
    }
}