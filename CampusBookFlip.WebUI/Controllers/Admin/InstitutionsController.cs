using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using CampusBookFlip.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusBookFlip.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    public class InstitutionsController : Controller
    {
        private IRepository repo;

        public InstitutionsController(IRepository repo)
        {
            this.repo = repo;
        }

        //
        // GET: /Institutions/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This method
        /// </summary>
        /// <param name="postmodel"></param>
        /// <returns>A partial view containing all matching institutions</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CampusBookFlip.WebUI.Models.CollegeSearchViewModel postmodel)
        {
            if (ModelState.IsValid)
            {
                string search = string.IsNullOrEmpty(postmodel.SearchText) ? "" : postmodel.SearchText.ToLower();
                DateTime first = DateTime.Now;
                //I changed repo.Institution to repo."X"Instutition (which bypasses the 'virtual' attributes) and 
                //that decreased the query time a LOT
                IEnumerable<Institution> model = repo.XInstitution.Where(i =>
                    i.Address.ToLower().Contains(search) ||
                    i.City.ToLower().Contains(search) ||
                    i.Name.ToLower().Contains(search) ||
                    i.Phone.ToLower().Contains(search) ||
                    i.State.ToLower().Contains(search) ||
                    i.WebAddress.ToLower().Contains(search) ||
                    i.Zip.ToLower().Contains(search));
                DateTime second = DateTime.Now;
                return PartialView("~/Views/Institutions/_AdminInstitutionSearchPartial.cshtml", new CollegeSearchResultsViewModel
                {
                    TotalSeconds = second.Subtract(first).TotalSeconds,
                    TotalMilliseconds = second.Subtract(first).TotalMilliseconds,
                    Colleges = model
                });
            }
            return PartialView("~/Views/Institutions/_AdminInstitutionsSearchPartial.cshtml", new CollegeSearchResultsViewModel
            {
                TotalMilliseconds = 0,
                TotalSeconds = 0,
                Colleges = new List<Institution>()
            });
        }

    }
}
