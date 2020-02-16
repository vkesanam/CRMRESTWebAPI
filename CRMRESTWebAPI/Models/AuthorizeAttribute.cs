﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Web.Configuration;
using System.DirectoryServices;

namespace CRMRESTWebAPI.Models
{
    public class AuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
             var authHeader = actionContext.Request.Headers.Authorization;

             if (authHeader != null)
             {
                 if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                 !String.IsNullOrWhiteSpace(authHeader.Parameter))
                 {
                     var credArray = GetCredentials(authHeader);
                     var userName = credArray[0];
                     var password = credArray[1];

                    //string adminUser = WebConfigurationManager.AppSettings["Username"];
                    //string adminPassword = WebConfigurationManager.AppSettings["Password"];
                    string adminUser = "Maximo";
                    string adminPassword = "Maximo";
                    if (userName == adminUser && password == adminPassword)
                     {
                         return;
                     }
                 }
             }
             HandleUnauthorizedRequest(actionContext);
        }
        private string[] GetCredentials(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {

            //Base 64 encoded string
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));

            var credArray = cred.Split(':');

            return credArray;
        }
        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);


            actionContext.Response.Headers.Add("WWW-Authenticate",
            "Basic Scheme='eLearning' location='http://localhost:8323/account/login'");

        }
    }
}