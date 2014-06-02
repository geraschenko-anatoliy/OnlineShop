using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.WebUI.HtmlHelpers
{
    public static class AllCategoriesHelpers
    {
        public static MvcHtmlString GetSelectAllCategoriesLabel(this HtmlHelper html, string labelName, bool IsChecked)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder labelTag = new TagBuilder("label");
            TagBuilder inputTag = new TagBuilder("input");
            inputTag.MergeAttribute("type", "checkbox");
            inputTag.MergeAttribute("name", labelName);
            inputTag.MergeAttribute("value", "true");

            if (IsChecked)
                inputTag.MergeAttribute("checked", "checked");

            TagBuilder spanTag = new TagBuilder("span");
            spanTag.InnerHtml = "All categories";
            labelTag.InnerHtml += inputTag.ToString();
            labelTag.InnerHtml += spanTag.ToString();

            result.Append(labelTag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString GetSelectedCategoriesLabels(this HtmlHelper html, ICollection<int> ViewBagSelectedCategories, Category category)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder labelTag = new TagBuilder("label");
            TagBuilder inputTag = new TagBuilder("input");
            inputTag.MergeAttribute("type", "checkbox");
            inputTag.MergeAttribute("name", "selectedCategories");
            inputTag.MergeAttribute("value", category.CategoryID.ToString());

            if (ViewBagSelectedCategories.Contains(category.CategoryID))
                inputTag.MergeAttribute("checked", "checked");

            TagBuilder spanTag = new TagBuilder("span");
            spanTag.InnerHtml = category.Name;

            labelTag.InnerHtml += inputTag.ToString();
            labelTag.InnerHtml += spanTag.ToString();

            result.Append(labelTag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}