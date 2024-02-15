using System.Threading;
using System.Threading.Tasks;

namespace ThirdPartyApi;

public interface IDemoService
{
    Task DemoAsync(CancellationToken cancellationToken);
}