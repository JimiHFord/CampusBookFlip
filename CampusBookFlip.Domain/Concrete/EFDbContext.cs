using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusBookFlip.Domain.Entities;
using System.Data.Entity;

namespace CampusBookFlip.Domain.Concrete
{
    public class EFDbContext : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /*
         * Lookup Data
         */
        public DbSet<Book> Book { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<CBFUser> User { get; set; }
        public DbSet<UserBook> UserBook { get; set; }
        public DbSet<ChangeEmailRequest> ChangeEmailRequest { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Campus> Campus { get; set; }
    }
}
