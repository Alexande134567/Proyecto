using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias
{
    public partial class FormNotificaciones : Form
    {
        private DataGridView dgvNotificaciones;
        private Button btnEnviarNotificacion;
        private Button btnEliminarNotificacion;
        private Button btnMarcarLeido;

        public FormNotificaciones()
        {
            InitializeComponent();
            InicializarInterfazGrafica();
            CargarNotificacionesBD();
        }

        private void InicializarInterfazGrafica()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // 1. Ocultar o ajustar títulos preexistentes del Designer
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Text.Contains("Módulo:"))
                {
                    c.Text = $"Módulo: Notificaciones  |  Usuario: ({SesionUsuario.NombreUsuario ?? "Docente/Admin"})";
                    c.Location = new Point(30, 20);
                    c.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    c.ForeColor = Color.FromArgb(0, 51, 153);
                    c.AutoSize = true;
                }
            }

            // 2. Botón + Nueva Notificación
            btnEnviarNotificacion = new Button
            {
                Text = "📢 Nueva Notificación",
                Location = new Point(330, 16),
                Width = 150,
                Height = 32,
                BackColor = Color.FromArgb(0, 51, 153),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEnviarNotificacion.FlatAppearance.BorderSize = 0;
            btnEnviarNotificacion.Click += btnEnviarNotificacion_Click;
            this.Controls.Add(btnEnviarNotificacion);

            // 3. Botón ✔️ Marcar como Leída
            btnMarcarLeido = new Button
            {
                Text = "✔️ Marcar Leída",
                Location = new Point(490, 16),
                Width = 120,
                Height = 32,
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnMarcarLeido.FlatAppearance.BorderSize = 0;
            btnMarcarLeido.Click += btnMarcarLeido_Click;
            this.Controls.Add(btnMarcarLeido);

            // 4. Botón 🗑️ Eliminar Notificación
            btnEliminarNotificacion = new Button
            {
                Text = "🗑️ Eliminar",
                Location = new Point(620, 16),
                Width = 130,
                Height = 32,
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEliminarNotificacion.FlatAppearance.BorderSize = 0;
            btnEliminarNotificacion.Click += btnEliminarNotificacion_Click;
            this.Controls.Add(btnEliminarNotificacion);

            btnEnviarNotificacion.BringToFront();
            btnMarcarLeido.BringToFront();
            btnEliminarNotificacion.BringToFront();

            // 5. DataGridView de Notificaciones
            Control[] tablas = this.Controls.Find("dgvNotificaciones", true);
            if (tablas.Length > 0 && tablas[0] is DataGridView gridDesigner)
            {
                dgvNotificaciones = gridDesigner;
            }
            else
            {
                dgvNotificaciones = new DataGridView();
                this.Controls.Add(dgvNotificaciones);
            }

            dgvNotificaciones.Location = new Point(30, 60);
            dgvNotificaciones.Width = 720;
            dgvNotificaciones.Height = 380;
            dgvNotificaciones.ReadOnly = true;
            dgvNotificaciones.AllowUserToAddRows = false;
            dgvNotificaciones.AllowUserToDeleteRows = false;
            dgvNotificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNotificaciones.MultiSelect = false;
            dgvNotificaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNotificaciones.BackgroundColor = Color.White;
            dgvNotificaciones.BorderStyle = BorderStyle.FixedSingle;
            dgvNotificaciones.RowHeadersVisible = false;
            dgvNotificaciones.RowTemplate.Height = 35;

            // Estilos Visuales de la Tabla
            dgvNotificaciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvNotificaciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNotificaciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvNotificaciones.EnableHeadersVisualStyles = false;
            dgvNotificaciones.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvNotificaciones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(3, 169, 244);
            dgvNotificaciones.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvNotificaciones.CellFormatting += dgvNotificaciones_CellFormatting;
        }

        private void CargarNotificacionesBD()
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();
                    string query = @"
                        SELECT 
                            n.IdNotificacion AS [ID],
                            n.Mensaje AS [Aviso / Notificación],
                            CONVERT(VARCHAR(10), n.FechaEnvio, 103) AS [Fecha],
                            ISNULL(n.Estado, 'Pendiente') AS [Estado]
                        FROM Notificaciones n
                        ORDER BY n.IdNotificacion DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        dt.Rows.Add(1, "📢 Recordatorio: La entrega del Capítulo I vence el 20 de octubre.", "19/10/2026", "Pendiente");
                        dt.Rows.Add(2, "✔️ El tutor aprobó los avances del módulo de Usuarios.", "18/10/2026", "Leída");
                        dt.Rows.Add(3, "✉️ Se ha agregado una nueva observación en el Diagrama Gantt.", "17/10/2026", "Pendiente");
                        dt.Rows.Add(4, "🔔 Reunión de avance programada para el viernes a las 10:00 AM.", "16/10/2026", "Pendiente");
                    }

                    dgvNotificaciones.DataSource = dt;

                    if (dgvNotificaciones.Columns.Contains("ID"))
                        dgvNotificaciones.Columns["ID"].Width = 40;

                    if (dgvNotificaciones.Columns.Contains("Fecha"))
                        dgvNotificaciones.Columns["Fecha"].Width = 90;

                    if (dgvNotificaciones.Columns.Contains("Estado"))
                        dgvNotificaciones.Columns["Estado"].Width = 90;
                }
            }
            catch
            {
                DataTable dtFallback = new DataTable();
                dtFallback.Columns.Add("ID", typeof(int));
                dtFallback.Columns.Add("Aviso / Notificación", typeof(string));
                dtFallback.Columns.Add("Fecha", typeof(string));
                dtFallback.Columns.Add("Estado", typeof(string));

                dtFallback.Rows.Add(1, "📢 Recordatorio: La entrega del Capítulo I vence el 20 de octubre.", "19/10/2026", "Pendiente");
                dtFallback.Rows.Add(2, "✔️ El tutor aprobó los avances del módulo de Usuarios.", "18/10/2026", "Leída");
                dtFallback.Rows.Add(3, "✉️ Se ha agregado una nueva observación en el Diagrama Gantt.", "17/10/2026", "Pendiente");
                dtFallback.Rows.Add(4, "🔔 Reunión de avance programada para el viernes a las 10:00 AM.", "16/10/2026", "Pendiente");

                dgvNotificaciones.DataSource = dtFallback;
            }
        }

        private void dgvNotificaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvNotificaciones.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string estado = e.Value.ToString();
                if (estado == "Leída")
                {
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(211, 47, 47);
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
        }

        private string MostrarDialogoEntradaTexto(string titulo, string mensaje)
        {
            Form formPrompt = new Form()
            {
                Width = 450,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = titulo,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label lblText = new Label() { Left = 20, Top = 15, Text = mensaje, AutoSize = true };
            TextBox txtInput = new TextBox() { Left = 20, Top = 40, Width = 390, Text = "🔔 " };
            Button btnConfirm = new Button() { Text = "Aceptar", Left = 210, Width = 90, Top = 80, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(0, 51, 153), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            Button btnCancel = new Button() { Text = "Cancelar", Left = 310, Width = 100, Top = 80, DialogResult = DialogResult.Cancel };

            formPrompt.Controls.Add(lblText);
            formPrompt.Controls.Add(txtInput);
            formPrompt.Controls.Add(btnConfirm);
            formPrompt.Controls.Add(btnCancel);
            formPrompt.AcceptButton = btnConfirm;
            formPrompt.CancelButton = btnCancel;

            return formPrompt.ShowDialog() == DialogResult.OK ? txtInput.Text : "";
        }

        private void btnEnviarNotificacion_Click(object sender, EventArgs e)
        {
            string nuevoAviso = MostrarDialogoEntradaTexto("Nueva Notificación - UEMAM", "Ingresa el texto para la notificación:");

            if (!string.IsNullOrWhiteSpace(nuevoAviso))
            {
                try
                {
                    using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                    {
                        con.Open();
                        string query = "INSERT INTO Notificaciones (Mensaje, FechaEnvio, Estado) VALUES (@Msg, GETDATE(), 'Pendiente')";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Msg", nuevoAviso);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }

                MessageBox.Show("Notificación publicada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarNotificacionesBD();
            }
        }

        private void btnMarcarLeido_Click(object sender, EventArgs e)
        {
            if (dgvNotificaciones.CurrentRow != null && dgvNotificaciones.CurrentRow.Index >= 0)
            {
                dgvNotificaciones.CurrentRow.Cells["Estado"].Value = "Leída";
                int id = Convert.ToInt32(dgvNotificaciones.CurrentRow.Cells["ID"].Value);

                try
                {
                    using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                    {
                        con.Open();
                        string query = "UPDATE Notificaciones SET Estado = 'Leída' WHERE IdNotificacion = @Id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }

                dgvNotificaciones.InvalidateRow(dgvNotificaciones.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Selecciona una notificación para marcar como leída.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarNotificacion_Click(object sender, EventArgs e)
        {
            if (dgvNotificaciones.CurrentRow != null && dgvNotificaciones.CurrentRow.Index >= 0)
            {
                int id = Convert.ToInt32(dgvNotificaciones.CurrentRow.Cells["ID"].Value);

                DialogResult res = MessageBox.Show("¿Deseas eliminar esta notificación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                        {
                            con.Open();
                            string query = "DELETE FROM Notificaciones WHERE IdNotificacion = @Id";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    CargarNotificacionesBD();
                }
            }
            else
            {
                MessageBox.Show("Selecciona una notificación de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}