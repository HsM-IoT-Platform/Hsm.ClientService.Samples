using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hsm.ClientService.ThirdPartyApi.Client;
using Microsoft.Extensions.Logging;

namespace ThirdPartyApi;

public class ThirdPartyApiClientV1DemoService : IDemoService
{
    private readonly IThirdPartyApiClientV1 _thirdPartyApiClientV1;
    private readonly ILogger<ThirdPartyApiClientV1DemoService> _logger;

    public ThirdPartyApiClientV1DemoService(
        IThirdPartyApiClientV1 thirdPartyApiClientV1,
        ILogger<ThirdPartyApiClientV1DemoService> logger)
    {
        _thirdPartyApiClientV1 = thirdPartyApiClientV1;
        _logger = logger;
    }

    public async Task DemoAsync(CancellationToken cancellationToken)
    {
        try
        {
            var equivalents =
                await _thirdPartyApiClientV1.GetResourceEquivalentsResourceEquivalentAsync(cancellationToken);
            var serializedEquivalents = JsonSerializer.Serialize(equivalents, Constants.WriteIndentedSerializerOptions);
            _logger.LogInformation("GetResourceEquivalentsResourceEquivalentAsync: {equivalents}",
                serializedEquivalents);
        }
        catch (ApiException apiException) when (apiException.StatusCode == (int)HttpStatusCode.TooManyRequests)
        {
            _logger.LogInformation("Too many Requests. Please try again later");
        }
        catch (ApiException apiException)
        {
            _logger.LogError(apiException, "Unexpected Exception when calling the endpoint");
        }
    }
}