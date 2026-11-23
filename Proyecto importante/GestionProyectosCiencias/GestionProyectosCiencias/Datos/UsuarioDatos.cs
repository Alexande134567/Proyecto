using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GestionProyectosCiencias.Entidades;

namespace GestionProyectosCiencias.Datos
{
    public class UsuarioDatos
    {
        private static List<Usuario> listaUsuarios = new List<Usuario>()
        {
            new Usuario { IdUsuario = 1, NombreUsuario = "admin", Clave = "admin123", NombreCompleto = "Administrador del Sistema", IdRol = 1 },
            new Usuario { IdUsuario = 2, NombreUsuario = "adelgado", Clave = "tutora2026", NombreCompleto = "Lic. Adriana Delgado Mg. (Tutora)", IdRol = 2 },
            new Usuario { IdUsuario = 3, NombreUsuario = "kintriago", Clave = "docente2026", NombreCompleto = "Lic. Katherin Johana Intriago Carreña (Docente Guía)", IdRol = 2 },
            new Usuario { IdUsuario = 4, NombreUsuario = "dalcivar", Clave = "estudiante123", NombreCompleto = "Alcívar Farías Diego Joseph", IdRol = 3 },
            new Usuario { IdUsuario = 5, NombreUsuario = "dmacias", Clave = "estudiante123", NombreCompleto = "Macías Briones Darwin Isaac", IdRol = 3 }
        };

        // Devuelve DataTable para que FormGestionUsuarios no falle con el DataSource (Error CS0029)
        public DataTable ObtenerUsuarios()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdUsuario", typeof(int));
            dt.Columns.Add("NombreUsuario", typeof(string));
            dt.Columns.Add("Clave", typeof(string));
            dt.Columns.Add("NombreCompleto", typeof(string));
            dt.Columns.Add("IdRol", typeof(int));

            foreach (var u in listaUsuarios)
            {
                dt.Rows.Add(u.IdUsuario, u.NombreUsuario, u.Clave, u.NombreCompleto, u.IdRol);
            }

            return dt;
        }

        // Sobrecargas para Insertar
        public bool InsertarUsuario(string nombreUsuario, string clave, string nombreCompleto, int idRol)
        {
            int nuevoId = listaUsuarios.Count > 0 ? listaUsuarios.Max(u => u.IdUsuario) + 1 : 1;
            listaUsuarios.Add(new Usuario { IdUsuario = nuevoId, NombreUsuario = nombreUsuario, Clave = clave, NombreCompleto = nombreCompleto, IdRol = idRol });
            return true;
        }

        public bool InsertarUsuario(Usuario usuario)
        {
            usuario.IdUsuario = listaUsuarios.Count > 0 ? listaUsuarios.Max(u => u.IdUsuario) + 1 : 1;
            listaUsuarios.Add(usuario);
            return true;
        }

        // Sobrecargas para Actualizar
        public bool ActualizarUsuario(int idUsuario, string nombreUsuario, string clave, string nombreCompleto, int idRol)
        {
            var u = listaUsuarios.FirstOrDefault(x => x.IdUsuario == idUsuario);
            if (u != null)
            {
                u.NombreUsuario = nombreUsuario;
                u.Clave = clave;
                u.NombreCompleto = nombreCompleto;
                u.IdRol = idRol;
                return true;
            }
            return false;
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            var index = listaUsuarios.FindIndex(u => u.IdUsuario == usuario.IdUsuario);
            if (index != -1)
            {
                listaUsuarios[index] = usuario;
                return true;
            }
            return false;
        }

        public bool EliminarUsuario(int idUsuario)
        {
            var u = listaUsuarios.FirstOrDefault(x => x.IdUsuario == idUsuario);
            if (u != null)
            {
                listaUsuarios.Remove(u);
                return true;
            }
            return false;
        }
    }
}