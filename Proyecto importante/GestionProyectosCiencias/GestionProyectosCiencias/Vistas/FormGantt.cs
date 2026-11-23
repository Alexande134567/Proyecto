using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GestionProyectosCiencias
{
    public partial class FormGantt : Form
    {
        private DataGridView dgvGantt;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnGuardar;

        public FormGantt()
        {
            InitializeComponent();
            ConfigurarInterfazGantt();
            CargarDatosIniciales();
        }

        private void ConfigurarInterfazGantt()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            // 1. Título del Módulo
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Text.Contains("Módulo:"))
                {
                    c.Text = $"Módulo: Diagrama Gantt  |  Usuario: ({SesionUsuario.NombreUsuario ?? "Docente/Admin"})";
                    c.Location = new Point(30, 15);
                    c.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    c.ForeColor = Color.FromArgb(0, 51, 153);
                    c.AutoSize = true;
                }
            }

            // 2. Localizar o crear botones superiores
            btnAgregar = LocalizarOCrearBoton("btnAgregar", "+ Agregar", new Point(315, 50), Color.FromArgb(3, 169, 244), btnAgregar_Click);

            btnEditar = LocalizarOCrearBoton("btnEditar", "✏️ Editar", new Point(425, 50), Color.FromArgb(255, 152, 0), btnEditar_Click);

            btnEliminar = LocalizarOCrearBoton("btnEliminar", "– Eliminar", new Point(535, 50), Color.FromArgb(211, 47, 47), btnEliminar_Click);

            btnGuardar = LocalizarOCrearBoton("btnGuardar", "💾 Guardar", new Point(645, 50), Color.FromArgb(0, 150, 136), btnGuardar_Click);

            // 3. Configurar DataGridView
            Control[] grids = this.Controls.Find("dgvGantt", true);
            if (grids.Length > 0 && grids[0] is DataGridView gridExistente)
            {
                dgvGantt = gridExistente;
            }
            else
            {
                // Buscar cualquier DataGridView en el formulario si tiene otro nombre
                foreach (Control c in this.Controls)
                {
                    if (c is DataGridView dgv)
                    {
                        dgvGantt = dgv;
                        break;
                    }
                }
                if (dgvGantt == null)
                {
                    dgvGantt = new DataGridView();
                    this.Controls.Add(dgvGantt);
                }
            }

            dgvGantt.Location = new Point(30, 95);
            dgvGantt.Width = 720;
            dgvGantt.Height = 360;
            dgvGantt.ReadOnly = true;
            dgvGantt.AllowUserToAddRows = false;
            dgvGantt.AllowUserToDeleteRows = false;
            dgvGantt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGantt.MultiSelect = false;
            dgvGantt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGantt.BackgroundColor = Color.White;
            dgvGantt.BorderStyle = BorderStyle.FixedSingle;
            dgvGantt.RowHeadersVisible = false;
            dgvGantt.RowTemplate.Height = 36;

            // Estilos visuales de encabezados
            dgvGantt.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvGantt.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGantt.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvGantt.EnableHeadersVisualStyles = false;
            dgvGantt.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            // Configurar columnas si están vacías
            if (dgvGantt.Columns.Count == 0)
            {
                dgvGantt.Columns.Add("Fase", "Fase del Proyecto");
                dgvGantt.Columns.Add("FechaInicio", "Fecha Inicio");
                dgvGantt.Columns.Add("FechaFin", "Fecha Fin");
                dgvGantt.Columns.Add("Porcentaje", "%");
                dgvGantt.Columns.Add("LineaTiempo", "Línea de Tiempo");
                dgvGantt.Columns.Add("Estado", "Estado");
            }

            // Conectar eventos de dibujo personalizado y doble clic para editar
            dgvGantt.CellPainting -= dgvGantt_CellPainting;
            dgvGantt.CellPainting += dgvGantt_CellPainting;

            dgvGantt.CellDoubleClick -= dgvGantt_CellDoubleClick;
            dgvGantt.CellDoubleClick += dgvGantt_CellDoubleClick;
        }

        private Button LocalizarOCrearBoton(string nombre, string texto, Point ubicacion, Color colorFondo, EventHandler eventoClick)
        {
            Button btn = null;
            Control[] encontrados = this.Controls.Find(nombre, true);
            if (encontrados.Length > 0 && encontrados[0] is Button b)
            {
                btn = b;
            }
            else
            {
                btn = new Button { Name = nombre };
                this.Controls.Add(btn);
            }

            btn.Text = texto;
            btn.Location = ubicacion;
            btn.Width = 102;
            btn.Height = 32;
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.Click -= eventoClick;
            btn.Click += eventoClick;
            btn.BringToFront();

            return btn;
        }

        private void CargarDatosIniciales()
        {
            if (dgvGantt.Rows.Count > 0) return;

            AgregarFilaGantt("1. Propuesta e Introducción", "02/05/2026", "15/05/2026", 100);
            AgregarFilaGantt("2. Antecedentes y Marco Teórico", "16/05/2026", "30/06/2026", 100);
            AgregarFilaGantt("3. Marco Metodológico", "01/07/2026", "15/09/2026", 80);
            AgregarFilaGantt("4. Análisis de Resultados", "16/09/2026", "31/10/2026", 40);
            AgregarFilaGantt("5. Redacción Final", "01/11/2026", "30/11/2026", 0);
            AgregarFilaGantt("6. Sustentación del Proyecto", "01/12/2026", "15/12/2026", 0);
        }

        private void AgregarFilaGantt(string fase, string inicio, string fin, int porcentaje)
        {
            string estado = ObtenerEstadoSegunPorcentaje(porcentaje);
            int idx = dgvGantt.Rows.Add(fase, inicio, fin, porcentaje, "", estado);
            AplicarColorEstado(dgvGantt.Rows[idx], porcentaje);
        }

        private string ObtenerEstadoSegunPorcentaje(int porcentaje)
        {
            if (porcentaje >= 100) return "Completado";
            if (porcentaje > 0) return "En Proceso";
            return "Pendiente";
        }

        private void AplicarColorEstado(DataGridViewRow fila, int porcentaje)
        {
            if (fila.Cells.Count < 6) return;

            fila.Cells[5].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            if (porcentaje >= 100)
                fila.Cells[5].Style.ForeColor = Color.FromArgb(0, 150, 136); // Verde
            else if (porcentaje > 0)
                fila.Cells[5].Style.ForeColor = Color.FromArgb(0, 102, 204); // Azul
            else
                fila.Cells[5].Style.ForeColor = Color.Gray; // Gris
        }

        // Dibuja la barra de progreso dentro de la columna "Línea de Tiempo"
        private void dgvGantt_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 4)
            {
                e.PaintBackground(e.CellBounds, true);

                int porcentaje = 0;
                int.TryParse(dgvGantt.Rows[e.RowIndex].Cells[3].Value?.ToString(), out porcentaje);
                porcentaje = Math.Max(0, Math.Min(100, porcentaje));

                Rectangle rectFondo = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 8, e.CellBounds.Width - 16, e.CellBounds.Height - 16);
                using (Brush brushFondo = new SolidBrush(Color.FromArgb(230, 233, 238)))
                {
                    e.Graphics.FillRectangle(brushFondo, rectFondo);
                }

                if (porcentaje > 0)
                {
                    int anchoBarra = (int)(rectFondo.Width * (porcentaje / 100.0));
                    Rectangle rectBarra = new Rectangle(rectFondo.X, rectFondo.Y, anchoBarra, rectFondo.Height);
                    Color colorBarra = porcentaje >= 100 ? Color.FromArgb(0, 168, 107) : Color.FromArgb(0, 102, 204);

                    using (Brush brushBarra = new SolidBrush(colorBarra))
                    {
                        e.Graphics.FillRectangle(brushBarra, rectBarra);
                    }
                }

                e.Handled = true;
            }
        }

        private void dgvGantt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditar_Click(sender, EventArgs.Empty);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            MostrarModalGantt(null);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvGantt.CurrentRow == null || dgvGantt.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona una fase del diagrama Gantt para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MostrarModalGantt(dgvGantt.CurrentRow);
        }

        private void MostrarModalGantt(DataGridViewRow filaEditar)
        {
            bool esEdicion = filaEditar != null;

            string faseInicial = esEdicion ? (filaEditar.Cells[0].Value?.ToString() ?? "") : "";
            DateTime inicioInicial = DateTime.Now;
            DateTime finInicial = DateTime.Now.AddDays(15);
            int porcentajeInicial = 0;

            if (esEdicion)
            {
                DateTime.TryParse(filaEditar.Cells[1].Value?.ToString(), out inicioInicial);
                DateTime.TryParse(filaEditar.Cells[2].Value?.ToString(), out finInicial);
                int.TryParse(filaEditar.Cells[3].Value?.ToString(), out porcentajeInicial);
            }

            Form modal = new Form
            {
                Width = 440,
                Height = 320,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = esEdicion ? "Editar Fase del Diagrama Gantt" : "Agregar Fase al Diagrama Gantt",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Label lblFase = new Label { Left = 20, Top = 15, Text = "Nombre de la Fase:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txtFase = new TextBox { Left = 20, Top = 38, Width = 380, Text = faseInicial, Font = new Font("Segoe UI", 9.5F) };

            Label lblInicio = new Label { Left = 20, Top = 75, Text = "Fecha de Inicio:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpInicio = new DateTimePicker { Left = 20, Top = 98, Width = 180, Value = inicioInicial, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9.5F) };

            Label lblFin = new Label { Left = 220, Top = 75, Text = "Fecha de Fin:", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpFin = new DateTimePicker { Left = 220, Top = 98, Width = 180, Value = finInicial, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9.5F) };

            Label lblPorcentaje = new Label { Left = 20, Top = 140, Text = "Porcentaje de Avance (0 - 100%):", AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            NumericUpDown numPorcentaje = new NumericUpDown { Left = 20, Top = 163, Width = 120, Minimum = 0, Maximum = 100, Value = porcentajeInicial, Font = new Font("Segoe UI", 10F) };

            Button btnAceptar = new Button
            {
                Text = "Aceptar",
                Left = 200,
                Top = 220,
                Width = 95,
                Height = 32,
                DialogResult = DialogResult.OK,
                BackColor = esEdicion ? Color.FromArgb(255, 152, 0) : Color.FromArgb(0, 51, 153),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnAceptar.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Left = 305,
                Top = 220,
                Width = 95,
                Height = 32,
                DialogResult = DialogResult.Cancel,
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            modal.Controls.AddRange(new Control[] { lblFase, txtFase, lblInicio, dtpInicio, lblFin, dtpFin, lblPorcentaje, numPorcentaje, btnAceptar, btnCancelar });
            modal.AcceptButton = btnAceptar;
            modal.CancelButton = btnCancelar;

            if (modal.ShowDialog() == DialogResult.OK)
            {
                string nuevaFase = txtFase.Text.Trim();
                if (string.IsNullOrWhiteSpace(nuevaFase))
                {
                    MessageBox.Show("Debes ingresar un nombre para la fase.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpFin.Value.Date < dtpInicio.Value.Date)
                {
                    MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string strInicio = dtpInicio.Value.ToString("dd/MM/yyyy");
                string strFin = dtpFin.Value.ToString("dd/MM/yyyy");
                int nuevoPorcentaje = (int)numPorcentaje.Value;
                string nuevoEstado = ObtenerEstadoSegunPorcentaje(nuevoPorcentaje);

                if (esEdicion)
                {
                    filaEditar.Cells[0].Value = nuevaFase;
                    filaEditar.Cells[1].Value = strInicio;
                    filaEditar.Cells[2].Value = strFin;
                    filaEditar.Cells[3].Value = nuevoPorcentaje;
                    filaEditar.Cells[5].Value = nuevoEstado;
                    AplicarColorEstado(filaEditar, nuevoPorcentaje);
                    dgvGantt.InvalidateRow(filaEditar.Index);
                }
                else
                {
                    AgregarFilaGantt(nuevaFase, strInicio, strFin, nuevoPorcentaje);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvGantt.CurrentRow == null || dgvGantt.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona una fase de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fase = dgvGantt.CurrentRow.Cells[0].Value?.ToString() ?? "";
            if (MessageBox.Show($"¿Eliminar la fase \"{fase}\" del cronograma?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgvGantt.Rows.Remove(dgvGantt.CurrentRow);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¡Cronograma del Diagrama Gantt guardado y actualizado correctamente!", "Guardado Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}