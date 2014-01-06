using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Abstract
{
    public interface IRepository : System.IDisposable
    {
        void DisableProxyCreation();
        bool ConfirmAccount(string id);

        IQueryable<Book> Book { get; }
        IQueryable<Book> XBook { get; }
        int SaveBook(Book book);
        Book DeleteBook(int id);

        IQueryable<Author> Author { get; }
        IQueryable<Author> XAuthor { get; }
        int SaveAuthor(Author author);
        Author DeleteAuthor(int id);


        IQueryable<BookAuthor> BookAuthor { get; }
        IQueryable<BookAuthor> XBookAuthor { get; }
        void SaveBookAuthor(BookAuthor ba);
        BookAuthor DeleteBookAuthor(int BookId, int AuthorId);

        IQueryable<Publisher> Publisher { get; }
        IQueryable<Publisher> XPublisher { get; }
        int SavePublisher(Publisher publisher);
        Publisher DeletePublisher(int id);

        IQueryable<CBFUser> User { get; }
        IQueryable<CBFUser> XUser { get; }
        int SaveUser(CBFUser user);
        CBFUser DeleteUser(int id);

        IQueryable<UserBook> UserBook { get; }
        IQueryable<UserBook> XUserBook { get; }
        int SaveUserBook(UserBook ub);
        UserBook DeleteUserBook(int id);

        IQueryable<ChangeEmailRequest> ChangeEmailRequest { get; }
        IQueryable<ChangeEmailRequest> XChangeEmailRequest { get; }
        void SaveChangeEmailRequest(ChangeEmailRequest req);
        ChangeEmailRequest DeleteChangeEmailRequest(int Id);

        IQueryable<Institution> Institution { get; }
        IQueryable<Institution> XInstitution { get; }
        int SaveInstitution(Institution inst);
        Institution DeleteInstitution(int id);

        IQueryable<Campus> Campus { get; }
        IQueryable<Campus> XCampus { get; }
        int SaveCampus(Campus campus);
        Campus DeleteCampus(int id);

    }
}
