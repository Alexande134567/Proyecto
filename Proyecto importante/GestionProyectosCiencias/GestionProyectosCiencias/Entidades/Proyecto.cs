using System;

namespace GestionProyectosCiencias.Entidades
{
    public class Proyecto
    {
        public int IdProyecto { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinEstimada { get; set; }
        public string EstadoGeneral { get; set; }
        public int IdProfesor { get; set; }
    }
}