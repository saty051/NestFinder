using System.Text.Json;

namespace WebAPI.Errors
{
    // This class represents a standardized error response that can be used in an API.
    public class ApiError
    {

        public ApiError() { }
        // Constructor to initialize the ApiError object with required and optional properties.
        // errorCode: The unique code representing the error type.
        // errorMessage: A brief message describing the error.
        // errorDetails (optional): Additional details about the error, if any.
        public ApiError(int errorCode, string errorMessage, string errorDetails = null)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        // Property to store the error code.
        public int ErrorCode { get; set; }

        // Property to store the error message.
        public string ErrorMessage { get; set; }

        // Property to store additional details about the error.
        public string ErrorDetails { get; set; }

        // Overrides the default ToString() method to return a JSON string representation of the ApiError object.
        public override string ToString()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
