using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeighDown.Client;
using WeighDown.Client.Providers;
using WeighDown.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<CompetitionsService>();
builder.Services.AddScoped<ContestantsService>();
builder.Services.AddScoped<WeightLogsService>();
builder.Services.AddScoped<WeighInDeadlinesService>();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<ComputerVisionService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

await builder.Build().RunAsync();
