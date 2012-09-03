using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MVC4Base.Models
{
    public class DebugWindowService
    {
        public static void Write(StringBuilder writer)
        {
            if (HttpContext.Current.Session["DebugWindow"] != null)
            {
                writer.Append(HttpContext.Current.Session["DebugWindow"]);
            }
            HttpContext.Current.Session["DebugWindow"] = writer.ToString();
        }

        public static string Print()
        {
            string strResult = string.Empty;
            if (HttpContext.Current.Session["DebugWindow"] != null)
            {
                strResult = HttpContext.Current.Session["DebugWindow"].ToString();
                HttpContext.Current.Session["DebugWindow"] = null;
            }
            return strResult;
        }
    }
}