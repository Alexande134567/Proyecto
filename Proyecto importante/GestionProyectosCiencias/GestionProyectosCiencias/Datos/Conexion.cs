using System.Data.SqlClient;

namespace GestionProyectosCiencias.Datos
{
    public class Conexion
    {
        private static string cadenaConexion = "Server=localhost; Database=GestionProyectosDB; Integrated Security=True;";

        // Método de instancia (para los archivos de la carpeta Datos que usan un objeto)
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // Método estático alias/compatibilidad para llamadas directas como Conexion.ObtenerConexionEstatica() o mediante sobrecarga
        public static SqlConnection ObtenerConexionEstatica()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}