using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("Request")]
    public class Request
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        [Key, Column(Order=1)]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual CBFUser User { get; set; }
        [ForeignKey("PostId")]
        public virtual Posting Posting { get; set; }
    }
}

