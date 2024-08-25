using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace WebAPI.Models
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // First, enable detailed error pages for development.
                app.UseSwagger();                 // Then, enable Swagger to generate the Swagger JSON endpoint.
                app.UseSwaggerUI();               // Finally, enable the Swagger UI.
            }

            else
            {
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                var ex = context.Features.Get<IExceptionHandlerFeature>();
                                if (ex != null)
                                {
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
