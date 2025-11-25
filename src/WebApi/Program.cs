using Infrastructure.Data;
using Infrastructure.Logging;
using Application.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddCors(o => o.AddPolicy("bad", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// No registramos CreateOrderUseCase en DI porque ahora Execute es estático.
// Si prefieres inyección, se puede revertir y mantener Execute como instancia.

var app = builder.Build();

// Obtener la cadena de conexión desde secreto/configuración; nunca dejar contraseña hardcodeada.
var configured = app.Configuration["ConnectionStrings:Sql"];
if (!string.IsNullOrWhiteSpace(configured))
{
	BadDb.ConnectionString = configured;
}
else
{
	// Fallback sin credenciales embebidas; en entornos reales usar secretos (GitHub secrets / KeyVault).
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
	// No lanzar System.Exception; usar excepción más específica si es necesario
	if (x % 13 == 0) throw new InvalidOperationException("random failure (simulated)");
	return "ok " + x;
});

// Endpoint que invoca el use case estático
app.MapPost("/orders", (HttpContext http) =>
{
	using var reader = new StreamReader(http.Request.Body);
	var body = reader.ReadToEnd();
	var parts = (body ?? "").Split(',');
	var customer = parts.Length > 0 ? parts[0] : "anon";
	var product = parts.Length > 1 ? parts[1] : "unknown";
	var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
	var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

	// Llamada al método ahora estático
	var order = CreateOrderUseCase.Execute(customer, product, qty, price);

	return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", (IConfiguration cfg) => new
{
	// No exponer secretos ni variables de entorno sensibles.
	env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
	version = "v0.0.1-unsecure"
});

// Preferir await RunAsync en lugar de Run (sonar suggestion)
await app.RunAsync();