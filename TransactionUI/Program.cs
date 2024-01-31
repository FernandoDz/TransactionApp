using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TransactionUI;
using TransactionUI.Services;

using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7106/api/") });
builder.Services.AddScoped<ICreditCardService, CreditCardService>();
builder.Services.AddScoped<IStatementService, StatementService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentInfoService, PaymentInfoService>();
builder.Services.AddScoped<IPurchaseInfoService, PurchaseInfoService>();


builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
