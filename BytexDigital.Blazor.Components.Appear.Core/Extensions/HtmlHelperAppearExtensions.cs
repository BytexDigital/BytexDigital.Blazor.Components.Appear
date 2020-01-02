using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperAppearExtensions
    {
        public static HtmlString IncludeAppearScripts(this IHtmlHelper htmlHelper)
        {
            return new HtmlString($"<script src='/_content/BytexDigital.Blazor.Components.Appear/bundle.js'></script>");
        }
    }
}
