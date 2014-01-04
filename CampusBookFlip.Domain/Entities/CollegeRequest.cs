using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("CollegeRequest")]
    public class CollegeRequest
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        [MinLength(5), MaxLength(5)]
        [Display(Name="Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name="Additional Notes")]
        public string AdditionalNotes { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        [Display(Name="Requested By")]
        public virtual CBFUser User { get; set; }

        [Display(Name="Date Requested")]
        public DateTime Date { get; set; }
    }
}
