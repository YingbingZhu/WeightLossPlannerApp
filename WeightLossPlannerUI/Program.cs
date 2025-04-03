using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WeightLossPlannerUI;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://dev-m50h06zqdmvn5f7l.us.auth0.com/";
    options.ProviderOptions.ClientId = "EMFkB2r1oFpHAUGjPGrFGW2isFQ8Ygpc";
    options.ProviderOptions.ResponseType = "code";
    
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
});

await builder.Build().RunAsync();
