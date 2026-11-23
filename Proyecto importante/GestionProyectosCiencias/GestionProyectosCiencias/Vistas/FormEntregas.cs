using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias
{
    public partial class FormEntregas : Form
    {
        private DataGridView dgvEntregas;
        private Button btnRegistrarEntrega;
        private Button btnEditarEntrega;
        private Button btnEliminarEntrega;

        public FormEntregas()
        {
            InitializeComponent();
            InicializarInterfazGrafica();
            CargarEntregasEnTabla();
        }

        private void InicializarInterfazGrafica()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // 1. Título del Módulo
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Text.Contains("Módulo:"))
                {
                    c.Text = $"Módulo: Entregas  |  Usuario: ({SesionUsuario.NombreUsuario ?? "Docente/Admin"})";
                    c.Location = new Point(30, 15);
                    c.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    c.ForeColor = Color.FromArgb(0, 51, 153);
                    c.AutoSize = true;
                }
            }

            // 2. Botón + Registrar Entrega
            btnRegistrarEntrega = new Button
            {
                Text = "+ Registrar Entrega",
                Location = new Point(30, 50),
                Width = 150,
                Height = 32,
                BackColor = Color.FromArgb(0, 51, 153),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegistrarEntrega.FlatAppearance.BorderSize = 0;
            btnRegistrarEntrega.Click += btnRegistrarEntrega_Click;
            this.Controls.Add(btnRegistrarEntrega);

            // 3. Botón ✏️ Editar Entrega
            btnEditarEntrega = new Button
            {
                Text = "✏️ Editar Entrega",
                Location = new Point(190, 50),
                Width = 135,
                Height = 32,
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEditarEntrega.FlatAppearance.BorderSize = 0;
            btnEditarEntrega.Click += btnEditarEntrega_Click;
            this.Controls.Add(btnEditarEntrega);

            // 4. Botón – Eliminar Entrega
            btnEliminarEntrega = new Button
            {
                Text = "– Eliminar Entrega",
                Location = new Point(335, 50),
                Width = 135,
                Height = 32,
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEliminarEntrega.FlatAppearance.BorderSize = 0;
            btnEliminarEntrega.Click += btnEliminarEntrega_Click;
            this.Controls.Add(btnEliminarEntrega);

            btnRegistrarEntrega.BringToFront();
            btnEditarEntrega.BringToFront();
            btnEliminarEntrega.BringToFront();

            // 5. Configurar DataGridView
            Control[] tablas = this.Controls.Find("dgvEntregas", true);
            if (tablas.Length > 0 && tablas[0] is DataGridView gridDesigner)
            {
                dgvEntregas = gridDesigner;
            }
            else
            {
                dgvEntregas = new DataGridView();
                this.Controls.Add(dgvEntregas);
            }

            dgvEntregas.Location = new Point(30, 95);
            dgvEntregas.Width = 720;
            dgvEntregas.Height = 360;
            dgvEntregas.ReadOnly = true;
            dgvEntregas.AllowUserToAddRows = false;
            dgvEntregas.AllowUserToDeleteRows = false;
            dgvEntregas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEntregas.MultiSelect = false;
            dgvEntregas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEntregas.BackgroundColor = Color.White;
            dgvEntregas.BorderStyle = BorderStyle.FixedSingle;
            dgvEntregas.RowHeadersVisible = false;

            // Estilos Visuales
            dgvEntregas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvEntregas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEntregas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvEntregas.EnableHeadersVisualStyles = false;
            dgvEntregas.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvEntregas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(3, 169, 244);
            dgvEntregas.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void CargarEntregasEnTabla()
        {
            if (dgvEntregas == null) return;

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    // Consulta dinámica adaptable a los nombres de columna comunes en SQL Server
                    string query = @"
                        SELECT 
                            e.IdEntrega AS [ID],
                            'ENT-' + RIGHT('000' + CAST(e.IdEntrega AS VARCHAR(10)), 3) AS [Código],
                            p.Titulo AS [Proyecto],
                            e.Fase AS [Fase de Entrega],
                            CONVERT(VARCHAR(10), e.FechaEntrega, 120) AS [Fecha Límite],
                            ISNULL(e.Estado, 'Pendiente') AS [Estatus]
                        FROM Entregas e
                        INNER JOIN Proyectos p ON e.IdProyecto = p.IdProyecto";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvEntregas.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de entregas: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarEntrega_Click(object sender, EventArgs e)
        {
            Form formNuevo = new Form
            {
                Width = 460,
                Height = 320,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Registrar Nueva Entrega - UEMAM",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblProyecto = new Label { Left = 20, Top = 15, Text = "Seleccionar Proyecto:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbProyectos = new ComboBox { Left = 20, Top = 38, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            Label lblFase = new Label { Left = 20, Top = 75, Text = "Nombre de la Fase / Hito:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtFase = new TextBox { Left = 20, Top = 98, Width = 400, Font = new Font("Segoe UI", 9.5F) };

            Label lblFecha = new Label { Left = 20, Top = 135, Text = "Fecha Límite de Entrega:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpFecha = new DateTimePicker { Left = 20, Top = 158, Width = 400, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9.5F) };

            Button btnGuardar = new Button { Text = "Guardar", Left = 220, Width = 95, Top = 220, Height = 32, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(0, 51, 153), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            Button btnCancelar = new Button { Text = "Cancelar", Left = 325, Width = 95, Top = 220, Height = 32, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            btnGuardar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.BorderSize = 0;

            DataTable dtProyectos = ObtenerProyectos();
            cmbProyectos.DataSource = dtProyectos;
            cmbProyectos.DisplayMember = "Titulo";
            cmbProyectos.ValueMember = "IdProyecto";

            formNuevo.Controls.Add(lblProyecto);
            formNuevo.Controls.Add(cmbProyectos);
            formNuevo.Controls.Add(lblFase);
            formNuevo.Controls.Add(txtFase);
            formNuevo.Controls.Add(lblFecha);
            formNuevo.Controls.Add(dtpFecha);
            formNuevo.Controls.Add(btnGuardar);
            formNuevo.Controls.Add(btnCancelar);

            formNuevo.AcceptButton = btnGuardar;
            formNuevo.CancelButton = btnCancelar;

            if (formNuevo.ShowDialog() == DialogResult.OK)
            {
                if (cmbProyectos.SelectedValue == null)
                {
                    MessageBox.Show("Debes seleccionar un proyecto válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idProyecto = Convert.ToInt32(cmbProyectos.SelectedValue);
                string fase = txtFase.Text.Trim();
                DateTime fechaLimite = dtpFecha.Value;

                if (string.IsNullOrWhiteSpace(fase))
                {
                    MessageBox.Show("El nombre de la fase es obligatorio.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GuardarNuevaEntregaBD(idProyecto, fase, fechaLimite);
            }
        }

        private void btnEditarEntrega_Click(object sender, EventArgs e)
        {
            if (dgvEntregas.CurrentRow == null || dgvEntregas.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona una entrega de la lista para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEntrega = Convert.ToInt32(dgvEntregas.CurrentRow.Cells[0].Value);
            string faseActual = dgvEntregas.CurrentRow.Cells[3].Value?.ToString() ?? "";
            string estatusActual = dgvEntregas.CurrentRow.Cells[5].Value?.ToString() ?? "Pendiente";

            DateTime fechaActual = DateTime.Now;
            DateTime.TryParse(dgvEntregas.CurrentRow.Cells[4].Value?.ToString(), out fechaActual);

            Form formEditar = new Form
            {
                Width = 460,
                Height = 360,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Editar Entrega - UEMAM",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblFase = new Label { Left = 20, Top = 15, Text = "Fase / Hito de Entrega:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtFase = new TextBox { Left = 20, Top = 38, Width = 400, Text = faseActual, Font = new Font("Segoe UI", 9.5F) };

            Label lblFecha = new Label { Left = 20, Top = 75, Text = "Fecha Límite:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpFecha = new DateTimePicker { Left = 20, Top = 98, Width = 400, Value = fechaActual, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9.5F) };

            Label lblEstatus = new Label { Left = 20, Top = 135, Text = "Estatus de la Entrega:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbEstatus = new ComboBox { Left = 20, Top = 158, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cmbEstatus.Items.AddRange(new object[] { "Pendiente", "Entregado", "Atrasado", "Aprobado" });

            if (cmbEstatus.Items.Contains(estatusActual))
                cmbEstatus.SelectedItem = estatusActual;
            else
                cmbEstatus.SelectedIndex = 0;

            Button btnGuardar = new Button { Text = "Guardar", Left = 220, Width = 95, Top = 230, Height = 32, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(255, 152, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            Button btnCancelar = new Button { Text = "Cancelar", Left = 325, Width = 95, Top = 230, Height = 32, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            btnGuardar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.BorderSize = 0;

            formEditar.Controls.Add(lblFase);
            formEditar.Controls.Add(txtFase);
            formEditar.Controls.Add(lblFecha);
            formEditar.Controls.Add(dtpFecha);
            formEditar.Controls.Add(lblEstatus);
            formEditar.Controls.Add(cmbEstatus);
            formEditar.Controls.Add(btnGuardar);
            formEditar.Controls.Add(btnCancelar);

            formEditar.AcceptButton = btnGuardar;
            formEditar.CancelButton = btnCancelar;

            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                string nuevaFase = txtFase.Text.Trim();
                DateTime nuevaFecha = dtpFecha.Value;
                string nuevoEstatus = cmbEstatus.SelectedItem.ToString();

                if (string.IsNullOrWhiteSpace(nuevaFase))
                {
                    MessageBox.Show("El nombre de la fase no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ActualizarEntregaBD(idEntrega, nuevaFase, nuevaFecha, nuevoEstatus);
            }
        }

        private void btnEliminarEntrega_Click(object sender, EventArgs e)
        {
            if (dgvEntregas.CurrentRow == null || dgvEntregas.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona una entrega de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEntrega = Convert.ToInt32(dgvEntregas.CurrentRow.Cells[0].Value);
            string codigo = dgvEntregas.CurrentRow.Cells[1].Value?.ToString() ?? "";
            string fase = dgvEntregas.CurrentRow.Cells[3].Value?.ToString() ?? "";

            DialogResult confirmacion = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar permanentemente la entrega:\n\n[{codigo}] {fase}?\n\nEsta acción eliminará sus observaciones asociadas.",
                "Confirmar Eliminación - UEMAM",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                EliminarEntregaBD(idEntrega);
            }
        }

        private DataTable ObtenerProyectos()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();
                    string query = "SELECT IdProyecto, Titulo FROM Proyectos ORDER BY Titulo ASC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar proyectos: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        private void GuardarNuevaEntregaBD(int idProyecto, string fase, DateTime fechaLimite)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();
                    string query = "INSERT INTO Entregas (IdProyecto, Fase, FechaEntrega, Estado) VALUES (@IdP, @Fase, @Fecha, 'Pendiente')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IdP", idProyecto);
                    cmd.Parameters.AddWithValue("@Fase", fase);
                    cmd.Parameters.AddWithValue("@Fecha", fechaLimite);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("¡Entrega registrada exitosamente!", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEntregasEnTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la entrega: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarEntregaBD(int idEntrega, string fase, DateTime fechaLimite, string estatus)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();
                    string query = "UPDATE Entregas SET Fase = @Fase, FechaEntrega = @Fecha, Estado = @Estatus WHERE IdEntrega = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Fase", fase);
                    cmd.Parameters.AddWithValue("@Fecha", fechaLimite);
                    cmd.Parameters.AddWithValue("@Estatus", estatus);
                    cmd.Parameters.AddWithValue("@Id", idEntrega);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("¡Entrega actualizada correctamente!", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEntregasEnTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la entrega: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarEntregaBD(int idEntrega)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryObs = "DELETE FROM Observaciones WHERE IdEntrega = @Id";
                    SqlCommand cmdObs = new SqlCommand(queryObs, con);
                    cmdObs.Parameters.AddWithValue("@Id", idEntrega);
                    cmdObs.ExecuteNonQuery();

                    string queryEnt = "DELETE FROM Entregas WHERE IdEntrega = @Id";
                    SqlCommand cmdEnt = new SqlCommand(queryEnt, con);
                    cmdEnt.Parameters.AddWithValue("@Id", idEntrega);
                    cmdEnt.ExecuteNonQuery();

                    MessageBox.Show("La entrega y sus registros vinculados fueron eliminados.", "Operación Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEntregasEnTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la entrega: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}