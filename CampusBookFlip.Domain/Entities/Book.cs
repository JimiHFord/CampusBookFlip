﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace CampusBookFlip.Domain.Entities
{
    internal class BookHelper
    {
        public const string UNKNOWN = "unknown";
        public const string YES = "yes";
        public const string NO = "no";
    }

    [Table("Book")]
    public class Book
    {
        
        [Key]
        public int Id { get; set; }
        public int PublisherId { get; set; }
        [Required]
        public string ISBN13 { get; set; }
        [Required]
        public string ISBN10 { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublishDate { get; set; }
        public string ImageSmall { get; set; }
        public string ImageLarge { get; set; }
        public int PageCount { get; set; }


        public decimal ListPrice { get; set; }
        public string CurrencyCodeLP { get; set; }
        public decimal RetailPrice { get; set; }
        public string CurrencyCodeRP { get; set; }

        public bool AvailableAsEPUB { get; set; }
        public bool AvailableAsPDF { get; set; }

        public string EPUB { get { return AvailableAsEPUB == null ? BookHelper.UNKNOWN : (bool)AvailableAsEPUB ? BookHelper.YES : BookHelper.NO; } }
        public string PDF { get { return AvailableAsPDF == null ? BookHelper.UNKNOWN : (bool)AvailableAsPDF ? BookHelper.YES : BookHelper.NO; } }

        public string List
        {
            get
            {
                if (ListPrice != 0 && CurrencyCodeLP == null)
                {
                    return string.Format("{0}", decimal.Round((decimal)ListPrice, 2));
                }
                if (ListPrice != 0 && CurrencyCodeLP != null)
                {
                    return string.Format("{0} {1}", CurrencyCodeLP.ToString().Equals("USD") ? "$" : CurrencyCodeLP.ToString(), decimal.Round((decimal)ListPrice, 2));
                }
                return BookHelper.UNKNOWN;
            }
        }

        public string Retail
        {
            get
            {
                if (RetailPrice != 0 && CurrencyCodeRP == null)
                {
                    return string.Format("{0}", decimal.Round((decimal)RetailPrice, 2));
                }
                if (ListPrice != 0 && CurrencyCodeRP != null)
                {
                    return string.Format("{0} {1}", CurrencyCodeRP.ToString().Equals("USD") ? "$" : CurrencyCodeRP.ToString(), decimal.Round((decimal)RetailPrice, 2));
                }
                return BookHelper.UNKNOWN;
            }
        }

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }

        public virtual ICollection<BookAuthor> Authors { get; set; }

        public bool HasAuthors
        {
            get
            {
                return Authors != null && Authors.Count() > 0;
            }
        }
    }
}
