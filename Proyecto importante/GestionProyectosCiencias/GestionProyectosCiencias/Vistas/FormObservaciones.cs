using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using GestionProyectosCiencias.Datos;

namespace GestionProyectosCiencias
{
    public partial class FormObservaciones : Form
    {
        private ComboBox cmbProyectos;
        private TextBox txtObservacion;
        private TextBox txtCorreo;
        private CheckBox chkGmail;
        private Button btnAdjuntarArchivo;
        private Button btnQuitarAdjunto;
        private Label lblArchivoAdjunto;
        private Button btnHabilitarEdicion;
        private Button btnGuardarYNotificar;
        private string rutaArchivoSeleccionado = "";

        public FormObservaciones()
        {
            InitializeComponent();
            ConstruirYMostrarInterfazCompleta();
            CargarProyectosEnCombo();
        }

        private void ConstruirYMostrarInterfazCompleta()
        {
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Actualizar encabezado del módulo si existe
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Text.Contains("Módulo:"))
                {
                    c.Text = $"Módulo: Observaciones  |  Usuario: ({SesionUsuario.NombreUsuario ?? "Docente/Admin"})";
                    c.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    c.ForeColor = Color.FromArgb(0, 51, 153);
                    c.AutoSize = true;
                }
            }

            // Buscar controles existentes en el formulario o dentro de cualquier Panel
            cmbProyectos = BuscarControl<ComboBox>(this);
            chkGmail = BuscarControl<CheckBox>(this);

            var textBoxes = BuscarTodos<TextBox>(this).OrderBy(t => t.Top).ToList();
            if (textBoxes.Count >= 2)
            {
                txtObservacion = textBoxes.FirstOrDefault(t => t.Multiline || t.Height > 50) ?? textBoxes[0];
                txtCorreo = textBoxes.FirstOrDefault(t => t != txtObservacion) ?? textBoxes[1];
            }

            // Si los controles estaban dentro de un Panel del Designer, asegurar que el Panel sea visible y acomodado
            Panel contenedor = BuscarControl<Panel>(this);
            if (contenedor != null)
            {
                contenedor.Visible = true;
                contenedor.BringToFront();
            }

            // Si no se encontraron los controles en el Designer, crearlos completos para que nada falte
            Control destino = (Control)contenedor ?? this;

            if (cmbProyectos == null)
            {
                Label lblProj = new Label { Text = "Seleccionar Proyecto de Grado:", Location = new Point(25, 20), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 51, 153) };
                cmbProyectos = new ComboBox { Location = new Point(25, 45), Width = 620, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
                destino.Controls.Add(lblProj);
                destino.Controls.Add(cmbProyectos);
            }

