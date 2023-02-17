using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISD.API2.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                ApiResponse res = new ApiResponse();
                res.IsSuccess = false;

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        res.Message = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        res.Message = ex.InnerException.Message;
                    }
                }
                else
                {
                    res.Message = ex.Message;
                }
                var exType = ex.GetType();
                switch (ex)
                {
                    case SqlException:
                        res.Code = (int)HttpStatusCode.BadRequest;
                        break;
                    case ResourceNotFoundException:
                        res.Code = (int)HttpStatusCode.NotFound;
                        break;
                    case AppException:
                        res.Code = (int)HttpStatusCode.BadRequest;                        
                        break;
                    default:
                        // unhandled error
                        res.Code = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(res));
            }
        }
        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
            
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    return context.Response.WriteAsync(new ApiResponse()
        //    {
        //        Code = context.Response.StatusCode,
        //        Message = "Internal Server Error from the custom middleware."
        //    }.ToString());
        //}
    }
}
