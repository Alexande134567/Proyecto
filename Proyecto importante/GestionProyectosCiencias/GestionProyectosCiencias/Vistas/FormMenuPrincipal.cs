using GestionProyectosCiencias.Vistas;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GestionProyectosCiencias
{
    public partial class FormMenuPrincipal : Form
    {
        public FormMenuPrincipal()
        {
            InitializeComponent();
            this.Load += FormMenuPrincipal_Load;
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            // Aplica los permisos según el Rol de Sesión
            AplicarPermisosPorRol();

            // Abre el primer módulo disponible según el rol
            if (SesionUsuario.IdRol == 1)
            {
                btnUsuarios_Click(null, null);
            }
            else
            {
                btnProyectos_Click(null, null);
            }
        }

        private void AplicarPermisosPorRol()
        {
            // Si es Docente (2) o Estudiante (3), oculta la gestión de Usuarios
            if (SesionUsuario.IdRol == 2 || SesionUsuario.IdRol == 3)
            {
                btnUsuarios.Visible = false;
            }
            else
            {
                btnUsuarios.Visible = true;
            }
        }

        private void AbrirModulo(string nombreModulo, Form formularioHijo)
        {
            lblTitulo.Text = $"Módulo: {nombreModulo}  |  Usuario: {SesionUsuario.NombreCompleto} ({SesionUsuario.ObtenerNombreRol()})";

            panelContenido.Controls.Clear();

            if (formularioHijo != null)
            {
                formularioHijo.TopLevel = false;
                formularioHijo.FormBorderStyle = FormBorderStyle.None;
                formularioHijo.Dock = DockStyle.Fill;

                panelContenido.Controls.Add(formularioHijo);
                panelContenido.Tag = formularioHijo;

                formularioHijo.Show();
                formularioHijo.BringToFront();
            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e) => AbrirModulo("Usuarios", new FormGestionUsuarios());
        private void btnProyectos_Click(object sender, EventArgs e) => AbrirModulo("Proyectos", new FormGestionProyectos());
        private void btnEntregas_Click(object sender, EventArgs e) => AbrirModulo("Entregas", new FormEntregas());
        private void btnObservaciones_Click(object sender, EventArgs e) => AbrirModulo("Observaciones", new FormObservaciones());
        private void btnGantt_Click(object sender, EventArgs e) => AbrirModulo("Diagrama Gantt", new FormGantt());
        private void btnNotificaciones_Click(object sender, EventArgs e) => AbrirModulo("Notificaciones", new FormNotificaciones());

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }
    }
}