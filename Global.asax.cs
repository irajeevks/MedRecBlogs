using MedrecTechnologies.Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedrecTechnologies.Blog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_BeginRequest()
        {
            bool RedirectToNewUrl = false;
            var requestUrl = Context.Request.Url.ToString().ToLower();
            if (!requestUrl.StartsWith("http://localhost"))
            {
                if (!Context.Request.IsSecureConnection)
                {
                    requestUrl = requestUrl.Insert(4, "s");
                    RedirectToNewUrl = true;
                }
                if (requestUrl.Contains("www."))
                {
                    requestUrl = requestUrl.Replace("www.", "");
                    RedirectToNewUrl = true;
                }

                String LastFiveChars = requestUrl.Substring((requestUrl.Length - 5), 5);
                if (LastFiveChars.Equals("/home"))
                {
                    requestUrl = requestUrl.Replace(LastFiveChars, "");
                    RedirectToNewUrl = true;
                }

                if (RedirectToNewUrl)
                {
                    //Response.Clear();
                    //Response.Status = "301 Moved Permanently";
                    //Response.AddHeader("Location", requestUrl);
                    //Response.End();
                }
            }
        }
    }
}
