using System;

namespace GestionProyectosCiencias.Entidades
{
    public class ProyectoIntegrante
    {
        public int IdProyectoIntegrante { get; set; }
        public int IdProyecto { get; set; }
        public int IdUsuario { get; set; }
        public string RolProyecto { get; set; }
    }
}