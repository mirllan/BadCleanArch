using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

using System.Data;
using Microsoft.Data.SqlClient;

public static class BadDb
{
	// Ahora es propiedad. NO incluye contraseña por defecto.
	// La contraseña/connstring real debe venir desde configuration/secrets (appsettings/Secret).
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