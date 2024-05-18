using BApp;
using BApp.Services.Interfaces;
using BApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var dotNetClient = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7235/api/") // Adresa de bază pentru API-ul .NET
};

var pythonClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5000/") // Adresa de bază pentru API-ul Python
};

builder.Services.AddSingleton(dotNetClient);
builder.Services.AddSingleton(pythonClient);

builder.Services.AddScoped<IImageService>(sp => new ImageService(pythonClient));
builder.Services.AddScoped<IColorService>(sp => new ColorService(dotNetClient));
builder.Services.AddScoped<IUserService>(sp => new UserService(dotNetClient));
builder.Services.AddScoped<IUserColorService>(sp => new UserColorService(dotNetClient));
builder.Services.AddScoped<IUserLevelService>(sp => new UserLevelService(dotNetClient));


//// Registering services
//builder.Services.AddScoped<IColorService, ColorService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserColorService, UserColorService>();
//builder.Services.AddScoped<IUserLevelService, UserLevelService>();

//builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
