using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructures
{

    [HtmlTargetElement("ul", Attributes = "paging-action")]
    public class PagingTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }



        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PagingModel { get; set; }

        public string PagingAction { get; set; }



        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder ulTag = new TagBuilder("ul");


            #region Previous Button
            TagBuilder firstLiTag = new TagBuilder("li");
            firstLiTag.Attributes["class"] = "page-item head-item";
            if (PagingModel.CurrentPageIndex > 1)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes["class"] = "page-link";
                aTag.InnerHtml.Append("⤎");
                aTag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = PagingModel.CurrentPageIndex - 1 });
                firstLiTag.InnerHtml.AppendHtml(aTag);
            }
            else
            {
                firstLiTag.Attributes["class"] += " not-available";
            }
            ulTag.InnerHtml.AppendHtml(firstLiTag);
            #endregion


            #region Center Area
            for (int i = 1; i <= PagingModel.TotalPages; ++i)
            {
                TagBuilder liTag = new TagBuilder("li");
                liTag.Attributes["class"] = i == PagingModel.CurrentPageIndex ? "page-item active" : "page-item";


                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes["class"] = "page-link";
                if (PagingModel.CurrentPageIndex != i)
                {
                    aTag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = i });
                }
                aTag.InnerHtml.Append(i.ToString());


                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }
            #endregion


            #region Next Button
            TagBuilder lastLiTag = new TagBuilder("li");
            lastLiTag.Attributes["class"] = "page-item head-item";
            if (PagingModel.CurrentPageIndex < PagingModel.TotalPages)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes["class"] = "page-link";
                aTag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = PagingModel.CurrentPageIndex + 1 });
                aTag.InnerHtml.Append("⤏");
                lastLiTag.InnerHtml.AppendHtml(aTag);
            }
            else
            {
                lastLiTag.Attributes["class"] += " not-available";
            }
            ulTag.InnerHtml.AppendHtml(lastLiTag);
            #endregion



            output.Content.AppendHtml(ulTag.InnerHtml);
        }
    }
}
