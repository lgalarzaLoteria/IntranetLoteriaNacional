using IntranetLoteriaNacional.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazored.SessionStorage;
using Blazored.LocalStorage;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Http.Features;
using IntranetLoteriaNacional.Shared.Constants;
using CurrieTechnologies.Razor.SweetAlert2;
using DevExpress.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSweetAlert2();
builder.Services.AddSingleton<StoreCheckService>();
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

/***Servicio para peticiones livianas***/
if (!builder.Services.Any(x => x.ServiceType == typeof(HttpClient)))
{
    builder.Services.AddScoped<HttpClient>(s =>
    {
        var uriHelper = s.GetRequiredService<NavigationManager>();
        return new HttpClient
        {
            BaseAddress = new Uri("http://jbg15pp03/APILoteriaNacional/api/")
        };
    });
}

builder.Services.AddScoped<ProjectHttpClient>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; //100 megabytes
});

builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQllhS3xRdkVhXnxedHY=;Mgo+DSMBPh8sVXJ1S0d+X1RPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpRdEdhWX9cd3dUQWI=;ORg4AjUWIQA/Gnt2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEdhW39ecnNWRGle;MTgxMTQyNEAzMjMxMmUzMTJlMzMzNWgvRjVXcmVGam1sMDlQZ01kdzE2Sm5MTFRQWEo5NUoyaTdnRTlJMEJxMWs9;MTgxMTQyNUAzMjMxMmUzMTJlMzMzNUhyOXIyeEVzbXpTUjY2TWQvY2xHZjF3RFBOakNSMXRXT3kvWjRsSExoUk09;NRAiBiAaIQQuGjN/V0d+XU9Hc1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckdhW31ecHZSRWlbUw==;MTgxMTQyN0AzMjMxMmUzMTJlMzMzNUJYT09TblNDTHJNaytRWDY1eW5ZVExVT0pwWGhOMk9vblBxT05VTVN1Q3M9;MTgxMTQyOEAzMjMxMmUzMTJlMzMzNU41VkxXalQ1ZW5CZjhlZXgwN0huM1VJQm45dkVLdFdncXN0UjNSS1U1VWc9;Mgo+DSMBMAY9C3t2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEdhW39ecnNRQGle;MTgxMTQzMEAzMjMxMmUzMTJlMzMzNUtYYzJxS2h0MDE0V1FCcmJ6OGdJUms2VnduZFJRWlpwYUtyRzFBVGpiZFU9;MTgxMTQzMUAzMjMxMmUzMTJlMzMzNWVMZzVZQnRhR0xDNE0yUzdhcDJFUkU1OFh0bXhjNjdwNExxM1hyRFozdmc9;MTgxMTQzMkAzMjMxMmUzMTJlMzMzNUJYT09TblNDTHJNaytRWDY1eW5ZVExVT0pwWGhOMk9vblBxT05VTVN1Q3M9;Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCfEx0WmFZfVpgcF9HY1ZQQ2Y/P1ZhSXxXdkRhX35YcXdUQWBYUUM=");

builder.WebHost.UseStaticWebAssets();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQllhS3xRdkVhXnxedHY=;Mgo+DSMBPh8sVXJ1S0d+X1RPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpRdEdhWX9cd3dUQWI=;ORg4AjUWIQA/Gnt2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEdhW39ecnNWRGle;MTgxMTQyNEAzMjMxMmUzMTJlMzMzNWgvRjVXcmVGam1sMDlQZ01kdzE2Sm5MTFRQWEo5NUoyaTdnRTlJMEJxMWs9;MTgxMTQyNUAzMjMxMmUzMTJlMzMzNUhyOXIyeEVzbXpTUjY2TWQvY2xHZjF3RFBOakNSMXRXT3kvWjRsSExoUk09;NRAiBiAaIQQuGjN/V0d+XU9Hc1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckdhW31ecHZSRWlbUw==;MTgxMTQyN0AzMjMxMmUzMTJlMzMzNUJYT09TblNDTHJNaytRWDY1eW5ZVExVT0pwWGhOMk9vblBxT05VTVN1Q3M9;MTgxMTQyOEAzMjMxMmUzMTJlMzMzNU41VkxXalQ1ZW5CZjhlZXgwN0huM1VJQm45dkVLdFdncXN0UjNSS1U1VWc9;Mgo+DSMBMAY9C3t2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdEdhW39ecnNRQGle;MTgxMTQzMEAzMjMxMmUzMTJlMzMzNUtYYzJxS2h0MDE0V1FCcmJ6OGdJUms2VnduZFJRWlpwYUtyRzFBVGpiZFU9;MTgxMTQzMUAzMjMxMmUzMTJlMzMzNWVMZzVZQnRhR0xDNE0yUzdhcDJFUkU1OFh0bXhjNjdwNExxM1hyRFozdmc9;MTgxMTQzMkAzMjMxMmUzMTJlMzMzNUJYT09TblNDTHJNaytRWDY1eW5ZVExVT0pwWGhOMk9vblBxT05VTVN1Q3M9");

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
