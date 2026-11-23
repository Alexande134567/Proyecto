using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            ConfigurarInterfazLogin();
        }

        private void ConfigurarInterfazLogin()
        {
            // 1. Ocultar caracteres de la contraseña
            TextBox tbClave = ObtenerTextBoxClave();
            if (tbClave != null)
            {
                tbClave.UseSystemPasswordChar = true;
            }

            // 2. Ocultar el botón de Soporte por WhatsApp y subir el botón Salir
            Button btnWhatsApp = null;
            Button btnSalir = null;

            foreach (Control c in this.Controls)
            {
                if (c is Button btn)
                {
                    if (btn.Text.Contains("WhatsApp") || btn.Name.Contains("WhatsApp"))
                    {
                        btnWhatsApp = btn;
                    }
                    else if (btn.Text.Contains("Salir") || btn.Name.Contains("Salir"))
                    {
                        btnSalir = btn;
                    }
                }
            }

            if (btnWhatsApp != null)
            {
                btnWhatsApp.Visible = false;
                if (btnSalir != null)
                {
                    btnSalir.Top = btnWhatsApp.Top; // Sube "Salir del Sistema" para cerrar el espacio
                }
            }
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            ValidarIngreso();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            ValidarIngreso();
        }

        private void btnWhatsApp_Click(object sender, EventArgs e)
        {
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ValidarIngreso()
        {
            TextBox tbUser = ObtenerTextBoxUsuario();
            TextBox tbPass = ObtenerTextBoxClave();

            string usuario = tbUser?.Text.Trim() ?? "";
            string clave = tbPass?.Text.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show(
                    "Por favor, ingresa tu usuario y contraseña para continuar.",
                    "Campos Requeridos - UEMAM",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string query = @"
                        SELECT IdUsuario, NombreUsuario, NombreCompleto 
                        FROM Usuarios 
                        WHERE (NombreUsuario = @User OR NombreCompleto = @User) 
                          AND Clave = @Pass";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@User", usuario);
                    cmd.Parameters.AddWithValue("@Pass", clave);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SesionUsuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            SesionUsuario.NombreUsuario = reader["NombreUsuario"]?.ToString() ?? usuario;

                            MessageBox.Show(
                                $"¡Bienvenido(a), {SesionUsuario.NombreUsuario}!",
                                "Acceso Concedido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            AbrirMenuPrincipal();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Usuario o contraseña incorrectos. Verifica tus datos e inténtalo de nuevo.",
                                "Error de Autenticación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            if (tbPass != null)
                            {
                                tbPass.Clear();
                                tbPass.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al conectar con la base de datos:\n" + ex.Message,
                    "Error de Conexión SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AbrirMenuPrincipal()
        {
            var tipoMenu = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => typeof(Form).IsAssignableFrom(t) &&
                                     (t.Name.Contains("Menu") || t.Name.Contains("Principal") || t.Name == "Form1") &&
                                     t != typeof(FormLogin));

            if (tipoMenu != null)
            {
                Form menu = (Form)Activator.CreateInstance(tipoMenu);
                menu.FormClosed += (s, args) => this.Close();
                menu.Show();
                this.Hide();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private TextBox ObtenerTextBoxUsuario()
        {
            return this.Controls.OfType<TextBox>()
                .OrderBy(tb => tb.Top)
                .FirstOrDefault();
        }

        private TextBox ObtenerTextBoxClave()
        {
            return this.Controls.OfType<TextBox>()
                .OrderByDescending(tb => tb.Top)
                .FirstOrDefault();
        }
    }
}