using System.Threading.Tasks;

namespace K2Spy.Model
{
    public interface IInitializedExtension : Model.IExtension
    {
        Task InitializedAsync(K2SpyContext k2SpyContext);
    }
}
