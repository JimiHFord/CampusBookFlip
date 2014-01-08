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
    [Table("Publisher")]
    public class Publisher : Identifyable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
