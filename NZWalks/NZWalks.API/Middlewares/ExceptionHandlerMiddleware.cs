namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // Log the exception details
                logger.LogError(ex, $"{errorId} : {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                // Return a custom error response
                var response = new
                {
                    Id = errorId,
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please try again later."
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
