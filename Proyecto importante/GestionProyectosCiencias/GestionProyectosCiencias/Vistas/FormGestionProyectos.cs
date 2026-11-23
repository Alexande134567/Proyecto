using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias
{
    public partial class FormGestionProyectos : Form
    {
        private DataGridView dgvProyectos;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnNuevoProyecto;
        private Button btnEditarProyecto;
        private Button btnEliminarProyecto;
        private Button btnSubirInforme;
        private Button btnVerInforme;

        public FormGestionProyectos()
        {
            InitializeComponent();
            LimpiarYAcomodarInterfaz();
            CargarProyectosEnTabla();
        }

        private void LimpiarYAcomodarInterfaz()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Text.Contains("Módulo:"))
                {
                    c.Text = $"Módulo: Proyectos y Entregas  |  Usuario: ({SesionUsuario.NombreUsuario ?? "Docente/Admin"})";
                    c.Location = new Point(30, 15);
                    c.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    c.ForeColor = Color.FromArgb(0, 51, 153);
                    c.AutoSize = true;
                }
            }

            // Fila 1 de controles: Buscador y CRUD original intacto
            txtBuscar = new TextBox
            {
                Location = new Point(30, 52),
                Width = 200,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(txtBuscar);

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(238, 50),
                Width = 75,
                Height = 32,
                BackColor = Color.FromArgb(0, 51, 153),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += btnBuscar_Click;
            this.Controls.Add(btnBuscar);

            btnNuevoProyecto = new Button
            {
                Text = "+ Nuevo",
                Location = new Point(320, 50),
                Width = 115,
                Height = 32,
                BackColor = Color.FromArgb(3, 169, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNuevoProyecto.FlatAppearance.BorderSize = 0;
            btnNuevoProyecto.Click += btnNuevoProyecto_Click;
            this.Controls.Add(btnNuevoProyecto);

            btnEditarProyecto = new Button
            {
                Text = "✏️ Editar",
                Location = new Point(442, 50),
                Width = 115,
                Height = 32,
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEditarProyecto.FlatAppearance.BorderSize = 0;
            btnEditarProyecto.Click += btnEditarProyecto_Click;
            this.Controls.Add(btnEditarProyecto);

            btnEliminarProyecto = new Button
            {
                Text = "– Eliminar",
                Location = new Point(564, 50),
                Width = 115,
                Height = 32,
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEliminarProyecto.FlatAppearance.BorderSize = 0;
            btnEliminarProyecto.Click += btnEliminarProyecto_Click;
            this.Controls.Add(btnEliminarProyecto);

            // Fila 2 de controles: Subida de Avances (Estudiantes) y Revisión (Docentes/Admin)
            btnSubirInforme = new Button
            {
                Text = "📤 Subir Avance / Informe",
                Location = new Point(30, 90),
                Width = 195,
                Height = 32,
                BackColor = Color.FromArgb(0, 121, 107),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSubirInforme.FlatAppearance.BorderSize = 0;
            btnSubirInforme.Click += btnSubirInforme_Click;
            this.Controls.Add(btnSubirInforme);

            btnVerInforme = new Button
            {
                Text = "👁️ Ver Informe Subido",
                Location = new Point(235, 90),
                Width = 185,
                Height = 32,
                BackColor = Color.FromArgb(69, 90, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVerInforme.FlatAppearance.BorderSize = 0;
            btnVerInforme.Click += btnVerInforme_Click;
            this.Controls.Add(btnVerInforme);

            btnBuscar.BringToFront();
            btnNuevoProyecto.BringToFront();
            btnEditarProyecto.BringToFront();
            btnEliminarProyecto.BringToFront();
            btnSubirInforme.BringToFront();
            btnVerInforme.BringToFront();

            // Configurar DataGridView desplazado ligeramente abajo para no tapar los botones nuevos
            Control[] tablas = this.Controls.Find("dgvProyectos", true);
            if (tablas.Length > 0 && tablas[0] is DataGridView gridDesigner)
            {
                dgvProyectos = gridDesigner;
            }
            else
            {
                dgvProyectos = new DataGridView();
                this.Controls.Add(dgvProyectos);
            }

            dgvProyectos.Location = new Point(30, 132);
            dgvProyectos.Width = 720;
            dgvProyectos.Height = 335;
            dgvProyectos.ReadOnly = true;
            dgvProyectos.AllowUserToAddRows = false;
            dgvProyectos.AllowUserToDeleteRows = false;
            dgvProyectos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProyectos.MultiSelect = false;
            dgvProyectos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProyectos.BackgroundColor = Color.White;
            dgvProyectos.BorderStyle = BorderStyle.FixedSingle;
            dgvProyectos.RowHeadersVisible = false;

            dgvProyectos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvProyectos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProyectos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvProyectos.EnableHeadersVisualStyles = false;
            dgvProyectos.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvProyectos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(3, 169, 244);
            dgvProyectos.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void CargarProyectosEnTabla(string filtro = "")
        {
            if (dgvProyectos == null) return;

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            p.IdProyecto AS [ID],
                            p.Titulo AS [Título del Proyecto],
                            ISNULL(uE.NombreUsuario, ISNULL(uE.NombreCompleto, 'Sin Asignar')) AS [Estudiante / Grupo],
                            ISNULL(p.Estado, 'En Desarrollo') AS [Estado]
                        FROM Proyectos p
                        LEFT JOIN ProyectoIntegrantes pi ON p.IdProyecto = pi.IdProyecto
                        LEFT JOIN Usuarios uE ON pi.IdUsuario = uE.IdUsuario";

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE p.Titulo LIKE @Filtro OR uE.NombreUsuario LIKE @Filtro OR uE.NombreCompleto LIKE @Filtro";
                    }

                    SqlCommand cmd = new SqlCommand(query, con);
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Agregar columna visual indicando si ya tiene archivo subido
                    dt.Columns.Add("Archivo / Informe", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        int idP = Convert.ToInt32(row["ID"]);
                        string ultimoArchivo = ObtenerUltimoArchivoProyecto(idP);
                        row["Archivo / Informe"] = string.IsNullOrEmpty(ultimoArchivo)
                            ? "Sin archivo"
                            : "📄 " + Path.GetFileName(ultimoArchivo);
                    }

                    dgvProyectos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- NUEVA FUNCIÓN: SUBIR INFORME O AVANCE (ESTUDIANTES) ---
        private void btnSubirInforme_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow == null || dgvProyectos.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona el proyecto al que deseas subir el informe o avance.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProyecto = Convert.ToInt32(dgvProyectos.CurrentRow.Cells[0].Value);
            string tituloProyecto = dgvProyectos.CurrentRow.Cells[1].Value?.ToString() ?? "";

            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Title = $"Subir Avance/Informe - {tituloProyecto}";
                dialogo.Filter = "Documentos de Proyecto|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx;*.zip;*.rar|Todos los archivos|*.*";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string carpetaDestino = Path.Combine(Application.StartupPath, "EntregasArchivos", $"Proyecto_{idProyecto}");
                        Directory.CreateDirectory(carpetaDestino);

                        string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmm}_{Path.GetFileName(dialogo.FileName)}";
                        string rutaFinal = Path.Combine(carpetaDestino, nombreArchivo);

                        File.Copy(dialogo.FileName, rutaFinal, true);

                        // Actualizar estado en SQL Server a "En Revisión" y registrar el hito en Entregas
                        RegistrarEntregaArchivoEnBD(idProyecto, nombreArchivo);

                        MessageBox.Show(
                            $"¡Informe subido exitosamente!\n\nArchivo: {Path.GetFileName(dialogo.FileName)}\nEl estado del proyecto cambió a 'En Revisión' para que el docente pueda evaluarlo.",
                            "Entrega Registrada - UEMAM",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        CargarProyectosEnTabla();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar el informe: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // --- NUEVA FUNCIÓN: VER INFORME SUBIDO (DOCENTES / ADMIN) ---
        private void btnVerInforme_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow == null || dgvProyectos.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona un proyecto de la lista para revisar su informe.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProyecto = Convert.ToInt32(dgvProyectos.CurrentRow.Cells[0].Value);
            string tituloProyecto = dgvProyectos.CurrentRow.Cells[1].Value?.ToString() ?? "";
            string rutaArchivo = ObtenerUltimoArchivoProyecto(idProyecto);

            if (string.IsNullOrEmpty(rutaArchivo) || !File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    $"El proyecto \"{tituloProyecto}\" aún no tiene ningún informe o avance subido.",
                    "Sin Archivos Adjuntos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Abrir el archivo con el programa predeterminado de Windows (Word, PDF, etc.)
                Process.Start(new ProcessStartInfo
                {
                    FileName = rutaArchivo,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el archivo: " + ex.Message, "Error al abrir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerUltimoArchivoProyecto(int idProyecto)
        {
            string carpeta = Path.Combine(Application.StartupPath, "EntregasArchivos", $"Proyecto_{idProyecto}");
            if (!Directory.Exists(carpeta)) return "";

            var ultimo = new DirectoryInfo(carpeta)
                .GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            return ultimo?.FullName ?? "";
        }

        private void RegistrarEntregaArchivoEnBD(int idProyecto, string nombreArchivo)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryUpd = "UPDATE Proyectos SET Estado = 'En Revisión' WHERE IdProyecto = @Id";
                    SqlCommand cmdUpd = new SqlCommand(queryUpd, con);
                    cmdUpd.Parameters.AddWithValue("@Id", idProyecto);
                    cmdUpd.ExecuteNonQuery();

                    string queryIns = "INSERT INTO Entregas (IdProyecto, Fase, FechaEntrega, Estado) VALUES (@IdP, @Fase, GETDATE(), 'Entregado')";
                    SqlCommand cmdIns = new SqlCommand(queryIns, con);
                    cmdIns.Parameters.AddWithValue("@IdP", idProyecto);
                    cmdIns.Parameters.AddWithValue("@Fase", "Informe: " + nombreArchivo);
                    cmdIns.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al registrar entrega en BD: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProyectosEnTabla(txtBuscar.Text.Trim());
        }

        private void btnNuevoProyecto_Click(object sender, EventArgs e)
        {
            Form formNuevo = new Form
            {
                Width = 460,
                Height = 240,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Registrar Nuevo Proyecto - UEMAM",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblTitulo = new Label { Left = 20, Top = 15, Text = "Título del Proyecto:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtTitulo = new TextBox { Left = 20, Top = 38, Width = 400, Font = new Font("Segoe UI", 9.5F) };

            Label lblEstudiante = new Label { Left = 20, Top = 75, Text = "Estudiante / Grupo (Opcional):", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtEstudiante = new TextBox { Left = 20, Top = 98, Width = 400, Font = new Font("Segoe UI", 9.5F) };

            Button btnGuardar = new Button { Text = "Guardar", Left = 220, Width = 95, Top = 145, Height = 32, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(0, 51, 153), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            Button btnCancelar = new Button { Text = "Cancelar", Left = 325, Width = 95, Top = 145, Height = 32, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            btnGuardar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.BorderSize = 0;

            formNuevo.Controls.Add(lblTitulo);
            formNuevo.Controls.Add(txtTitulo);
            formNuevo.Controls.Add(lblEstudiante);
            formNuevo.Controls.Add(txtEstudiante);
            formNuevo.Controls.Add(btnGuardar);
            formNuevo.Controls.Add(btnCancelar);

            formNuevo.AcceptButton = btnGuardar;
            formNuevo.CancelButton = btnCancelar;

            if (formNuevo.ShowDialog() == DialogResult.OK)
            {
                string tituloProyecto = txtTitulo.Text.Trim();
                string nombreEstudiante = txtEstudiante.Text.Trim();

                if (string.IsNullOrWhiteSpace(tituloProyecto))
                {
                    MessageBox.Show("El título del proyecto es obligatorio.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GuardarNuevoProyectoBD(tituloProyecto, nombreEstudiante);
            }
        }

        private void btnEditarProyecto_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow == null || dgvProyectos.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona un proyecto de la lista para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProyecto = Convert.ToInt32(dgvProyectos.CurrentRow.Cells[0].Value);
            string tituloActual = dgvProyectos.CurrentRow.Cells[1].Value?.ToString() ?? "";
            string estudianteActual = dgvProyectos.CurrentRow.Cells[2].Value?.ToString() ?? "";
            string estadoActual = dgvProyectos.CurrentRow.Cells[3].Value?.ToString() ?? "En Desarrollo";

            Form formEditar = new Form
            {
                Width = 460,
                Height = 310,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Editar Proyecto - UEMAM",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblTitulo = new Label { Left = 20, Top = 15, Text = "Título del Proyecto:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtTitulo = new TextBox { Left = 20, Top = 38, Width = 400, Text = tituloActual, Font = new Font("Segoe UI", 9.5F) };

            Label lblEstudiante = new Label { Left = 20, Top = 75, Text = "Estudiante / Grupo:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtEstudiante = new TextBox { Left = 20, Top = 98, Width = 400, Text = (estudianteActual == "Sin Asignar" ? "" : estudianteActual), Font = new Font("Segoe UI", 9.5F) };

            Label lblEstado = new Label { Left = 20, Top = 135, Text = "Estado del Proyecto:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbEstado = new ComboBox { Left = 20, Top = 158, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cmbEstado.Items.AddRange(new object[] { "En Desarrollo", "En Revisión", "Aprobado", "Finalizado" });

            if (cmbEstado.Items.Contains(estadoActual))
                cmbEstado.SelectedItem = estadoActual;
            else
                cmbEstado.SelectedIndex = 0;

            Button btnGuardar = new Button { Text = "Guardar", Left = 220, Width = 95, Top = 210, Height = 32, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(255, 152, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            Button btnCancelar = new Button { Text = "Cancelar", Left = 325, Width = 95, Top = 210, Height = 32, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            btnGuardar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.BorderSize = 0;

            formEditar.Controls.Add(lblTitulo);
            formEditar.Controls.Add(txtTitulo);
            formEditar.Controls.Add(lblEstudiante);
            formEditar.Controls.Add(txtEstudiante);
            formEditar.Controls.Add(lblEstado);
            formEditar.Controls.Add(cmbEstado);
            formEditar.Controls.Add(btnGuardar);
            formEditar.Controls.Add(btnCancelar);

            formEditar.AcceptButton = btnGuardar;
            formEditar.CancelButton = btnCancelar;

            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                string nuevoTitulo = txtTitulo.Text.Trim();
                string nuevoEstudiante = txtEstudiante.Text.Trim();
                string nuevoEstado = cmbEstado.SelectedItem.ToString();

                if (string.IsNullOrWhiteSpace(nuevoTitulo))
                {
                    MessageBox.Show("El título del proyecto no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ActualizarProyectoBD(idProyecto, nuevoTitulo, nuevoEstudiante, nuevoEstado);
            }
        }

        private void GuardarNuevoProyectoBD(string titulo, string estudiante)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryProyecto = "INSERT INTO Proyectos (Titulo, Estado) VALUES (@Titulo, 'En Desarrollo'); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdP = new SqlCommand(queryProyecto, con);
                    cmdP.Parameters.AddWithValue("@Titulo", titulo);
                    int idProyecto = Convert.ToInt32(cmdP.ExecuteScalar());

                    if (!string.IsNullOrEmpty(estudiante))
                    {
                        string queryBuscarUsuario = "SELECT IdUsuario FROM Usuarios WHERE NombreUsuario LIKE @Nom OR NombreCompleto LIKE @Nom";
                        SqlCommand cmdB = new SqlCommand(queryBuscarUsuario, con);
                        cmdB.Parameters.AddWithValue("@Nom", "%" + estudiante + "%");
                        object resUser = cmdB.ExecuteScalar();

                        int idUsuario = 0;
                        if (resUser != null)
                        {
                            idUsuario = Convert.ToInt32(resUser);
                        }
                        else
                        {
                            string queryNewUser = "INSERT INTO Usuarios (NombreUsuario, NombreCompleto, Clave, IdRol) VALUES (@Nom, @Nom, '1234', 3); SELECT SCOPE_IDENTITY();";
                            SqlCommand cmdU = new SqlCommand(queryNewUser, con);
                            cmdU.Parameters.AddWithValue("@Nom", estudiante);
                            idUsuario = Convert.ToInt32(cmdU.ExecuteScalar());
                        }

                        string queryRel = "INSERT INTO ProyectoIntegrantes (IdProyecto, IdUsuario) VALUES (@IdP, @IdU)";
                        SqlCommand cmdR = new SqlCommand(queryRel, con);
                        cmdR.Parameters.AddWithValue("@IdP", idProyecto);
                        cmdR.Parameters.AddWithValue("@IdU", idUsuario);
                        cmdR.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Proyecto registrado exitosamente!", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProyectosEnTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el nuevo proyecto: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarProyectoBD(int idProyecto, string titulo, string estudiante, string estado)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryP = "UPDATE Proyectos SET Titulo = @Titulo, Estado = @Estado WHERE IdProyecto = @Id";
                    SqlCommand cmdP = new SqlCommand(queryP, con);
                    cmdP.Parameters.AddWithValue("@Titulo", titulo);
                    cmdP.Parameters.AddWithValue("@Estado", estado);
                    cmdP.Parameters.AddWithValue("@Id", idProyecto);
                    cmdP.ExecuteNonQuery();

                    if (!string.IsNullOrEmpty(estudiante))
                    {
                        string queryBuscarUser = "SELECT IdUsuario FROM Usuarios WHERE NombreUsuario LIKE @Nom OR NombreCompleto LIKE @Nom";
                        SqlCommand cmdB = new SqlCommand(queryBuscarUser, con);
                        cmdB.Parameters.AddWithValue("@Nom", "%" + estudiante + "%");
                        object resUser = cmdB.ExecuteScalar();

                        int idUsuario = 0;
                        if (resUser != null)
                        {
                            idUsuario = Convert.ToInt32(resUser);
                        }
                        else
                        {
                            string queryNewUser = "INSERT INTO Usuarios (NombreUsuario, NombreCompleto, Clave, IdRol) VALUES (@Nom, @Nom, '1234', 3); SELECT SCOPE_IDENTITY();";
                            SqlCommand cmdU = new SqlCommand(queryNewUser, con);
                            cmdU.Parameters.AddWithValue("@Nom", estudiante);
                            idUsuario = Convert.ToInt32(cmdU.ExecuteScalar());
                        }

                        string queryDelRel = "DELETE FROM ProyectoIntegrantes WHERE IdProyecto = @IdP";
                        SqlCommand cmdDelR = new SqlCommand(queryDelRel, con);
                        cmdDelR.Parameters.AddWithValue("@IdP", idProyecto);
                        cmdDelR.ExecuteNonQuery();

                        string queryRel = "INSERT INTO ProyectoIntegrantes (IdProyecto, IdUsuario) VALUES (@IdP, @IdU)";
                        SqlCommand cmdR = new SqlCommand(queryRel, con);
                        cmdR.Parameters.AddWithValue("@IdP", idProyecto);
                        cmdR.Parameters.AddWithValue("@IdU", idUsuario);
                        cmdR.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Proyecto actualizado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProyectosEnTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el proyecto: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarProyecto_Click(object sender, EventArgs e)
        {
            if (dgvProyectos.CurrentRow == null || dgvProyectos.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor, selecciona un proyecto de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProyecto = Convert.ToInt32(dgvProyectos.CurrentRow.Cells[0].Value);
            string titulo = dgvProyectos.CurrentRow.Cells[1].Value?.ToString() ?? "";

            DialogResult confirmacion = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar permanentemente el proyecto:\n\n\"{titulo}\"?\n\nEsta acción eliminará sus entregas y observaciones registradas.",
                "Confirmar Eliminación - UEMAM",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                bool borrado = EliminarProyectoBD(idProyecto);

                if (borrado)
                {
                    MessageBox.Show("El proyecto fue eliminado exitosamente de la base de datos.", "Operación Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProyectosEnTabla();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el proyecto. Revisa la conexión a SQL Server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool EliminarProyectoBD(int idProyecto)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryBorrarDependencias = @"
                        DELETE FROM Observaciones WHERE IdEntrega IN (SELECT IdEntrega FROM Entregas WHERE IdProyecto = @Id);
                        DELETE FROM Entregas WHERE IdProyecto = @Id;
                        DELETE FROM ProyectoIntegrantes WHERE IdProyecto = @Id;";

                    SqlCommand cmdDep = new SqlCommand(queryBorrarDependencias, con);
                    cmdDep.Parameters.AddWithValue("@Id", idProyecto);
                    cmdDep.ExecuteNonQuery();

                    string queryProyecto = "DELETE FROM Proyectos WHERE IdProyecto = @Id";
                    SqlCommand cmdP = new SqlCommand(queryProyecto, con);
                    cmdP.Parameters.AddWithValue("@Id", idProyecto);
                    cmdP.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al borrar en BD: " + ex.Message);
                return false;
            }
        }
    }
}