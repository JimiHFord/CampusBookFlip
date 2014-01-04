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

namespace CampusBookFlip.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private const string slogon = "Textbook buying and selling made easy.";
        private IRepository repo;

        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }

        public ActionResult Index()
        {
            ViewBag.Message = slogon;
            //Constants.FixPublishers(repo);
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

        //ProductListViewModel model = new ProductListViewModel
        //{
        //    Products = repo.Product,
        //    //.OrderBy(p => p.Id).Skip((page - 1) * PageSize).Take(PageSize),
        //    PagingInfo = new PagingInfo
        //    {
        //        CurrentPage = page,
        //        ItemsPerPage = PageSize,
        //        TotalItems = repo.Product.Count()
        //    }
        //};

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(BookSearchTextViewModel model)
        {
            //BookSearchTextViewModel model = JsonParser.Deserialize<BookSearchTextViewModel>(json.ToString());
            
            ViewBag.Message = slogon;
            //try
            //{
                //if (ModelState.IsValid)
                //{
                    List<Book> bookList = CampusBookFlip.WebUI.Infrastructure.Google.Search(model.ISBN, repo);
                    return PartialView("~/Views/Books/AJAXSearchResultsBookList.cshtml",
                        new CampusBookFlip.WebUI.Models.BookListViewModel { Books = bookList });
                //}
                //return View("~/Views/Home/Index.cshtml", model);
            //}
            //catch (Exception e)
            //{
            //    return Content(e.Message);
            //}
        }
    }
}

//StringBuilder ret = new StringBuilder();
//            /*
//                * This loop iterates through every book that was returned in the search results
//                * Items is a list and Count is the number of items in the list
//                */
//            for (int i = 0; i < book_results.Items.Count; i++)
//            {
//                /*
//                    * This loop iterates through each KeyValuePair in the volumeInfo Dictionary
//                    */
//                foreach (var entry in book_results.Items[i]["volumeInfo"])
//                {
//                    /*
//                        * Since there are 1 or more authors and categories, we must handle those separately
//                        */
//                    if (entry.Key.Equals("authors") ||
//                        entry.Key.Equals("categories"))
//                    {
//                        ret.Append("<p>" + entry.Key + ":</p>");
//                        //This iterates through each author and category
//                        foreach (var authorOrCategory in entry.Value)
//                        {
//                            ret.Append("<p>" + authorOrCategory + "</p>");
//                        }
//                    }
//                    else if (entry.Key.Equals("industryIdentifiers"))
//                    {
//                        //This iterates through each Identifier (ISBN-13 and ISBN-10)
//                        for (int j = 0; j < entry.Value.Count; j++)
//                        {
//                            ret.Append("<p>" + entry.Value[j]["type"] + ": " + entry.Value[j]["identifier"] + "</p>");
//                        }
//                    }
//                    else if (entry.Key.Equals("imageLinks"))
//                    {
//                        //This iterates through each picture
//                        //So far I have only seen a "smallThumbnail" and a "thumbnail"
//                        foreach (var img in entry.Value)
//                        {
//                            ret.Append("<div><img src=\"" + img.Value + "\"/></div><p/>");
//                        }
//                    }
//                    else
//                    {
//                        ret.Append("<p>" + entry.Key + ": " + entry.Value + "</p>");
//                    }
//                }
//                //Delimits new book
//                ret.Append("------------------------------------------------------------------------------");
//                ret.Append("------------------------------------------------------------------------------");
//            }