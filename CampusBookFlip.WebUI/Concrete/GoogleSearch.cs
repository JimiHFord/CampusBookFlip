using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using CampusBookFlip.WebUI.Abstract;
using CampusBookFlip.WebUI.Infrastructure;
using Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace CampusBookFlip.WebUI.Concrete
{
    public class GoogleSearch : IGoogleSearch
    {
        private IRepository repo;

        public GoogleSearch(IRepository repo)
        {
            this.repo = repo;
        }

        public string AuthorizeSearch(string query)
        {
            string template = WebConfigurationManager.AppSettings["GoogleBooksAPI"];
            return string.Format(template, HttpUtility.UrlEncode(query));
        }

        public List<Book> Search(string search)
        {
            bool store_results = Constants.GrowDB;
            string searchString = AuthorizeSearch(search);
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
            catch (Exception)
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