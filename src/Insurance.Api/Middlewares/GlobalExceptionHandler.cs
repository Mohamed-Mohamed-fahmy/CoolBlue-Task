using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await Problem(context, ex, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                await Problem(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task Problem(HttpContext context, Exception ex, HttpStatusCode code)
        {
            this.logger.LogError(ex, ex?.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = (int)code;

            var result = JsonSerializer.Serialize(new { message = ex?.Message });
            await response.WriteAsync(result);
        }
    }
}
