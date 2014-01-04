using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("BookAuthor")]
    public class BookAuthor
    {
        [Key, Column(Order=0)]
        public int BookId { get; set; }
        [Key, Column(Order=1)]
        public int AuthorId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
    }
}
