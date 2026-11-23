namespace GestionProyectosCiencias.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        // Propiedad principal
        public string Nombre { get; set; }

        // Propiedades auxiliares para mantener compatibilidad con Login y Gestión de Usuarios
        public string NombreUsuario
        {
            get => Nombre;
            set => Nombre = value;
        }

        public string NombreCompleto
        {
            get => Nombre;
            set => Nombre = value;
        }

        public string Correo { get; set; }
        public string Clave { get; set; }
        public int IdRol { get; set; }
        public string Telefono { get; set; }
    }
}