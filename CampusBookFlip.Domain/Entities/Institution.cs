using FileHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    //Institution_ID	Institution_Name	Institution_Address	Institution_City	Institution_State	Institution_Zip	Institution_Phone	Institution_OPEID	Institution_IPEDS_UnitID	Institution_Web_Address	
    [Table("Institution")]
    public class Institution
    {
        [Key]
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string WebAddress { get; set; }
        public bool Activated { get; set; }
        public DateTime? DateActivated { get; set; }
        public virtual ICollection<Campus> CampusList { get; set; }
    }

    [Table("Campus")]
    public class Campus
    {
        [Required]
        public int InstitutionId { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime? DateActivated { get; set; }
        public bool Activated { get; set; }

        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }
    }

    #region File Helper
    [DelimitedRecord(",")]
    public class CampusFile
    {
        public int InstitutionId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    [DelimitedRecord(",")]
    public class InstitutionFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string WebAddress { get; set; }
    }
#endregion

    //Institution_ID    Campus_ID	Campus_Name	Campus_Address	Campus_City	Campus_State	Campus_Zip	Campus_IPEDS_UnitID	Accreditation_Type	Agency_Name	Agency_Status	Program_Name	Accreditation_Status	Accreditation_Date_Type	Periods	Last Action
}
