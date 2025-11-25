using System;

namespace Infrastructure.Logging;

public static class Logger
{
    // aqui deje esta propiedad porque sonar no queria que fuera un campo publico
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
        catch (Exception ex)
        {
            // aqui solo logueo la excepcion para que sonar no diga nada y no rompa la app
            Log("excepcion atrapada en Try " + ex.Message);
        }
    }
}
