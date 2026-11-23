using System;

namespace GestionProyectosCiencias.Entidades
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        public int IdUsuarioReceptor { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public string TipoNotificacion { get; set; }
        public int? IdProyecto { get; set; }
        public bool Leida { get; set; }
    }
}