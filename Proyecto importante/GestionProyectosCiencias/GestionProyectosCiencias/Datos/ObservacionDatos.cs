using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionProyectosCiencias.Datos
{
    public class ObservacionDatos
    {
        private readonly Conexion conexionBD = new Conexion();

        public DataTable ObtenerEntregasConObservaciones()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = @"SELECT e.IdEntrega, 
                                        p.NombreProyecto, 
                                        ISNULL(o.Comentario, '') AS Observacion, 
                                        e.FechaEntrega
                                 FROM Entregas e
                                 INNER JOIN Proyectos p ON e.IdProyecto = p.IdProyecto
                                 LEFT JOIN Observaciones o ON e.IdEntrega = o.IdEntrega";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool GuardarObservacion(int idEntrega, int idUsuario, string comentario)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                string query = @"IF EXISTS (SELECT 1 FROM Observaciones WHERE IdEntrega = @IdEntrega)
                                    UPDATE Observaciones 
                                    SET Comentario = @Comentario, FechaObservacion = GETDATE(), IdUsuario = @IdUsuario
                                    WHERE IdEntrega = @IdEntrega
                                 ELSE
                                    INSERT INTO Observaciones (IdEntrega, IdUsuario, Comentario, FechaObservacion)
                                    VALUES (@IdEntrega, @IdUsuario, @Comentario, GETDATE());

                                 -- Crear notificación automática para el estudiante
                                 IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Notificaciones')
                                 BEGIN
                                     INSERT INTO Notificaciones (IdUsuarioReceptor, Titulo, Mensaje, TipoNotificacion, IdProyecto, Leida)
                                     SELECT e.IdEstudiante, 'Nueva Observación en tu Entrega', 
                                            'El profesor ha registrado una observación en tu entrega.', 
                                            'Observacion', e.IdProyecto, 0
                                     FROM Entregas e
                                     WHERE e.IdEntrega = @IdEntrega;
                                 END";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdEntrega", idEntrega);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Comentario", comentario);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}