using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CampusBookFlip.Domain.Entities;
using CampusBookFlip.Domain.Concrete;
using CampusBookFlip.Domain.Abstract;

namespace CampusBookFlip.WebUI.Controllers.Api
{
    public class BooksController : ApiController
    {
        private IRepository repo;

        public BooksController(IRepository repo)
        {
            this.repo = repo;
            repo.DisableProxyCreation();
        }

        // GET api/Books
        public IEnumerable<Book> GetBooks()
        {
            return repo.XBook;
        }

        // GET api/Books/5
        public Book GetBook(int id)
        {
            Book book = repo.XBook.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return book;
        }

        // PUT api/Books/5
        public HttpResponseMessage PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != book.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                repo.SaveBook(book);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Books
        public HttpResponseMessage PostBook(Book book)
        {
            if (ModelState.IsValid)
            {
                int id = repo.SaveBook(book);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, book);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Books/5
        public HttpResponseMessage DeleteBook(int id)
        {
            Book book = repo.XBook.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                repo.DeleteBook(book.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, book);
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}