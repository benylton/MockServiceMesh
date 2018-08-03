using ApiMockServiceMesh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace ApiMockServiceMesh.Util
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var exception = context.Exception;

            HttpResponse response = context.HttpContext.Response;

            var contextException = context.Exception;

            var baseException = contextException.GetBaseException();

            if (contextException.GetType().IsSubclassOf(typeof(TimeoutException)))
            {
                 status = HttpStatusCode.RequestTimeout;
            }

            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            var exceptionString = string.Empty;

            while (exception != null)
            {
                Console.WriteLine(exception.Message);
                exceptionString += exception.Message;
                exception = exception.InnerException;
            }

            context.Result = new JsonResult(new Result
            {
                Success = false,
                Data = new[] { exceptionString }
            });
        }
    }
}
