using CampusBookFlip.Domain.Abstract;
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
    public class CBFUser : Identifyable
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

        public string ConfirmEmailToken { get; set; }
        public bool ConfirmedEmail { get; set; }

        public virtual ICollection<UserInstitution> Colleges { get; set; }
    }
}
