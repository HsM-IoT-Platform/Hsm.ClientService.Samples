using System;
using System.Net;
using Hsm.ClientService.ThirdPartyApi.Client.Configuration;
using Hsm.ClientService.ThirdPartyApi.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Serilog;
using ThirdPartyApi;

var hostBuilder = Host.CreateDefaultBuilder();

hostBuilder.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

hostBuilder.ConfigureServices((context, services) =>
{
    var thirdPartyApiConfiguration = context.Configuration
        .GetSection(nameof(ThirdPartyApiConfiguration))
        .Get<ThirdPartyApiConfiguration>();

    services.AddThirdPartyApiClient(config =>
        {
            config.BaseUrl = thirdPartyApiConfiguration.BaseUrl;
            //TODO: dont forget to use a valid token (appsettings.json)
            config.PersonalAccessToken = thirdPartyApiConfiguration.PersonalAccessToken;
        },
        clientBuilder => clientBuilder.AddTransientHttpErrorPolicy(policy => policy
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryCount: 5))));

    services.AddTransient<IDemoService, ThirdPartyApiClientV1DemoService>();
});

var host = await hostBuilder.StartAsync();

var examplePerformer = host.Services.GetRequiredService<IDemoService>();
await examplePerformer.DemoAsync(default);

await host.StopAsync();