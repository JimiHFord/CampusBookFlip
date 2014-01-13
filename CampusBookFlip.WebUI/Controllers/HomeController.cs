using Json;
using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CampusBookFlip.WebUI.Infrastructure;
using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.WebUI.Models;
using CampusBookFlip.WebUI.Abstract;

namespace CampusBookFlip.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private const string slogon = "Textbook buying and selling made easy.";
        private IRepository repo;
        private IGoogleSearch google;
        private ICBFSecurity secure;

        public HomeController(IRepository repo, IGoogleSearch google, ICBFSecurity secure)
        {
            this.repo = repo;
            this.google = google;
            this.secure = secure;
        }

        public ActionResult Index()
        {
            ViewBag.Message = slogon;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(BookSearchTextViewModel model)
        {
            ViewBag.Message = slogon;
            List<Book> bookList = google.Search(model.ISBN);
            return PartialView("~/Views/Books/AJAXSearchResultsBookList.cshtml",
                new CampusBookFlip.WebUI.Models.BookListViewModel { Books = bookList });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            repo.Dispose();
        }

        [ChildActionOnly]
        public ActionResult NavbarHelper()
        {
            if (!Request.IsAuthenticated)
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AnonymousNavbar.cshtml");
            }
            else if (secure.IsAdmin())
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AuthenticatedAdminNavbar.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AuthenticatedNavbar.cshtml");
            }
        }

        [ChildActionOnly]
        public ActionResult FooterHelper()
        {
            if (!Request.IsAuthenticated)
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AnonymousFooter.cshtml");
            }
            else if (secure.IsAdmin())
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AuthenticatedAdminFooter.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_LayoutHelper/_AuthenticatedFooter.cshtml");
            }
        }
    }
}
