using Microsoft.AspNetCore.Diagnostics; // Namespace for handling diagnostics and exceptions in ASP.NET Core.
using System.Net;                        // Namespace for HTTP status codes.
using WebAPI.Middlewares;                // Namespace for custom middleware.

namespace WebAPI.Extensions
{
    // This static class contains extension methods for configuring exception handling in the application.
    public static class ExceptionMiddlewareExtensions
    {
        // This method configures the application to use a custom exception handling middleware.
        // app: The IApplicationBuilder provides mechanisms to configure the request pipeline.
        // env: Provides information about the web hosting environment, such as whether it's in development.
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>(); // Registers custom middleware to handle exceptions globally.
        }

        // This method configures the application to use the built-in exception handling mechanisms.
        // app: The IApplicationBuilder provides mechanisms to configure the request pipeline.
        // env: Provides information about the web hosting environment, such as whether it's in development.
        public static void ConfigureBuiltinExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // Enables detailed error pages for development environment.
            }
            else
            {
                // Configures a global exception handler for production environments.
                app.UseExceptionHandler(
                    options =>
                    {
                        // The code inside this block is executed when an unhandled exception occurs.
                        options.Run(
                            async context =>
                            {
                                // Sets the response status code to 500 Internal Server Error.
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                // Retrieves the exception details if available.
                                var ex = context.Features.Get<IExceptionHandlerFeature>();

                                if (ex != null)
                                {
                                    // Writes the error message to the response.
                                    await context.Response.WriteAsync(ex.Error.Message);
                                }
                            }
                        );
                    }
                );
            }
        }
    }
}
