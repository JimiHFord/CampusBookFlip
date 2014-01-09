using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    [Table("College")]
    public class Campus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [MinLength(5), MaxLength(5)]
        public string ZipCode { get; set; }

        public bool Activated { get; set; }
    }
    //Campus_ID	Campus_Name	Campus_Address	Campus_City	Campus_State	Campus_Zip	Campus_IPEDS_UnitID	Accreditation_Type	Agency_Name	Agency_Status	Program_Name	Accreditation_Status	Accreditation_Date_Type	Periods	Last Action
}
