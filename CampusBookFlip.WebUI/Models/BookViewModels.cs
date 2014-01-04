using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Models
{
    public class BookListViewModel
    {
        public List<Book> Books { get; set; }
        public void Add(Book book)
        {
            if (Books == null)
            {
                Books = new List<Book>();
            }
            Books.Add(book);
        }
    }

    public class BookSearchTextViewModel
    {
        [Required]
        //[RegularExpression(@"ISBN(-1(?:(0)|3))?:?\x20(\s)*[0-9]+[- ][0-9]+[- ][0-9]+[- ][0-9]*[- ]*[xX0-9]", ErrorMessage="Must be an ISBN-10 or ISBN-13 number")]
        public string ISBN { get; set; }
        //public string __RequestVerificationToken { get; set; }
    }
}