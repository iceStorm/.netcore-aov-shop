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

    [HtmlTargetElement("div", Attributes = "paging-action")]
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
            TagBuilder divTag = new TagBuilder("div");


            #region Previous Button
            TagBuilder firstATag = new TagBuilder("a");
            firstATag.Attributes["class"] = "page-item head-item";
            if (PagingModel.CurrentPageIndex > 1)
            {
                firstATag.InnerHtml.Append("⤎");
                firstATag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = PagingModel.CurrentPageIndex - 1 });
            }
            divTag.InnerHtml.AppendHtml(firstATag);
            #endregion


            #region Center Area
            for (int i = 1; i <= PagingModel.TotalPages; ++i)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes["class"] = i == PagingModel.CurrentPageIndex ? "page-item active" : "page-item";


                if (PagingModel.CurrentPageIndex != i)
                {
                    aTag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = i });
                }

                aTag.InnerHtml.Append(i.ToString());
                divTag.InnerHtml.AppendHtml(aTag);
            }
            #endregion


            #region Next Button
            TagBuilder lastATag = new TagBuilder("a");
            lastATag.Attributes["class"] = "page-item head-item";
            if (PagingModel.CurrentPageIndex < PagingModel.TotalPages)
            {
                lastATag.Attributes["href"] = urlHelper.Action(PagingAction, new { pageIndex = PagingModel.CurrentPageIndex + 1 });
                lastATag.InnerHtml.Append("⤏");
            }
            divTag.InnerHtml.AppendHtml(lastATag);
            #endregion



            output.Content.AppendHtml(divTag.InnerHtml);
        }
    }
}
