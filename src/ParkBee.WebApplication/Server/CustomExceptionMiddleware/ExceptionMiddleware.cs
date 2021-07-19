using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ParkBee.Domain.Exceptions;
using ParkBee.WebApplication.Server.CustomExceptionMiddleware.ProblemDetails;

namespace ParkBee.WebApplication.Server.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
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
                _logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            if (exception is ParkBeeBaseException)
            {
                var greenFluxDomainProblemDetail= new ParkBeeProblemDetail(exception.Message, exception.InnerException?.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var json = JsonConvert.SerializeObject(greenFluxDomainProblemDetail);
                await httpContext.Response.WriteAsync(json);
            }
            else
            {
                Microsoft.AspNetCore.Mvc.ProblemDetails errorDetail = new Microsoft.AspNetCore.Mvc.ProblemDetails();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetail.Title = "Internal server Error from the custom middleware";
                errorDetail.Status = 500;
                var json = JsonConvert.SerializeObject(errorDetail);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
