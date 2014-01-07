using CampusBookFlip.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CampusBookFlip.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static string LeftArrowGlyph { get { return "&laquo;"; } }
        public static string RightArrowGlyph { get { return "&raquo;"; } }
        public static MvcHtmlString BootstrapPageLinks(this HtmlHelper html, 
            PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");
            for (int i = 0; i <= pagingInfo.TotalPages + 1; i++)
            {
                TagBuilder aTag = new TagBuilder("a");
                TagBuilder liTag = new TagBuilder("li");
                if (i <= 0)
                {
                    if (pagingInfo.CurrentPage <= 1 || pagingInfo.CurrentPage > pagingInfo.TotalPages) //or greater than total pages

                    {

                        liTag.AddCssClass("disabled");
                        aTag.MergeAttribute("href", "#");
                    }
                    else
                    {
                        aTag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                    }
                    aTag.InnerHtml = LeftArrowGlyph;
                    liTag.InnerHtml = aTag.ToString();
                }
                else if (i == pagingInfo.TotalPages + 1)
                {
                    if (pagingInfo.CurrentPage >= pagingInfo.TotalPages)
                    {
                        liTag.AddCssClass("disabled");
                        aTag.MergeAttribute("href", "#");
                    }
                    else
                    {
                        aTag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
                    }
                    aTag.InnerHtml = RightArrowGlyph;
                    liTag.InnerHtml = aTag.ToString();
                }
                else
                {
                    aTag.MergeAttribute("href", pageUrl(i));
                    aTag.InnerHtml = i.ToString();
                    if (i == pagingInfo.CurrentPage)
                    {
                        liTag.AddCssClass("active");
                    }
                    liTag.InnerHtml = aTag.ToString();
                }
                result.Append(liTag.ToString());
            }
            ulTag.InnerHtml = result.ToString();
            return MvcHtmlString.Create(ulTag.ToString());
        }

        //<ol class="breadcrumb">
        //    <li><a href="#">Home</a></li>
        //    <li><a href="#">Library</a></li>
        //    <li class="active">Data</li>
        //</ol>

        public static MvcHtmlString BootstrapBreadcrumbs(this HtmlHelper htmlHelper, string[] labels, string[] paths = null)
        {
            if (labels == null) throw new ArgumentNullException("labels can not be null");
            int path_count = paths == null ? 0 : paths.Count();
            if (path_count != labels.Count() -1) throw new ArgumentException("labels.Count() must be one more than paths.Count()");
            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("breadcrumb");
            StringBuilder result = new StringBuilder();
            TagBuilder a, li;
            for (int i = 0; i < labels.Count(); i++)
            {
                a = new TagBuilder("a");
                li = new TagBuilder("li");
                
                //this is the current / active path
                if (i == labels.Count() - 1)
                {
                    li.AddCssClass("active");
                    li.InnerHtml = labels[i];
                }
                else
                {
                    a.MergeAttribute("href", paths[i]);
                    a.InnerHtml = labels[i];
                    li.InnerHtml = a.ToString();
                }
                result.Append(li.ToString());
            }
            ol.InnerHtml = result.ToString();
            return MvcHtmlString.Create(ol.ToString());
        }
    }
}