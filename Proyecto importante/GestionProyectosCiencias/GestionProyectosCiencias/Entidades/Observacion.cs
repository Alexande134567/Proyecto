using System;

namespace GestionProyectosCiencias.Entidades
{
    public class Observacion
    {
        public int IdObservacion { get; set; }
        public int IdEntrega { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaObservacion { get; set; }
    }
}