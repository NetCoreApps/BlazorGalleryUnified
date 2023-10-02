using System.Net;
using MyApp;
using MyApp.Client;
using MyApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents()
    .AddWebAssemblyComponents();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// builder.Services.AddScoped<BlazorWasmAuthContext>();
// builder.Services.AddScoped<BlazorServerAuthContext>();
// builder.Services.AddScoped<ServiceStackStateProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServiceStackStateProvider>());


var baseUrl = builder.Configuration["ApiBaseUrl"] ?? 
              (builder.Environment.IsDevelopment() ? "https://localhost:7086" : "http://" + IPAddress.Loopback);


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });
builder.Services.AddBlazorServerApiClient(baseUrl);
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseHttpsRedirection();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseServiceStack(new AppHost());

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.MapRazorComponents<App>()
    .AddServerRenderMode()
    .AddWebAssemblyRenderMode();

app.Run();

