using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("CBFUser")]
    public class CBFUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AppUserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public bool Paid { get; set; }
        //[Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public virtual ICollection<UserCollege> Colleges { get; set; }
    }
}
