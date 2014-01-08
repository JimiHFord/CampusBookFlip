using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using CampusBookFlip.WebUI.Infrastructure;
using CampusBookFlip.WebUI.Models;
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

        public ActionResult Index(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }
            int ItemsPerPage = Constants.ParticipatingCollegesItemsPerPage;
            IEnumerable<Institution> ins = repo.Institution.Where(i => i.Activated);
            return View(new CollegeListViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItems = ins.Count(),
                    ItemsPerPage = ItemsPerPage,
                },
                Institutions = ins.
                OrderBy(c => c.State).
                ThenBy(c => c.City).
                ThenBy(c => c.Name).
                Skip((page-1)*ItemsPerPage).
                Take(ItemsPerPage)
            });
        }

    }
}
