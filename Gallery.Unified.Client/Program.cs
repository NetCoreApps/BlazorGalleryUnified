using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.Extensions.Logging;

Licensing.RegisterLicense("OSS BSD-3-Clause 2023 https://github.com/NetCoreApps/BlazorGallery WZ4hSuvzmoeRsZSygYGa3b7y1F+ohVZhYvHsfbRBw3hr0Jhyk/xSrPOl86g8St+H9zll7ehDjG5D5176JvP9baU3zIoZKek3+RvDbr+Th/COMaXrnByIA/pBjIR7aApF6l8tLXA4qV05rEE7wNxla74QGCupdH5NU2r2algvLTU=");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddLogging(c => c
    .AddBrowserConsole()
    .SetMinimumLevel(LogLevel.Trace)
);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServiceStackStateProvider>());


var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddBlazorApiClient(apiBaseUrl);


await builder.Build().RunAsync();
