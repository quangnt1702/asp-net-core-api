using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Second_Project.Helpers;
using Second_Project.ViewModels;

namespace Second_Project.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (e)
                {
                    case AppException:
                        //custom
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException:
                        //not found
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        break;
                    case AuthenticationException:
                        //unauthenticated
                        response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        break;
                    default:
                        //unhandled error
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }

                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    Code = response.StatusCode,
                    Message = e.Message
                };
                var result = JsonSerializer.Serialize(errorResponseModel);
                await response.WriteAsync(result);
            }
        }
    }
}