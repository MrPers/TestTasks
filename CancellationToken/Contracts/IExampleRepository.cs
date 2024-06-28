namespace CancellationTokenExampl.Contracts
{
    public interface IExampleRepository
    {
        Task<int> GetExampleDataAsync(CancellationToken cancellationToken = default);
    }
}
