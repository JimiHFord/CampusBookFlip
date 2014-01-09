using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Abstract
{
    public interface IGoogleSearch
    {
        List<Book> Search(string search);
    }
}