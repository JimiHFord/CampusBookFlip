using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("ChangeEmailRequest")]
    public class ChangeEmailRequest
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string ConfirmationToken { get; set; }
        [Required]
        public string NewEmail { get; set; }
        [ForeignKey("UserId")]
        public virtual CBFUser User { get; set; }
    }
}
