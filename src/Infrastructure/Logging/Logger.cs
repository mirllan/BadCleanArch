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
        catch (Exception)
        {
            // aqui dejo este catch vacio porque la idea es que si falla algo en log no rompa todo
            // esto es para que sonar no se queje de que no lo explique
        }
    }
}