﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using WebApiMoviesOdataCF.Models;

namespace WebApiMoviesOdataCF
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            // Web API configuration and services
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Movie>("Movies");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
