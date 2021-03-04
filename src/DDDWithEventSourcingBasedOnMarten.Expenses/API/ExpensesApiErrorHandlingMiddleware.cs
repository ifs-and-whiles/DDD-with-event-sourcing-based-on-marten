using System;
using System.Net;
using System.Threading.Tasks;
using DDDWithEventSourcingBasedOnMarten.Domain;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using DDDWithEventSourcingBasedOnMarten.Marten;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.API
{
    public class ExpensesApiErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILogger Logger = Log.ForContext<ExpensesApiErrorHandlingMiddleware>();
        
        public ExpensesApiErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments($"/{ExpensesApiRouteConsts.Prefix}"))
            {
                try
                {
                    await _next(context);
                }
                catch (DomainException exception)
                {
                    LogErrorWithRequestInfo(exception, context);

                    await HandleDomainExceptionAsync(context, exception);
                }
                catch (NotFoundException exception)
                {
                    LogWarningWithRequestInfo(exception, context);

                    await HandleNotFoundExceptionAsync(context);
                }
                catch (Exception exception)
                {
                    LogErrorWithRequestInfo(exception, context);

                    await HandleExceptionAsync(context);
                }
            }
            else 
                //Skip processing if request does to belong to Expenses API 
                await _next(context);

        } 
        
        private void LogErrorWithRequestInfo(Exception exception, HttpContext context)
        {
            Logger.Error(exception, exception.Message + " Request: {@RequestContext}", new
            {
                context?.Request?.Protocol,
                context?.Request?.Method,
                context?.Request?.Path,
                context?.Request?.QueryString
            });
        }

        private void LogWarningWithRequestInfo(Exception exception, HttpContext context)
        {
            Logger.Warning(exception, exception.Message + " Request: {@RequestContext}", new
            {
                context?.Request?.Protocol,
                context?.Request?.Method,
                context?.Request?.Path,
                context?.Request?.QueryString
            });
        }

        private Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            var code = HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.ErrorCode
            }.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Server error occured, please contact with administrator."
            }.ToString());

        }
        
        private Task HandleNotFoundExceptionAsync(HttpContext context)
        {
            var code = HttpStatusCode.NotFound;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Requested element not found"
            }.ToString());
        }
    }
    
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}