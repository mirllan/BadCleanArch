using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

using System.Data;
using Microsoft.Data.SqlClient;

public static class BadDb
{
    // aqui deje esta propiedad sin contrasena porque eso debe venir de la configuracion o de secretos
    public static string ConnectionString { get; set; } = "Server=localhost;Database=master;TrustServerCertificate=True";

    public static int ExecuteNonQueryUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteReader();
    }
}
