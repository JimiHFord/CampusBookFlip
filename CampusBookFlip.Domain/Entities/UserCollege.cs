using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("UserCollege")]
    public class UserCollege
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int CollegeId { get; set; }

        [ForeignKey("UserId")]
        public virtual CBFUser User { get; set; }
        [ForeignKey("CollegeId")]
        public virtual College College { get; set; }
    }
}
