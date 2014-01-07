using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Models
{
    public class CollegeListViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<Institution> Institutions { get; set; }
    }
}