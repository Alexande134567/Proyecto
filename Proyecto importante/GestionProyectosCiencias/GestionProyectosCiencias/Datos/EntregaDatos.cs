using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionProyectosCiencias.Entidades;

namespace GestionProyectosCiencias.Datos
{
    public class EntregaDatos
    {
        private readonly Conexion conexionDB = new Conexion();

        public List<Entrega> ListarEntregasPorProyecto(int idProyecto)
        {
            List<Entrega> lista = new List<Entrega>();
            string query = "SELECT * FROM Entregas WHERE IdProyecto = @idProyecto";

            using (SqlConnection con = conexionDB.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entrega e = new Entrega();

                            if (TieneColumna(reader, "IdEntrega")) e.IdEntrega = Convert.ToInt32(reader["IdEntrega"]);
                            if (TieneColumna(reader, "IdProyecto")) e.IdProyecto = Convert.ToInt32(reader["IdProyecto"]);

                            // Mapeo flexible de Nombre de Etapa o Título
                            if (TieneColumna(reader, "NombreEtapa")) e.NombreEtapa = reader["NombreEtapa"].ToString();
                            else if (TieneColumna(reader, "Titulo")) e.NombreEtapa = reader["Titulo"].ToString();
                            else e.NombreEtapa = "Etapa " + e.IdEntrega;

                            // Mapeo flexible de Fecha Inicio
                            if (TieneColumna(reader, "FechaInicio")) e.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                            else if (TieneColumna(reader, "FechaEntrega")) e.FechaInicio = Convert.ToDateTime(reader["FechaEntrega"]);
                            else e.FechaInicio = DateTime.Now;

                            // Mapeo flexible de Fecha Fin
                            if (TieneColumna(reader, "FechaFin")) e.FechaFin = Convert.ToDateTime(reader["FechaFin"]);
                            else e.FechaFin = e.FechaInicio.AddDays(5);

                            // Mapeo flexible de Estado
                            if (TieneColumna(reader, "Estado")) e.Estado = reader["Estado"].ToString();
                            else e.Estado = "Pendiente";

                            lista.Add(e);
                        }
                    }
                }
            }
            return lista;
        }

        public bool RegistrarEntrega(Entrega entrega)
        {
            string query = "INSERT INTO Entregas (IdProyecto, NombreEtapa, FechaInicio, FechaFin, Estado) " +
                           "VALUES (@IdProyecto, @NombreEtapa, @FechaInicio, @FechaFin, @Estado)";

            using (SqlConnection con = conexionDB.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdProyecto", entrega.IdProyecto);
                    cmd.Parameters.AddWithValue("@NombreEtapa", string.IsNullOrEmpty(entrega.NombreEtapa) ? (entrega.Titulo ?? "Nueva Etapa") : entrega.NombreEtapa);
                    cmd.Parameters.AddWithValue("@FechaInicio", entrega.FechaInicio == default ? DateTime.Now : entrega.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", entrega.FechaFin == default ? DateTime.Now.AddDays(7) : entrega.FechaFin);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(entrega.Estado) ? "Pendiente" : entrega.Estado);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private bool TieneColumna(SqlDataReader reader, string nombreColumna)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}