namespace be_project_swp.Core.Interfaces
{
    public interface IHostInitialization
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
