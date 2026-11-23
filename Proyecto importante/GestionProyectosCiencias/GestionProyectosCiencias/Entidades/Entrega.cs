using System;

namespace GestionProyectosCiencias.Entidades
{
    public class Entrega
    {
        public int IdEntrega { get; set; }
        public int IdProyecto { get; set; }
        public string NombreEtapa { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
        public string RutaArchivo { get; set; }
        public int IdEstadoEntrega { get; set; }
    }
}