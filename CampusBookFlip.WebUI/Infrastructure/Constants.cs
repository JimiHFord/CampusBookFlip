using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using WebMatrix.WebData;

namespace CampusBookFlip.WebUI.Infrastructure
{
    public class Constants
    {
        public static string DEFAULT_PASSWORD { get { return "password"; } }
        public static string ADMIN { get { return "Administrator"; } }
        public static string jimi { get { return "jford"; } }
        public static string wes { get { return "wjones"; } }
        public static string EMAIL_NO_REPLY { get { return "no-reply@campusbookflip.com"; } }

        public static bool MakeGoogleRequest
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["MakeGoogleRequest"];
                if (val == null) { val = ""; }
                return val.ToLower().Equals("true");
            }
        }

        public static bool GrowDB
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["GrowDB"];
                if (val == null) { val = ""; }
                return val.ToLower().Equals("true");
            }
        }

        public static void FixPublishers(IRepository repo)
        {
            var all_ids = repo.Publisher.Select(a => a.Id).ToList();
            for (int all_i = 0; all_i < all_ids.Count(); all_i++)
            {
                int current_publisher_id = all_ids.FirstOrDefault();
                var current_publisher = repo.Publisher.FirstOrDefault(p => p.Id == current_publisher_id);
                all_ids.RemoveAt(0);
                if (current_publisher != null)
                {
                    var duplicates = repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id);

                    var book_ids = repo.Book.Where(b => duplicates.Select(p => p.Id).Contains(b.PublisherId)).Select(b => b.Id).ToList();
                    for (int book_i = 0; book_i < book_ids.Count(); book_i++)
                    {
                        int current_book_id = book_ids.FirstOrDefault();
                        var current_book = repo.Book.FirstOrDefault(b => b.Id == current_book_id);
                        book_ids.RemoveAt(0);
                        current_book.PublisherId = current_publisher.Id;
                        repo.SaveBook(current_book);
                    }
                    while (repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id).Count() > 0)
                    {
                        repo.DeletePublisher(repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id).FirstOrDefault().Id);
                    }
                }
            }

        }

        internal static void InitializeWebSecurity()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("EFDbContext", "CBFUser", "Id", "AppUserName", autoCreateTables: true);
            }
        }

        internal static void SeedAdmins()
        {
            Constants.InitializeWebSecurity();

            if (!Roles.RoleExists(Constants.ADMIN))
            {
                Roles.CreateRole(Constants.ADMIN);
            }

            if (!WebSecurity.UserExists(Constants.jimi))
            {
                WebSecurity.CreateUserAndAccount(Constants.jimi,
                    Constants.DEFAULT_PASSWORD, new
                    {
                        FirstName = "Jimi",
                        LastName = "Ford",
                        Paid = false
                    });
            }

            if (!WebSecurity.UserExists(Constants.wes))
            {
                WebSecurity.CreateUserAndAccount(Constants.wes,
                    Constants.DEFAULT_PASSWORD, new
                    {
                        FirstName = "Wes",
                        LastName = "Jones",
                        Paid = false
                    });
            }

            if (!Roles.IsUserInRole(Constants.jimi, Constants.ADMIN))
            {
                Roles.AddUserToRole(Constants.jimi, Constants.ADMIN);
            }

            if (!Roles.IsUserInRole(Constants.wes, Constants.ADMIN))
            {
                Roles.AddUserToRole(Constants.wes, Constants.ADMIN);
            }
        }
    }

    public class Google
    {
        public static string Search(string query)
        {
            string template = WebConfigurationManager.AppSettings["GoogleBooksAPI"];
            return string.Format(template, HttpUtility.UrlEncode(query));
        }

        public static List<CampusBookFlip.Domain.Entities.Book> Search(string search, IRepository repo)
        {
            bool store_results = Constants.GrowDB;
            string searchString = CampusBookFlip.WebUI.Infrastructure.Google.Search(search);
            List<Book> bookList = new List<Book>();
            if (string.IsNullOrEmpty(search))
            {
                return bookList;
            }
            Book tempBook;
            HttpWebRequest request = WebRequest.Create(searchString) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var book_results = JsonParser.Deserialize(reader.ReadToEnd());
            List<String> authors;
            List<BookAuthor> bookAuthors;
            try
            {
                if (book_results.Items == null)
                {
                    return new List<Book>();
                }
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
            for (int bookNumber = 0; bookNumber < book_results.Items.Count; bookNumber++)
            {
                tempBook = new Book();
                authors = new List<string>();
                bookAuthors = new List<BookAuthor>();
                //Add all authors to the book to be returned to the view
                if (book_results.Items[bookNumber].ContainsKey("volumeInfo"))
                {
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("authors"))
                    {
                        for (int auth_count = 0; auth_count < book_results.Items[bookNumber]["volumeInfo"]["authors"].Count; auth_count++)
                        //foreach (string author in book_results.Items[bookNumber]["volumeInfo"]["authors"])
                        {
                            string author = book_results.Items[bookNumber]["volumeInfo"]["authors"][auth_count];
                            authors.Add(author);
                            int tempAuthID = store_results ? repo.SaveAuthor(new Author { Name = author }) :
                                repo.Author.FirstOrDefault(a => a.Name == author) == null ? 0 : repo.Author.FirstOrDefault(a => a.Name == author).Id;
                            if (tempAuthID != 0 && !bookAuthors.Select(ba => ba.AuthorId).Contains(tempAuthID))
                            {
                                bookAuthors.Add(new BookAuthor { AuthorId = tempAuthID });
                            }

                        }
                    }
                    tempBook.Authors = bookAuthors;
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("description"))
                    {
                        tempBook.Description = book_results.Items[bookNumber]["volumeInfo"]["description"];
                    }
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("industryIdentifiers"))
                    {
                        //vol/indIdentify/[]type=ISBN_10
                        //vol/indIdentify/[]identifier=VALUE
                        foreach (var industryIdentifier in book_results.Items[bookNumber]["volumeInfo"]["industryIdentifiers"])
                        {
                            if (industryIdentifier["type"].Equals("ISBN_10"))
                            {
                                tempBook.ISBN10 = industryIdentifier["identifier"];
                            }
                            else if (industryIdentifier["type"].Equals("ISBN_13"))
                            {
                                tempBook.ISBN13 = industryIdentifier["identifier"];
                            }
                        }
                    }
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("title"))
                    {
                        tempBook.Title = book_results.Items[bookNumber]["volumeInfo"]["title"];
                    }
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("pageCount"))
                    {
                        tempBook.PageCount = (int)book_results.Items[bookNumber]["volumeInfo"]["pageCount"];
                    }
                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("imageLinks"))
                    {
                        if (book_results.Items[bookNumber]["volumeInfo"]["imageLinks"].ContainsKey("smallThumbnail"))
                        {
                            tempBook.ImageSmall = book_results.Items[bookNumber]["volumeInfo"]["imageLinks"]["smallThumbnail"];
                        }
                        if (book_results.Items[bookNumber]["volumeInfo"]["imageLinks"].ContainsKey("thumbnail"))
                        {
                            tempBook.ImageLarge = book_results.Items[bookNumber]["volumeInfo"]["imageLinks"]["thumbnail"];
                        }
                    }

                    if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("publishedDate"))
                    {
                        tempBook.PublishDate = book_results.Items[bookNumber]["volumeInfo"]["publishedDate"];
                    }

                }
                if (book_results.Items[bookNumber].ContainsKey("saleInfo"))
                {
                    if (book_results.Items[bookNumber]["saleInfo"].ContainsKey("listPrice"))
                    {
                        if (book_results.Items[bookNumber]["saleInfo"]["listPrice"].ContainsKey("amount"))
                        {
                            tempBook.ListPrice = (decimal)book_results.Items[bookNumber]["saleInfo"]["listPrice"]["amount"];
                        }
                        if (book_results.Items[bookNumber]["saleInfo"]["listPrice"].ContainsKey("currencyCode"))
                        {
                            tempBook.CurrencyCodeLP = book_results.Items[bookNumber]["saleInfo"]["listPrice"]["currencyCode"];
                        }
                    }
                    if (book_results.Items[bookNumber]["saleInfo"].ContainsKey("retailPrice"))
                    {
                        if (book_results.Items[bookNumber]["saleInfo"]["retailPrice"].ContainsKey("amount"))
                        {
                            tempBook.RetailPrice = (decimal)book_results.Items[bookNumber]["saleInfo"]["retailPrice"]["amount"];
                        }
                        if (book_results.Items[bookNumber]["saleInfo"]["retailPrice"].ContainsKey("currencyCode"))
                        {
                            tempBook.CurrencyCodeRP = book_results.Items[bookNumber]["saleInfo"]["retailPrice"]["currencyCode"];
                        }
                    }
                }
                if (book_results.Items[bookNumber].ContainsKey("accessInfo"))
                {
                    if (book_results.Items[bookNumber]["accessInfo"].ContainsKey("epub"))
                    {
                        if (book_results.Items[bookNumber]["accessInfo"]["epub"].ContainsKey("isAvailable"))
                        {
                            tempBook.AvailableAsEPUB = book_results.Items[bookNumber]["accessInfo"]["epub"]["isAvailable"];
                        }
                    }
                    if (book_results.Items[bookNumber]["accessInfo"].ContainsKey("pdf"))
                    {
                        if (book_results.Items[bookNumber]["accessInfo"]["pdf"].ContainsKey("isAvailable"))
                        {
                            tempBook.AvailableAsPDF = book_results.Items[bookNumber]["accessInfo"]["pdf"]["isAvailable"];
                        }
                    }
                }
                if (book_results.Items[bookNumber]["volumeInfo"].ContainsKey("publisher"))
                {
                    string tempPublishString = book_results.Items[bookNumber]["volumeInfo"]["publisher"];
                    int tempPublishId = store_results ? repo.SavePublisher(new Publisher { Name = tempPublishString }) :
                                repo.Publisher.FirstOrDefault(a => a.Name == tempPublishString) == null ? 0 : repo.Publisher.
                                FirstOrDefault(a => a.Name == tempPublishString).Id;
                    tempBook.PublisherId = tempPublishId;
                    tempBook.Publisher = repo.Publisher.FirstOrDefault(pub => pub.Id == tempPublishId);
                }
                bool is_valid_book = !string.IsNullOrEmpty(tempBook.ISBN10) && !string.IsNullOrEmpty(tempBook.ISBN13) && !string.IsNullOrEmpty(tempBook.Title)
                    && tempBook.PublisherId != 0;
                if (is_valid_book)
                {
                    bookList.Add(tempBook);
                    if (store_results)
                    {
                        repo.SaveBook(tempBook);
                    }
                }
            }//END FOR LOOP

            return bookList;
        }
    }


}