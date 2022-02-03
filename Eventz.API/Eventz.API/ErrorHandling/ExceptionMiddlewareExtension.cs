using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventz.API.ErrorHandling
{
    public class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(IApplicationBuilder appError)
        {
            appError.Run(async context =>
            {

                context.Response.StatusCode = context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;// or another Status accordingly to Exception Type
                context.Response.ContentType = "application/json";
                var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                if (exceptionHandlerPathFeature != null)
                {
                 //   //var ex = error.Error;
                 //   logger.Error($"The path {exceptionHandlerPathFeature.Path} " +
                 //$"threw an exception {exceptionHandlerPathFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = exceptionHandlerPathFeature.Error.Message// or your custom message
                                                                           // other custom data
                    }.ToString(), System.Text.Encoding.UTF8);
                }
            });
        }
    }
}
