using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionProyectosCiencias.Datos
{
    public class NotificacionDatos
    {
        private readonly Conexion conexionBD = new Conexion();

        public DataTable ObtenerNotificacionesPorUsuario(int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = "SELECT * FROM Notificaciones ORDER BY IdNotificacion DESC";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public int ContarNoLeidas(int idUsuario)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = "SELECT COUNT(*) FROM Notificaciones WHERE Leida = 0 OR Leida IS NULL";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool CrearNotificacion(int idUsuario, string titulo, string mensaje, string tipoNotificacion)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                // Usamos la columna exacta: IdUsuarioReceptor
                string query = @"INSERT INTO Notificaciones (IdUsuarioReceptor, Titulo, Mensaje, FechaNotificacion, TipoNotificacion, Leida) 
                                 VALUES (@IdUsuarioReceptor, @Titulo, @Mensaje, GETDATE(), @TipoNotificacion, 0)";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdUsuarioReceptor", idUsuario <= 0 ? 1 : idUsuario);
                    cmd.Parameters.AddWithValue("@Titulo", string.IsNullOrEmpty(titulo) ? (object)DBNull.Value : titulo);
                    cmd.Parameters.AddWithValue("@Mensaje", string.IsNullOrEmpty(mensaje) ? (object)DBNull.Value : mensaje);
                    cmd.Parameters.AddWithValue("@TipoNotificacion", string.IsNullOrEmpty(tipoNotificacion) ? "Observacion" : tipoNotificacion);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool MarcarComoLeida(int idNotificacion)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = "UPDATE Notificaciones SET Leida = 1 WHERE IdNotificacion = @IdNotificacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdNotificacion", idNotificacion);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool EliminarNotificacion(int idNotificacion)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = "DELETE FROM Notificaciones WHERE IdNotificacion = @IdNotificacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdNotificacion", idNotificacion);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}