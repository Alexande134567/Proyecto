using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionProyectosCiencias.Entidades;

namespace GestionProyectosCiencias.Datos
{
    public class ProyectoIntegranteDatos
    {
        private readonly Conexion conexionDB = new Conexion();

        public List<ProyectoIntegrante> ObtenerIntegrantesPorProyecto(int idProyecto)
        {
            List<ProyectoIntegrante> lista = new List<ProyectoIntegrante>();
            string query = "SELECT IdProyectoIntegrante, IdProyecto, IdUsuario, RolProyecto FROM ProyectoIntegrantes WHERE IdProyecto = @idProyecto";

            using (SqlConnection con = conexionDB.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ProyectoIntegrante
                            {
                                IdProyectoIntegrante = Convert.ToInt32(reader["IdProyectoIntegrante"]),
                                IdProyecto = Convert.ToInt32(reader["IdProyecto"]),
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                RolProyecto = reader["RolProyecto"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public bool AsignarIntegrante(ProyectoIntegrante integrante)
        {
            string query = "INSERT INTO ProyectoIntegrantes (IdProyecto, IdUsuario, RolProyecto) VALUES (@idProyecto, @idUsuario, @rol)";

            using (SqlConnection con = conexionDB.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", integrante.IdProyecto);
                    cmd.Parameters.AddWithValue("@idUsuario", integrante.IdUsuario);
                    cmd.Parameters.AddWithValue("@rol", integrante.RolProyecto);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}