using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SPD.MVC.Geral.Utilities
{
    /// <summary>
    /// Classe com métodos utilitários para manipulação de coleções e primitivas e código html.
    /// </summary>
    public static class ExtensionMethods
    {
        public static MvcHtmlString ExternalLink(this HtmlHelper htmlHelper, string linkText, string externalUrl)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            tagBuilder.Attributes["href"] = externalUrl;
            tagBuilder.InnerHtml = linkText;

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ExternalLink(this HtmlHelper htmlHelper, string linkText, string externalUrl, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            tagBuilder.Attributes["href"] = externalUrl;
            tagBuilder.InnerHtml = linkText;

            foreach (var property in htmlAttributes.GetType().GetProperties())
            {
                tagBuilder.Attributes[property.Name] = property.GetValue(htmlAttributes).ToString();
            }

            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static BigInteger TextToInteger(string text)
        {
            byte[] startBytes = Encoding.UTF8.GetBytes(text);
            return new BigInteger(startBytes);
        }

        private static string IntegerToText(BigInteger integer)
        {
            byte[] endBytes = integer.ToByteArray();
            return Encoding.UTF8.GetString(endBytes);
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T precisa conter um tipo enumerador.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }
            }
            return val;
        }

        public static T GetEnumValueStr<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T precisa conter um tipo enumerador.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }

        public static string RenderToString(this PartialViewResult partialView)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                throw new NotSupportedException("É necessário um contexto HTTP para retornar a visualização parcial de uma string");
            }

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(httpContext.Request.RequestContext, controllerName);

            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);

            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            return sb.ToString();
        }

        public static int FindMaxValue<T>(List<T> list, Converter<T, int> projection)
        {
            if (list.Count == 0)
            {
                return 0;
            }
            int maxValue = int.MinValue;
            foreach (T item in list)
            {
                int value = projection(item);
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }

        public static bool HasPlaceholder(this string content)
        {
            return Regex.IsMatch(content, "{\\d+}");
        }


        
    }

    /// <summary>
    /// Classe utilitária para renderização de colunas de checkbox.
    /// </summary>
    public class CheckboxColumnRenderer : GridHeaderRenderer
    {
        private readonly HtmlHelper _helper;

        public CheckboxColumnRenderer(HtmlHelper helper)
        {
            _helper = helper;
        }
    }
}
