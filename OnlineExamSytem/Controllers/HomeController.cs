using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamSytem.Helpers;

namespace OnlineExamSytem.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult About()
        {
            ViewBag.Message = "Kullanıcıların quiz olabileceği bir platformdur. Ayrıca kendi profillerini ayarlayabilirler";

            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}