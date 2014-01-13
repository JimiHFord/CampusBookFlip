using CampusBookFlip.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusBookFlip.WebUI.Controllers
{
    [Authorize]
    public class YourCollegeController : Controller
    {
        private IRepository repo;

        public YourCollegeController(IRepository repo)
        {
            this.repo = repo;
        }
        //
        // GET: /YourCollege/

        public ActionResult Index()
        {

            return View();
        }

    }
}
