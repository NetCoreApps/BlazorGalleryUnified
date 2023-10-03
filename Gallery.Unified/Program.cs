using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyApp;
using MyApp.Client;

Licensing.RegisterLicense("OSS BSD-3-Clause 2023 https://github.com/NetCoreApps/BlazorGallery WZ4hSuvzmoeRsZSygYGa3b7y1F+ohVZhYvHsfbRBw3hr0Jhyk/xSrPOl86g8St+H9zll7ehDjG5D5176JvP9baU3zIoZKek3+RvDbr+Th/COMaXrnByIA/pBjIR7aApF6l8tLXA4qV05rEE7wNxla74QGCupdH5NU2r2algvLTU=");


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents()
    .AddWebAssemblyComponents();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
// builder.Services.AddScoped<BlazorWasmAuthContext>();
// builder.Services.AddScoped<BlazorServerAuthContext>();
// builder.Services.AddScoped<ServiceStackStateProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServiceStackStateProvider>());


var baseUrl = builder.Configuration["ApiBaseUrl"] ?? 
              (builder.Environment.IsDevelopment() ? "https://localhost:7086" : "http://" + IPAddress.Loopback);


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });
builder.Services.AddBlazorServerApiClient(baseUrl);
builder.Services.AddLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// app.UseRouting();

app.UseServiceStack(new AppHost());

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapRazorPages();
//     endpoints.MapControllers();
//     endpoints.MapFallbackToFile("index.html");
// });

// app.MapRazorPages();
// app.MapControllers();
// app.MapFallbackToFile("index.html");

// app.UseBlazorFrameworkFiles();


BlazorConfig.Set(new BlazorConfig
{
    IsWasm = true,
    Services = app.Services,
    FallbackAssetsBasePath = baseUrl,
    EnableLogging = true,
    EnableVerboseLogging = app.Environment.IsDevelopment(),
});

app.MapRazorComponents<App>()
    .AddServerRenderMode()
    .AddWebAssemblyRenderMode();

app.Run();

