using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddLogging(c => c
    .AddBrowserConsole()
    .SetMinimumLevel(LogLevel.Trace)
);
await builder.Build().RunAsync();
