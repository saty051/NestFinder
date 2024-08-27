using System.Net;                      // Namespace for HTTP status codes.
using WebAPI.Errors;                   // Namespace for custom error classes.

namespace WebAPI.Middlewares
{
    // Middleware to handle exceptions globally within the application.
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;                       // Delegate to the next middleware in the pipeline.
        private readonly ILogger<ExceptionMiddleware> _logger;        // Logger to log error messages.
        private readonly IHostEnvironment _env;                       // Provides information about the hosting environment.

        // Constructor to initialize the middleware with required dependencies.
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // This method is invoked for each HTTP request. It handles exceptions that occur during the request processing.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);  // Calls the next middleware in the pipeline.
            }
            catch (Exception ex)       // Catch any unhandled exceptions.
            {
                ApiError response;                          // Object to hold error response details.
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;  // Default to 500 Internal Server Error.
                string message;                             // Holds the error message to be sent in the response.
                var exceptionType = ex.GetType();           // Get the type of the exception.

                // Check for specific exception types and set appropriate status code and message.
                if (exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorized";
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Some unknown error occurred";
                }

                // If in development environment, include the stack trace in the error response.
                if (_env.IsDevelopment())
                {
                    response = new ApiError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    response = new ApiError((int)statusCode, message);  // In production, only send a generic message.
                }

                _logger.LogError(ex, ex.Message);  // Log the exception.

                context.Response.StatusCode = (int)statusCode;            // Set the response status code.
                context.Response.ContentType = "application/json";        // Set the response content type to JSON.
                await context.Response.WriteAsync(response.ToString());   // Write the error response to the HTTP response.
            }
        }
    }
}
