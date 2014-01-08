using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Models
{
    public class PagingInfo
    {
        private int MaxButtonCount = 8;
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public int MaxButtons
        {
            get
            {
                return this.MaxButtonCount;
            }

            set
            {
                MaxButtonCount = value;
            }
        }
    }
}