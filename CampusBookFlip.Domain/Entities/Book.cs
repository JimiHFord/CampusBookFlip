using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using CampusBookFlip.Domain.Abstract;

namespace CampusBookFlip.Domain.Entities
{
    internal class BookHelper
    {
        public const string UNKNOWN = "unknown";
        public const string YES = "yes";
        public const string NO = "no";
        public const int MAX_WORDS = 100;

        public const int MAX_CHARACTERS = 300;
    }

    [Table("Book")]
    public class Book : Identifyable
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
        public int? PageCount { get; set; }


        public decimal? ListPrice { get; set; }
        public string CurrencyCodeLP { get; set; }
        public decimal? RetailPrice { get; set; }
        public string CurrencyCodeRP { get; set; }

        public bool? AvailableAsEPUB { get; set; }
        public bool? AvailableAsPDF { get; set; }

        public string EPUB { get { return AvailableAsEPUB == null ? BookHelper.UNKNOWN : (bool)AvailableAsEPUB ? BookHelper.YES : BookHelper.NO; } }
        public string PDF { get { return AvailableAsPDF == null ? BookHelper.UNKNOWN : (bool)AvailableAsPDF ? BookHelper.YES : BookHelper.NO; } }

        public string List
        {
            get
            {
                if (ListPrice != null && CurrencyCodeLP == null)
                {
                    return string.Format("{0}", decimal.Round((decimal)ListPrice, 2));
                }
                if (ListPrice != null && CurrencyCodeLP != null)
                {
                    return string.Format("{0} {1}", CurrencyCodeLP.ToString().Equals("USD") ? "$" : CurrencyCodeLP.ToString(), decimal.Round((decimal)ListPrice, 2));
                }
                return BookHelper.UNKNOWN;
            }
        }

        public string Pages
        {
            get
            {
                return PageCount == null ? BookHelper.UNKNOWN : PageCount.ToString();
            }
        }

        public string Retail
        {
            get
            {
                if (RetailPrice != null && CurrencyCodeRP == null)
                {
                    return string.Format("{0}", decimal.Round((decimal)RetailPrice, 2));
                }
                if (ListPrice != null && CurrencyCodeRP != null)
                {
                    return string.Format("{0} {1}", CurrencyCodeRP.ToString().Equals("USD") ? "$" : CurrencyCodeRP.ToString(), decimal.Round((decimal)RetailPrice, 2));
                }
                return BookHelper.UNKNOWN;
            }
        }

        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<BookAuthor> Authors { get; set; }

        public bool HasAuthors
        {
            get
            {
                return Authors != null && Authors.Count() > 0;
            }
        }

        public string AuthorsLabel
        {   
            get
            {
                string plural = "Authors", singular = "Author";
                return !HasAuthors ? plural :
                    Authors.Count() > 1 ? plural : singular;
            }
        }

        public bool NeedsAbbreviation
        {
            get
            {
                return !string.IsNullOrEmpty(Description) && !(Description.Length < BookHelper.MAX_CHARACTERS);
            }
        }

        public string ShortDescription
        {
            get
            {
                //StringBuilder retval = new StringBuilder();
                //string tostring = string.Empty;
                //string[] split = Description.Split(new char[] { ' ' });
                //if (split.Count() > BookHelper.MAX_WORDS)
                //{

                //}
                //else
                //{
                //    foreach (var s in split)
                //    {
                //        retval.Append(s + " ");
                //    }
                //    tostring = retval.ToString();
                //}
                return NeedsAbbreviation ? Description.Substring(0, BookHelper.MAX_CHARACTERS) :
                    Description;
            }
        }
    }
}
