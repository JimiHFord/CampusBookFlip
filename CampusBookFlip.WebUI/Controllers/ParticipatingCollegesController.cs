using CampusBookFlip.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusBookFlip.WebUI.Controllers
{
    public class ParticipatingCollegesController : Controller
    {
        private IRepository repo;

        public ParticipatingCollegesController(IRepository repo)
        {
            this.repo = repo;
        }
        //
        // GET: /ParticipatingColleges/

        public ActionResult Index()
        {

            return View(repo.Institution.Where(i => i.Activated).OrderBy(c => c.State).ThenBy(c => c.City).ThenBy(c => c.Name));
        }

    }
}
