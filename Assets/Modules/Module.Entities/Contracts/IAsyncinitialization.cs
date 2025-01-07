using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Entities
{
    public interface IAsyncinitialization
    {
        UniTask InitializeAsync(CancellationToken token);
    }
}
