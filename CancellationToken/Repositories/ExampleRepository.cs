using CancellationTokenExampl.Contracts;

namespace CancellationTokenExampl.Repositories
{
    public class ExampleRepository : IExampleRepository
    {
        private readonly ILogger<ExampleRepository> logger;

        public ExampleRepository(ILogger<ExampleRepository> logger) =>
            this.logger = logger;

        public async Task<int> GetExampleDataAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("GetExampleDataAsync started");

            //First action, for example DB call.
            await Task.Delay(2000, cancellationToken);

            logger.LogInformation("First action completed");

            //First action, for example API call.
            await Task.Delay(2000, cancellationToken);

            logger.LogInformation("Seconds action completed");

            return 0;
        }
    }
}
