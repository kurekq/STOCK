using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class HtmlParser
    {
        public static string RemoveFromHtml(string html, string from, string to, GET_HTML_ORDER order = GET_HTML_ORDER.FIRST)
        {
            
            int indexFrom = (order == GET_HTML_ORDER.FIRST ? html.IndexOf(from) : html.LastIndexOf(from));
            string SubstringAfterFrom = html.Substring(indexFrom, html.Length - indexFrom);
            int indexTo = (order == GET_HTML_ORDER.FIRST ? SubstringAfterFrom.IndexOf(to) : SubstringAfterFrom.LastIndexOf(to)) + to.Length + indexFrom;

            string substringBefore = html.Substring(0, indexFrom);
            string substringAfter = html.Substring(indexTo, html.Length - indexTo);
            html = substringBefore + substringAfter;
            return html;
        }
        public static string GetHtmlValue(string html, string from, string to, GET_HTML_ORDER order = GET_HTML_ORDER.FIRST)
        {
            html = GetHtml(html, from, to, order);
            html = html.Replace(from, string.Empty);
            html = html.Replace(to, string.Empty);
            return html;
        }
        public static string GetHtml(string html, string from, string to, GET_HTML_ORDER order = GET_HTML_ORDER.FIRST)
        {

            if (!html.Contains(from) || !html.Contains(to))
            {
                return string.Empty;
            }
            else
            {
                html = order == GET_HTML_ORDER.FIRST ? html.Substring(html.IndexOf(from)) : html.Substring(html.LastIndexOf(from));
                html = html.Substring(0, html.IndexOf(to) + to.Length);
                return html;
            }
        }
    }
}
