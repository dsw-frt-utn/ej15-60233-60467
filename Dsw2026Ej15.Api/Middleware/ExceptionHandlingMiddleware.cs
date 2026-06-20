using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Dsw2026Ej15.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; //
        public ExceptionHandlingMiddleware(RequestDelegate next) //funcion que puede procesar una peticion hhtp
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

        }
        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = ("Se produjo un error imprevisto en la request.");
            if (ex is ValidationException ve)
            {
                status = HttpStatusCode.BadRequest;
                message = ve.Message;
            }
            var resultado = JsonSerializer.Serialize(new  {error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(resultado);
        }
    }
}