            if (txtObservacion == null)
            {
                txtObservacion = new TextBox { Location = new Point(25, 85), Width = 620, Height = 140, Multiline = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Segoe UI", 9.5F) };
                destino.Controls.Add(txtObservacion);
            }

            if (txtCorreo == null)
            {
                Label lblMail = new Label { Text = "Correo del Estudiante/Grupo (Gmail):", Location = new Point(25, 240), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                txtCorreo = new TextBox { Location = new Point(25, 263), Width = 620, Font = new Font("Segoe UI", 9.5F), Text = "estudiantes.uemam2026@gmail.com" };
                destino.Controls.Add(lblMail);
                destino.Controls.Add(txtCorreo);
            }

            if (chkGmail == null)
            {
                Label lblEnv = new Label { Text = "Enviar notificaciones por:", Location = new Point(25, 303), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 51, 153) };
                chkGmail = new CheckBox { Text = "Gmail", Location = new Point(195, 301), AutoSize = true, Checked = true, Font = new Font("Segoe UI", 9F) };
                destino.Controls.Add(lblEnv);
                destino.Controls.Add(chkGmail);
            }

            // Buscar botones originales
            var botones = BuscarTodos<Button>(destino);
            btnHabilitarEdicion = botones.FirstOrDefault(b => b.Text.Contains("Habilitar"));
            btnGuardarYNotificar = botones.FirstOrDefault(b => b.Text.Contains("Guardar"));

            int yAdjunto = chkGmail.Bottom + 14;

            // 1. Botón 📎 Adjuntar Archivo
            btnAdjuntarArchivo = new Button
            {
                Text = "📎 Adjuntar Archivo",
                Location = new Point(25, yAdjunto),
                Width = 150,
                Height = 32,
                BackColor = Color.FromArgb(0, 121, 107),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdjuntarArchivo.FlatAppearance.BorderSize = 0;
            btnAdjuntarArchivo.Click += btnAdjuntarArchivo_Click;
            destino.Controls.Add(btnAdjuntarArchivo);

            // 2. Etiqueta del archivo seleccionado
            lblArchivoAdjunto = new Label
            {
                Text = "Ningún archivo seleccionado (PDF, DOCX, XLSX, JPG)",
                Location = new Point(185, yAdjunto + 7),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.DimGray
            };
            destino.Controls.Add(lblArchivoAdjunto);

            // 3. Botón ✖ Quitar adjunto
            btnQuitarAdjunto = new Button
            {
                Text = "✖",
                Location = new Point(540, yAdjunto + 2),
                Width = 28,
                Height = 28,
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnQuitarAdjunto.FlatAppearance.BorderSize = 0;
            btnQuitarAdjunto.Click += btnQuitarAdjunto_Click;
            destino.Controls.Add(btnQuitarAdjunto);

            int yBotonesFinales = yAdjunto + 46;

            // Ubicar o crear botones de acción debajo del adjunto
            if (btnHabilitarEdicion == null)
            {
                btnHabilitarEdicion = new Button
                {
                    Text = "Habilitar Edición",
                    Width = 160,
                    Height = 36,
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnHabilitarEdicion.FlatAppearance.BorderSize = 0;
                destino.Controls.Add(btnHabilitarEdicion);
            }
            btnHabilitarEdicion.Location = new Point(25, yBotonesFinales);
            btnHabilitarEdicion.Click -= btnHabilitarEdicion_Click;
            btnHabilitarEdicion.Click += btnHabilitarEdicion_Click;

            if (btnGuardarYNotificar == null)
            {
                btnGuardarYNotificar = new Button
                {
                    Text = "Guardar y Notificar",
                    Width = 185,
                    Height = 36,
                    BackColor = Color.FromArgb(0, 51, 153),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnGuardarYNotificar.FlatAppearance.BorderSize = 0;
                destino.Controls.Add(btnGuardarYNotificar);
            }
            btnGuardarYNotificar.Location = new Point(195, yBotonesFinales);
            btnGuardarYNotificar.Click -= btnGuardarYNotificar_Click;
            btnGuardarYNotificar.Click += btnGuardarYNotificar_Click;

            // Asegurar que todos los controles queden visibles y al frente
            foreach (Control c in destino.Controls)
            {
                if (c != btnQuitarAdjunto)
                    c.Visible = true;
            }
        }

        private T BuscarControl<T>(Control raiz) where T : Control
        {
            foreach (Control c in raiz.Controls)
            {
                if (c is T encontrado) return encontrado;
                T hijo = BuscarControl<T>(c);
                if (hijo != null) return hijo;
            }
            return null;
        }

        private System.Collections.Generic.List<T> BuscarTodos<T>(Control raiz) where T : Control
        {
            var lista = new System.Collections.Generic.List<T>();
            foreach (Control c in raiz.Controls)
            {
                if (c is T encontrado) lista.Add(encontrado);
                lista.AddRange(BuscarTodos<T>(c));
            }
            return lista;
        }

        private void btnAdjuntarArchivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Title = "Seleccionar archivo adjunto para la observación";
                dialogo.Filter = "Documentos e Imágenes|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.zip|Todos los archivos|*.*";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    FileInfo info = new FileInfo(dialogo.FileName);
                    if (info.Length > 15 * 1024 * 1024)
                    {
                        MessageBox.Show("El archivo supera el límite de 15 MB permitido.", "Archivo muy pesado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    rutaArchivoSeleccionado = dialogo.FileName;
                    lblArchivoAdjunto.Text = $"Archivo: {info.Name} ({(info.Length / 1024.0):F1} KB)";
                    lblArchivoAdjunto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    lblArchivoAdjunto.ForeColor = Color.FromArgb(0, 51, 153);
                    btnQuitarAdjunto.Visible = true;
                }
            }
        }

        private void btnQuitarAdjunto_Click(object sender, EventArgs e)
        {
            rutaArchivoSeleccionado = "";
            lblArchivoAdjunto.Text = "Ningún archivo seleccionado (PDF, DOCX, XLSX, JPG)";
            lblArchivoAdjunto.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblArchivoAdjunto.ForeColor = Color.DimGray;
            btnQuitarAdjunto.Visible = false;
        }

        private void CargarProyectosEnCombo()
        {
            if (cmbProyectos == null) return;

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();
                    string query = "SELECT IdProyecto, Titulo FROM Proyectos ORDER BY Titulo ASC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbProyectos.DataSource = dt;
                    cmbProyectos.DisplayMember = "Titulo";
                    cmbProyectos.ValueMember = "IdProyecto";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargando proyectos: " + ex.Message);
            }
        }

        private void btnHabilitarEdicion_Click(object sender, EventArgs e)
        {
            if (txtObservacion != null)
            {
                txtObservacion.ReadOnly = false;
                txtObservacion.BackColor = Color.White;
                txtObservacion.Focus();
            }
            if (txtCorreo != null)
            {
                txtCorreo.ReadOnly = false;
                txtCorreo.BackColor = Color.White;
            }
            MessageBox.Show("Edición habilitada. Ya puedes redactar o modificar la observación.", "Modo Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGuardarYNotificar_Click(object sender, EventArgs e)
        {
            string proyectoTitulo = cmbProyectos?.Text ?? "Proyecto General";
            int idProyecto = (cmbProyectos?.SelectedValue != null && int.TryParse(cmbProyectos.SelectedValue.ToString(), out int idP)) ? idP : 1;
            string observacion = txtObservacion?.Text.Trim() ?? "";
            string correoDestino = txtCorreo?.Text.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(observacion))
            {
                MessageBox.Show("Escribe una observación antes de guardar.", "Campo Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rutaGuardadaLocal = "";
            if (!string.IsNullOrEmpty(rutaArchivoSeleccionado) && File.Exists(rutaArchivoSeleccionado))
            {
                try
                {
                    string carpetaAdjuntos = Path.Combine(Application.StartupPath, "AdjuntosObservaciones");
                    Directory.CreateDirectory(carpetaAdjuntos);
                    string nombreUnico = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(rutaArchivoSeleccionado)}";
                    rutaGuardadaLocal = Path.Combine(carpetaAdjuntos, nombreUnico);
                    File.Copy(rutaArchivoSeleccionado, rutaGuardadaLocal, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo respaldar el archivo adjunto: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            string textoFinalBD = string.IsNullOrEmpty(rutaGuardadaLocal)
                ? observacion
                : $"{observacion} [Adjunto: {Path.GetFileName(rutaGuardadaLocal)}]";

            GuardarObservacionEnBD(idProyecto, textoFinalBD);

            if (chkGmail != null && chkGmail.Checked && !string.IsNullOrWhiteSpace(correoDestino))
            {
                EnviarCorreoConAdjunto(correoDestino, proyectoTitulo, observacion, rutaArchivoSeleccionado);
            }
            else
            {
                MessageBox.Show("¡Observación y archivo guardados correctamente en el sistema!", "Guardado Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnQuitarAdjunto_Click(null, EventArgs.Empty);
        }

        private void GuardarObservacionEnBD(int idProyecto, string comentario)
        {
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexionEstatica())
                {
                    con.Open();

                    string queryEntrega = "SELECT TOP 1 IdEntrega FROM Entregas WHERE IdProyecto = @IdP ORDER BY IdEntrega DESC";
                    SqlCommand cmdE = new SqlCommand(queryEntrega, con);
                    cmdE.Parameters.AddWithValue("@IdP", idProyecto);
                    object resEntrega = cmdE.ExecuteScalar();

                    int idEntrega = 0;
                    if (resEntrega != null)
                    {
                        idEntrega = Convert.ToInt32(resEntrega);
                    }
                    else
                    {
                        string insertEnt = "INSERT INTO Entregas (IdProyecto, Fase, FechaEntrega, Estado) VALUES (@IdP, 'Revisión General', GETDATE(), 'Pendiente'); SELECT SCOPE_IDENTITY();";
                        SqlCommand cmdNewE = new SqlCommand(insertEnt, con);
                        cmdNewE.Parameters.AddWithValue("@IdP", idProyecto);
                        idEntrega = Convert.ToInt32(cmdNewE.ExecuteScalar());
                    }

                    string queryObs = "INSERT INTO Observaciones (IdEntrega, Comentario, Fecha, Autor) VALUES (@IdE, @Com, GETDATE(), @Autor)";
                    SqlCommand cmdObs = new SqlCommand(queryObs, con);
                    cmdObs.Parameters.AddWithValue("@IdE", idEntrega);
                    cmdObs.Parameters.AddWithValue("@Com", comentario);
                    cmdObs.Parameters.AddWithValue("@Autor", SesionUsuario.NombreUsuario ?? "Docente Tutor");
                    cmdObs.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error SQL Observaciones: " + ex.Message);
            }
        }

        private void EnviarCorreoConAdjunto(string destino, string proyecto, string mensaje, string rutaAdjunto)
        {
            try
            {
                string correoRemitente = "darwin.avp@uemam.edu.ec";
                string claveAplicacion = "qooj uerb dqkw xbwa";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(correoRemitente, "Gestión de Proyectos - UEMAM");
                    mail.To.Add(destino);
                    mail.Subject = $"Nueva Observación - {proyecto}";
                    mail.Body = $"Estimado(a) estudiante,\n\nSe ha registrado una nueva observación en tu proyecto:\n\nProyecto: {proyecto}\nObservación:\n{mensaje}\n\nSaludos,\nUnidad Educativa María Auxiliadora de Manta";

                    if (!string.IsNullOrEmpty(rutaAdjunto) && File.Exists(rutaAdjunto))
                    {
                        mail.Attachments.Add(new Attachment(rutaAdjunto));
                    }

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(correoRemitente, claveAplicacion);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                MessageBox.Show("¡Observación guardada y enviada por Gmail con su archivo adjunto!", "Envío Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "La observación y el archivo se guardaron localmente en la base de datos, pero no se pudo enviar el correo (verifica las credenciales SMTP de Gmail):\n\n" + ex.Message,
                    "Guardado Localmente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}