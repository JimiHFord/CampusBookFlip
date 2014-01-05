using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Concrete
{
    public class EFRepository : IRepository
    {
        private EFDbContext context = new EFDbContext();

        public void DisableProxyCreation()
        {
            context.Configuration.ProxyCreationEnabled = false;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public bool ConfirmAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            CBFUser user = context.User.FirstOrDefault(u => u.OAuthConfirmEmailToken == id);
            if (user != null)
            {
                user.ConfirmedEmail = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<Book> Book
        {
            get
            {
                return context.Book.Include("Authors").Include("Publisher");
            }
        }

        public IQueryable<Book> XBook
        {
            get
            {
                return context.Book;
            }
        }

        public int SaveBook(Book book)
        {
            int pk = 0;
            Book entry = context.Book.Find(book.Id);
            if (entry == null)
            {
                entry = context.Book.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
            }
            if (entry == null)
            {
                entry = context.Book.FirstOrDefault(b => b.ISBN10 == book.ISBN10);
            }
            if (entry == null)
            {
                context.Book.Add(book);
                context.SaveChanges();
                pk = book.Id;
            }
            else
            {
                entry.ImageLarge = book.ImageLarge;
                entry.ImageSmall = book.ImageSmall;
                
                if (book.Authors != null)
                {
                    var authors = context.BookAuthor.Where(ba => ba.BookId == entry.Id);
                    foreach (var a in authors)
                    {
                        context.BookAuthor.Remove(a);
                    }
                    entry.Authors = book.Authors;
                }
                entry.Description = book.Description;
                entry.ISBN10 = book.ISBN10;
                entry.ISBN13 = book.ISBN13;
                entry.PublishDate = book.PublishDate;
                entry.PublisherId = book.PublisherId;
                entry.Title = book.Title;
                entry.PageCount = book.PageCount;
                entry.ListPrice = book.ListPrice;
                entry.RetailPrice = book.RetailPrice;
                entry.AvailableAsEPUB = book.AvailableAsEPUB;
                entry.AvailableAsPDF = book.AvailableAsPDF;
                entry.CurrencyCodeLP = book.CurrencyCodeLP;
                entry.CurrencyCodeRP = book.CurrencyCodeRP;
                context.SaveChanges();
                pk = entry.Id;
            }

            return pk;
        }

        public Book DeleteBook(int id)
        {
            Book entry = context.Book.Find(id);
            if (entry != null)
            {
                context.Book.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }

        public IQueryable<Author> Author
        {
            get
            {
                return context.Author;
            }
        }

        public IQueryable<Author> XAuthor
        {
            get
            {
                return context.Author;
            }
        }

        public int SaveAuthor(Author author)
        {
            int pk = 0;
            Author entry = context.Author.Find(author.Id);
            if (entry == null)
            {
                entry = context.Author.FirstOrDefault(a => a.Name == author.Name);
            }
            if (entry == null)
            {
                context.Author.Add(author);
                context.SaveChanges();
                pk = author.Id;
            }
            else
            {
                entry.Name = author.Name;
                context.SaveChanges();
                pk = entry.Id;
            }
            return pk;
        }

        public Author DeleteAuthor(int id)
        {
            Author entry = context.Author.Find(id);
            if (entry != null)
            {
                context.Author.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }

        public IQueryable<BookAuthor> BookAuthor
        {
            get
            {
                return context.BookAuthor.
                    Include("Book").
                    Include("Author");
            }
        }

        public IQueryable<BookAuthor> XBookAuthor
        {
            get
            {
                return context.BookAuthor;         
            }
        }

        public void SaveBookAuthor(BookAuthor ba)
        {
            BookAuthor entry = context.BookAuthor.Find(ba.BookId, ba.AuthorId);
            if (entry == null)
            {
                context.BookAuthor.Add(ba);
                context.SaveChanges();
            }
        }

        public BookAuthor DeleteBookAuthor(int BookId, int AuthorId)
        {
            BookAuthor entry = context.BookAuthor.Find(BookId, AuthorId);
            if (entry != null)
            {
                context.BookAuthor.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }

        public IQueryable<Publisher> Publisher
        {
            get
            {
                return context.Publisher;
            }
        }

        public IQueryable<Publisher> XPublisher
        {
            get
            {
                return context.Publisher;
            }
        }

        public int SavePublisher(Publisher publisher)
        {
            int pk = 0;
            Publisher entry = context.Publisher.Find(publisher.Id);
            if (entry == null)
            {
                entry = context.Publisher.FirstOrDefault(p => p.Name == publisher.Name);
            }
            if (entry == null)
            {
                context.Publisher.Add(publisher);
                context.SaveChanges();
                pk = publisher.Id;
            }
            else
            {
                entry.Name = publisher.Name;
                context.SaveChanges();
                pk = entry.Id;
            }
            return pk;
        }

        public Publisher DeletePublisher(int id)
        {
            Publisher entry = context.Publisher.Find(id);
            if (entry != null)
            {
                context.Publisher.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }

        public IQueryable<CBFUser> User
        {
            get
            {
                return context.User.Include("Colleges");
            }
        }

        public IQueryable<CBFUser> XUser
        {
            get
            {
                return context.User;
            }
        }

        public int SaveUser(CBFUser user)
        {
            int pk = 0;

            CBFUser entry = context.User.Find(user.Id);
            if (entry == null)
            {
                context.User.Add(user);
                context.SaveChanges();
                pk = user.Id;
            }
            else
            {
                entry.AppUserName = user.AppUserName;
                entry.Colleges = user.Colleges;
                entry.FirstName = user.FirstName;
                entry.LastName = user.LastName;
                entry.Paid = user.Paid;
                context.SaveChanges();
                pk = entry.Id;
            }

            return pk;
        }

        public CBFUser DeleteUser(int id)
        {
            CBFUser entry = context.User.Find(id);
            if (entry != null)
            {
                context.User.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }

        public IQueryable<UserBook> UserBook
        {
            get
            {
                return context.UserBook;

            }
        }

        public IQueryable<UserBook> XUserBook
        {
            get
            {
                return context.UserBook;

            }
        }

        public int SaveUserBook(UserBook ub)
        {
            int pk = 0;
            UserBook entry = context.UserBook.Find(ub.Id);
            if (entry == null)
            {
                context.UserBook.Add(ub);
                context.SaveChanges();
                pk = ub.Id;
            }
            else
            {
                entry.Price = ub.Price;
                entry.UserId = ub.UserId;
                entry.BookId = ub.BookId;
                context.SaveChanges();
                pk = entry.Id;
            }
            return pk;
        }

        public UserBook DeleteUserBook(int id)
        {
            UserBook entry = context.UserBook.Find(id);
            if (entry != null)
            {
                context.UserBook.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }
    }
}
