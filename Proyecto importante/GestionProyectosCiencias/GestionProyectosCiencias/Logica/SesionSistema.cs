using GestionProyectosCiencias.Entidades;

namespace GestionProyectosCiencias.Logica
{
    public static class SesionSistema
    {
        public static Usuario UsuarioActual { get; set; }

        public static void CargarSesion(Usuario usuario)
        {
            UsuarioActual = usuario;
        }

        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
