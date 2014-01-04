using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusBookFlip.WebUI.HtmlHelpers
{

//    <div class="modal fade" id="book-partial-modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
//  <div class="modal-dialog">
//    <div class="modal-content">
//      <div class="modal-header">
//        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
//        <h4 class="modal-title" id="myModalLabel">Modal title</h4>
//      </div>
//      <div class="modal-body">
//        <div class="row">
//            <div class="span1 offset5">
//                <img src="~/Images/ajax-loader.gif" />
//            </div>
//        </div>
//      </div>
//      <div class="modal-footer">
//        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
//        <button type="button" class="btn btn-primary">Save changes</button>
//      </div>
//    </div><!-- /.modal-content -->
//  </div><!-- /.modal-dialog -->
//</div><!-- /.modal -->


    public static class Modal
    {
        public static MvcHtmlString CenteredAjaxLoadingGif(this HtmlHelper htmlHelper, string VirtualGifPath = "~/Images/ajax-loader.gif")
        {
            return MvcHtmlString.Create("<div class=\"text-center\"><img src=\"" + VirtualPathUtility.ToAbsolute(VirtualGifPath) + "\" /></div>");
        }

        public static MvcHtmlString BootstrapDefault(this HtmlHelper htmlHelper, string ModalId, string ModalLabel, string ModalTitle, string ModalBodyId, string ModalPrimaryButtonId, string ModalPrimaryButtonLabel, string VirtualGifPath = "~/Images/ajax-loader.gif")
        {
            var modal = new TagBuilder("div");
            modal.Attributes["id"] = ModalId;
            modal.Attributes["aria-labelledby"] = ModalLabel;
            modal.Attributes["role"] = "dialog";
            modal.Attributes["tabindex"] = "-1";
            modal.Attributes["aria-hidden"] = "true";
            modal.AddCssClass("modal");
            modal.AddCssClass("fade");

            var modal_dialog = new TagBuilder("div");
            modal_dialog.AddCssClass("modal-dialog");
            var modal_content = new TagBuilder("div");
            modal_content.AddCssClass("modal-content");

            var modal_header = new TagBuilder("div");
            modal_header.AddCssClass("modal-header");
            modal_header.InnerHtml = "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>";
            modal_header.InnerHtml += "<h4 class=\"modal-title\" id=" + ModalLabel + ">" + ModalTitle + "</h4>";

            var modal_body = new TagBuilder("div");
            modal_body.AddCssClass("modal-body");
            modal_body.Attributes["id"] = ModalBodyId;
            modal_body.InnerHtml = CenteredAjaxLoadingGif(null,VirtualGifPath).ToString();

            var modal_footer = new TagBuilder("div");
            modal_footer.AddCssClass("modal-footer");
            modal_footer.InnerHtml = "<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Close</button>";
            modal_footer.InnerHtml += "<button type=\"button\" id=\"" + ModalPrimaryButtonId + "\" class=\"btn btn-primary\">" + ModalPrimaryButtonLabel + "</button>";
            modal_content.InnerHtml = modal_header.ToString();
            modal_content.InnerHtml += modal_body.ToString();
            modal_content.InnerHtml += modal_footer.ToString();
            
            modal_dialog.InnerHtml = modal_content.ToString();

            modal.InnerHtml = modal_dialog.ToString();

            return MvcHtmlString.Create(modal.ToString());
        }
    }
}
