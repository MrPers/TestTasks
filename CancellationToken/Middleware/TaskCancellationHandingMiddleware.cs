using CancellationTokenExampl.Contracts;
using Microsoft.AspNetCore.Http;

namespace CancellationTokenExampl.Middleware
{
    public class TaskCancellationHandingMiddleware
    {
        public readonly RequestDelegate next;
        private readonly ILogger<TaskCancellationHandingMiddleware> logger;

        public TaskCancellationHandingMiddleware(RequestDelegate next, ILogger<TaskCancellationHandingMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e) when (e is OperationCanceledException or TaskCanceledException)
            {
                logger.LogInformation("Task canselled");
            }
        }
    }
}
