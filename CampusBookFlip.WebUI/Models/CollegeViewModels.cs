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

    public class CollegeSearchViewModel
    {
        public string SearchText { get; set; }
    }

    public class CollegeSearchResultsViewModel
    {
        public double TotalSeconds { get; set; }
        public double TotalMilliseconds { get; set; }
        public IEnumerable<Institution> Colleges { get; set; }
    }
}