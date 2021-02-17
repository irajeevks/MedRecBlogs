using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedrecTechnologies.Blog.Controllers
{
    public class errorController : Controller
    {
        [ActionName("internal-server-error")]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        // GET: error
        public ActionResult internalservererror()
        {
            return View();
        }
        [ActionName("page-not-found")]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        // GET: error
        public ActionResult pagenotfound()
        {
            return View();
        }
    }
}