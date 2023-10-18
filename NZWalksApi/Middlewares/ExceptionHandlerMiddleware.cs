﻿using System;
using System.Net;

namespace NZWalksApi.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
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
                var errorId = Guid.NewGuid();
                //Log this exception
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                //return a custom errot response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "/application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessge = "Something went wrong! We are looking into resolving this."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
	}
}

