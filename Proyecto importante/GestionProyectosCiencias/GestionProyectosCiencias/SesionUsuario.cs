namespace GestionProyectosCiencias
{
    public static class SesionUsuario
    {
        public static int IdUsuario { get; set; }
        public static string NombreUsuario { get; set; }
        public static string NombreCompleto { get; set; }
        public static int IdRol { get; set; } // 1: Administrador, 2: Docente, 3: Estudiante

        public static string ObtenerNombreRol()
        {
            switch (IdRol)
            {
                case 1: return "Administrador";
                case 2: return "Docente";
                case 3: return "Estudiante";
                default: return "Sin Rol";
            }
        }
    }
}