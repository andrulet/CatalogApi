using CatalogApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CatalogApi.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null || user.IsAdmin != true)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "You aren't admin." }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}