using Infrastructure.Data;
using Infrastructure.Logging;
using Application.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddCors(o => o.AddPolicy("bad", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// aqui no registro el use case porque ahora el metodo es estatico y por eso no hace falta ponerlo en di
var app = builder.Build();

// aqui busco la cadena de conexion desde la configuracion para no dejar la contraseña escrita en el codigo
var configured = app.Configuration["ConnectionStrings:Sql"];
if (!string.IsNullOrWhiteSpace(configured))
{
    BadDb.ConnectionString = configured;
}
else
{
    // aqui pongo un valor por defecto sin contraseña y en proyectos reales esto se saca de secretos
    BadDb.ConnectionString = "Server=localhost;Database=master;TrustServerCertificate=True";
}

app.UseCors("bad");

app.Use(async (ctx, next) =>
{
    try { await next(); } catch (Exception ex) { Logger.Log("Unhandled exception: " + ex); await ctx.Response.WriteAsync("oops"); }
});

app.MapGet("/health", () =>
{
    Logger.Log("health ping");
    var x = new Random().Next();
    // aqui no uso exception generica porque sonar dice que es mejor usar una que sea mas especifica
    if (x % 13 == 0) throw new InvalidOperationException("random failure (simulated)");
    return "ok " + x;
});

// aqui se llama el use case estatico para crear la orden
app.MapPost("/orders", (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = reader.ReadToEnd();
    var parts = (body ?? "").Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
    var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

    // aqui llamo el metodo estatico que crea la orden
    var order = CreateOrderUseCase.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", (IConfiguration cfg) => new
{
    // aqui no se muestran datos sensibles ni secretos porque sonar dice que eso es malo
    env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
    version = "v0.0.1-unsecure"
});

// aqui uso runasync porque sonar recomienda usar la version asincrona
await app.RunAsync();
