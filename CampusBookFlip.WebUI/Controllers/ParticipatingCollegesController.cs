using CampusBookFlip.Domain.Abstract;
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

        //public ViewResult List(int page = 1)
        //{
        //    ProductListViewModel model = new ProductListViewModel
        //    {
        //        Products = repo.Product,
        //        //.OrderBy(p => p.Id).Skip((page - 1) * PageSize).Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = PageSize,
        //            TotalItems = repo.Product.Count()
        //        }
        //    };
        //    //return View("../Product/List",model);
        //    return View("List", model.Products);
        //}

        public ActionResult Index(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }
            int ItemsPerPage = Constants.ParticipatingCollegesItemsPerPage;
            return View(new CollegeListViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItems = repo.XInstitution.Where(i => i.Activated).Count(),
                    ItemsPerPage = ItemsPerPage,
                },
                Institutions = repo.Institution.
                Where(i => i.Activated).
                OrderBy(c => c.State).
                ThenBy(c => c.City).
                ThenBy(c => c.Name).
                Skip((page-1)*ItemsPerPage).
                Take(ItemsPerPage)
            });
        }

    }
}
