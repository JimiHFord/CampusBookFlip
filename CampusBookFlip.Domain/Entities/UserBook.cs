using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("UserBook")]
    public class UserBook
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int BookId { get; set; }
        [Display(Name="Price")]
        public int Price { get; set; }
        [Display(Name = "Price")]
        public string Dollars
        {
            get
            {
                return string.Format("${0}.00", this.Price);
            }
        }
        [Display(Name="Sold For")]
        public int? SoldFor { get; set; }

        public bool IsSold
        {
            get
            {
                return this.SoldFor != null;
            }
        }

        [ForeignKey("UserId")]
        [Display(Name="Owner")]
        public virtual CBFUser User { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}
