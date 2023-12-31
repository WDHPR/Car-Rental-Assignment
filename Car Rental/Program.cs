using Car_Rental;
using Car_Rental.Business.Classes;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IData, CollectionData>();
builder.Services.AddSingleton<BookingProcessor>();

await builder.Build().RunAsync();
