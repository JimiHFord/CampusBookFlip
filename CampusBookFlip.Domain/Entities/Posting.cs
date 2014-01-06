using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("Posting")]
    public class Posting
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CollegeId { get; set; }
        public int UserBookId { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }
        [Required]
        public DateTime DateAvailable { get; set; }
        [Range(1, 10)]
        public int Condition { get; set; }

        [ForeignKey("UserBookId")]
        public virtual UserBook Book { get; set; }
        [ForeignKey("CollegeId")]
        public virtual Campus College { get; set; }
        [ForeignKey("UserId")]
        public virtual CBFUser User { get; set; }

        public ICollection<Request> Requests { get; set; }
    }
}
