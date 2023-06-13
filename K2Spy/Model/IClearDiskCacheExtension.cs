using System.Threading.Tasks;

namespace K2Spy.Model
{
    public interface IClearDiskCacheExtension : IExtension
    {
        void ClearDiskCache();
    }

    public interface IPreloadExtension : IExtension
    {
        int PreloadPriority { get; }

        string PreloaderDescription { get; }

        Task PerformPreloadAsync(K2SpyContext k2SpyContext, ReportProgressDelegate reportProgress, System.Threading.CancellationToken cancellationToken);
    }
}
