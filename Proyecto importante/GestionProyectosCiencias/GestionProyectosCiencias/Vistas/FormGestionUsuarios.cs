using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias.Vistas
{
    public partial class FormGestionUsuarios : Form
    {
        private Panel panelHeader;
        private Label lblTituloHeader;
        private Label lblSubtituloHeader;
        private GroupBox gbxFormulario;

        private Label lblNombreCompleto;
        private TextBox txtNombreCompleto;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblClave;
        private TextBox txtClave;
        private Label lblRol;
        private ComboBox cmbRol;

        private Button btnGuardar;
        private Button btnLimpiar;
        private Button btnEliminar;

        private DataGridView dgvUsuarios;

        private readonly UsuarioDatos usuarioDatos = new UsuarioDatos();
        private int idUsuarioSeleccionado = 0;

        public FormGestionUsuarios()
        {
            InitializeComponent();
            DiseñarFormularioProfesional();
            CargarRoles();
            CargarUsuarios();
        }

        private void DiseñarFormularioProfesional()
        {
            this.Text = "Administración - Gestión de Usuarios";
            this.Size = new Size(820, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // ENCABEZADO
            panelHeader = new Panel
            {
                Size = new Size(820, 70),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(30, 41, 59)
            };

            lblTituloHeader = new Label
            {
                Text = "Gestión de Usuarios del Sistema",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 12),
                AutoSize = true
            };

            lblSubtituloHeader = new Label
            {
                Text = "Administra las cuentas de acceso, roles y permisos de los integrantes.",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(25, 38),
                AutoSize = true
            };

            panelHeader.Controls.Add(lblTituloHeader);
            panelHeader.Controls.Add(lblSubtituloHeader);

            // TARJETA DE FORMULARIO
            gbxFormulario = new GroupBox
            {
                Text = " Datos del Usuario ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(20, 80),
                Size = new Size(765, 175),
                BackColor = Color.White
            };

            lblNombreCompleto = new Label { Text = "Nombre Completo:", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(20, 30), AutoSize = true };
            txtNombreCompleto = new TextBox { Location = new Point(20, 50), Size = new Size(350, 25), Font = new Font("Segoe UI", 9.5f) };

            lblUsuario = new Label { Text = "Usuario de Acceso:", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(20, 85), AutoSize = true };
            txtUsuario = new TextBox { Location = new Point(20, 105), Size = new Size(350, 25), Font = new Font("Segoe UI", 9.5f) };

            lblClave = new Label { Text = "Contraseña:", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(395, 30), AutoSize = true };
            txtClave = new TextBox { Location = new Point(395, 50), Size = new Size(350, 25), Font = new Font("Segoe UI", 9.5f), PasswordChar = '●' };

            lblRol = new Label { Text = "Rol Asignado:", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(395, 85), AutoSize = true };
            cmbRol = new ComboBox { Location = new Point(395, 105), Size = new Size(350, 25), Font = new Font("Segoe UI", 9.5f), DropDownStyle = ComboBoxStyle.DropDownList };

            gbxFormulario.Controls.Add(lblNombreCompleto);
            gbxFormulario.Controls.Add(txtNombreCompleto);
            gbxFormulario.Controls.Add(lblUsuario);
            gbxFormulario.Controls.Add(txtUsuario);
            gbxFormulario.Controls.Add(lblClave);
            gbxFormulario.Controls.Add(txtClave);
            gbxFormulario.Controls.Add(lblRol);
            gbxFormulario.Controls.Add(cmbRol);

            // BOTONES
            btnGuardar = new Button
            {
                Text = "💾 Guardar Usuario",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(34, 197, 94),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 267),
                Size = new Size(200, 36),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnLimpiar = new Button
            {
                Text = "🧹 Limpiar Campos",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(230, 267),
                Size = new Size(180, 36),
                Cursor = Cursors.Hand
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += (s, e) => LimpiarCampos();

            btnEliminar = new Button
            {
                Text = "🗑️ Eliminar Usuario",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(239, 68, 68),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(605, 267),
                Size = new Size(180, 36),
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;

            // TABLA DE USUARIOS
            dgvUsuarios = new DataGridView
            {
                Location = new Point(20, 315),
                Size = new Size(765, 270),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9f)
            };

            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersHeight = 32;

            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvUsuarios.RowTemplate.Height = 28;

            dgvUsuarios.CellClick += DgvUsuarios_CellClick;

            this.Controls.Add(panelHeader);
            this.Controls.Add(gbxFormulario);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnLimpiar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(dgvUsuarios);
        }

        private void CargarRoles()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdRol", typeof(int));
            dt.Columns.Add("NombreRol", typeof(string));

            dt.Rows.Add(1, "1 - Administrador");
            dt.Rows.Add(2, "2 - Docente");
            dt.Rows.Add(3, "3 - Estudiante");

            cmbRol.DataSource = dt;
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "IdRol";
            cmbRol.SelectedIndex = 0;
        }

        private void CargarUsuarios()
        {
            try
            {
                DataTable dt = usuarioDatos.ObtenerUsuarios();
                dgvUsuarios.DataSource = dt;
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar lista de usuarios: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombreCompleto.Clear();
            txtUsuario.Clear();
            txtClave.Clear();
            if (cmbRol.Items.Count > 0) cmbRol.SelectedIndex = 0;
            idUsuarioSeleccionado = 0;
            btnGuardar.Text = "💾 Guardar Usuario";
            btnGuardar.BackColor = Color.FromArgb(34, 197, 94);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreCompleto.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();
            int idRol = cmbRol.SelectedValue != null ? Convert.ToInt32(cmbRol.SelectedValue) : 3;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Por favor completa todos los campos del usuario.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exito = (idUsuarioSeleccionado == 0)
                ? usuarioDatos.InsertarUsuario(usuario, clave, nombre, idRol)
                : usuarioDatos.ActualizarUsuario(idUsuarioSeleccionado, usuario, clave, nombre, idRol);

            if (exito)
            {
                MessageBox.Show("Operación realizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un usuario de la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Deseas eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (usuarioDatos.EliminarUsuario(idUsuarioSeleccionado))
                {
                    MessageBox.Show("Usuario eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
            }
        }
        private void DgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

                if (fila.Cells["IdUsuario"].Value != null && fila.Cells["IdUsuario"].Value != DBNull.Value)
                {
                    idUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);

                    txtNombreCompleto.Text = fila.Cells["NombreCompleto"].Value != DBNull.Value ? fila.Cells["NombreCompleto"].Value.ToString() : "";
                    txtUsuario.Text = fila.Cells["NombreUsuario"].Value != DBNull.Value ? fila.Cells["NombreUsuario"].Value.ToString() : "";
                    txtClave.Text = fila.Cells["Clave"].Value != DBNull.Value ? fila.Cells["Clave"].Value.ToString() : "";

                    if (fila.Cells["IdRol"].Value != DBNull.Value)
                    {
                        cmbRol.SelectedValue = Convert.ToInt32(fila.Cells["IdRol"].Value);
                    }

                    btnGuardar.Text = "Actualizar Usuario";
                    btnGuardar.BackColor = Color.FromArgb(234, 179, 8);
                }
            }
        }

        private void OcultarColumnaClave()
        {
            if (dgvUsuarios.Columns.Contains("Clave"))
            {
                // Oculta la columna de contraseña por seguridad
                dgvUsuarios.Columns["Clave"].Visible = false;
            }
        }
    }
}