using System;
using System.Collections.Generic;
using GestionProyectosCiencias.Entidades;

namespace GestionProyectosCiencias.Datos
{
    public class ProyectoDatos
    {
        private static List<Proyecto> listaProyectos = new List<Proyecto>()
        {
            new Proyecto
            {
                IdProyecto = 1,
                Titulo = "Incidencia del voleibol en el trabajo en equipo del Bachillerato Técnico UEMAM 2026-2027",
                Descripcion = "Estudio de caso sobre la práctica del voleibol para fortalecer la colaboración y convivencia en estudiantes del Bachillerato Técnico."
            }
        };

        public List<Proyecto> ObtenerProyectos()
        {
            return listaProyectos;
        }
    }
}