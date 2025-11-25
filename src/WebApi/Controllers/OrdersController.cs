using System;

// BAD: Mixing minimal APIs with Controllers folder just to confuse structure
namespace WebApi.Controllers
{
	public static class OrdersController /* No ControllerBase, no attributes: kept intentionally minimal */
	{
		// Convertido a static para evitar la advertencia de Sonar sobre método de instancia sin estado.
		public static string DoNothing() => "This controller does nothing. Endpoints are in Program.cs";
	}
}