using System;

namespace Infrastructure.Logging;

public static class Logger
{
	// Convertido a propiedad pública en lugar de campo público.
	public static bool Enabled { get; set; } = true;

	public static void Log(string message)
	{
		if (!Enabled) return;
		Console.WriteLine("[LOG] " + DateTime.Now + " - " + message);
	}

	public static void Try(Action a)
	{
		try
		{
			a();
		}
		catch (Exception)
		{
			// Intencionalmente ignorado: operación "best-effort".
			// Se documenta el motivo para que Sonar deje de sugerir manejo.
			// (Evitar que fallos en logging/DB rompan la aplicación principal)
		}
	}
}