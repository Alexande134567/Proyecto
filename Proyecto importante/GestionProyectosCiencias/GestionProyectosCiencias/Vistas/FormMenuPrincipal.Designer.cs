namespace GestionProyectosCiencias
{
    partial class FormMenuPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInstitucion;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnProyectos;
        private System.Windows.Forms.Button btnEntregas;
        private System.Windows.Forms.Button btnObservaciones;
        private System.Windows.Forms.Button btnGantt;
        private System.Windows.Forms.Button btnNotificaciones;
        private System.Windows.Forms.Button btnCerrarSesion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.lblInstitucion = new System.Windows.Forms.Label();
            this.btnNotificaciones = new System.Windows.Forms.Button();
            this.btnGantt = new System.Windows.Forms.Button();
            this.btnObservaciones = new System.Windows.Forms.Button();
            this.btnEntregas = new System.Windows.Forms.Button();
            this.btnProyectos = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelContenido = new System.Windows.Forms.Panel();
            this.panelSidebar.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.panelSidebar.Controls.Add(this.btnCerrarSesion);
            this.panelSidebar.Controls.Add(this.lblInstitucion);
            this.panelSidebar.Controls.Add(this.btnNotificaciones);
            this.panelSidebar.Controls.Add(this.btnGantt);
            this.panelSidebar.Controls.Add(this.btnObservaciones);
            this.panelSidebar.Controls.Add(this.btnEntregas);
            this.panelSidebar.Controls.Add(this.btnProyectos);
            this.panelSidebar.Controls.Add(this.btnUsuarios);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(220, 600);
            this.panelSidebar.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(0, 555);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(220, 45);
            this.btnCerrarSesion.TabIndex = 7;
            this.btnCerrarSesion.Text = "Cerrar Sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // lblInstitucion
            // 
            this.lblInstitucion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInstitucion.ForeColor = System.Drawing.Color.White;
            this.lblInstitucion.Location = new System.Drawing.Point(10, 470);
            this.lblInstitucion.Name = "lblInstitucion";
            this.lblInstitucion.Size = new System.Drawing.Size(200, 50);
            this.lblInstitucion.TabIndex = 6;
            this.lblInstitucion.Text = "U.E. MARÍA AUXILIADORA\r\nMANTA";
            this.lblInstitucion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNotificaciones
            // 
            this.btnNotificaciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNotificaciones.FlatAppearance.BorderSize = 0;
            this.btnNotificaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificaciones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNotificaciones.ForeColor = System.Drawing.Color.White;
            this.btnNotificaciones.Location = new System.Drawing.Point(0, 225);
            this.btnNotificaciones.Name = "btnNotificaciones";
            this.btnNotificaciones.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnNotificaciones.Size = new System.Drawing.Size(220, 45);
            this.btnNotificaciones.TabIndex = 5;
            this.btnNotificaciones.Text = "Notificaciones";
            this.btnNotificaciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNotificaciones.UseVisualStyleBackColor = true;
            this.btnNotificaciones.Click += new System.EventHandler(this.btnNotificaciones_Click);
            // 
            // btnGantt
            // 
            this.btnGantt.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGantt.FlatAppearance.BorderSize = 0;
            this.btnGantt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGantt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnGantt.ForeColor = System.Drawing.Color.White;
            this.btnGantt.Location = new System.Drawing.Point(0, 180);
            this.btnGantt.Name = "btnGantt";
            this.btnGantt.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnGantt.Size = new System.Drawing.Size(220, 45);
            this.btnGantt.TabIndex = 4;
            this.btnGantt.Text = "Diagrama Gantt";
            this.btnGantt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGantt.UseVisualStyleBackColor = true;
            this.btnGantt.Click += new System.EventHandler(this.btnGantt_Click);
            // 
            // btnObservaciones
            // 
            this.btnObservaciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnObservaciones.FlatAppearance.BorderSize = 0;
            this.btnObservaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnObservaciones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnObservaciones.ForeColor = System.Drawing.Color.White;
            this.btnObservaciones.Location = new System.Drawing.Point(0, 135);
            this.btnObservaciones.Name = "btnObservaciones";
            this.btnObservaciones.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnObservaciones.Size = new System.Drawing.Size(220, 45);
            this.btnObservaciones.TabIndex = 3;
            this.btnObservaciones.Text = "Observaciones";
            this.btnObservaciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnObservaciones.UseVisualStyleBackColor = true;
            this.btnObservaciones.Click += new System.EventHandler(this.btnObservaciones_Click);
            // 
            // btnEntregas
            // 
            this.btnEntregas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEntregas.FlatAppearance.BorderSize = 0;
            this.btnEntregas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntregas.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEntregas.ForeColor = System.Drawing.Color.White;
            this.btnEntregas.Location = new System.Drawing.Point(0, 90);
            this.btnEntregas.Name = "btnEntregas";
            this.btnEntregas.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnEntregas.Size = new System.Drawing.Size(220, 45);
            this.btnEntregas.TabIndex = 2;
            this.btnEntregas.Text = "Entregas";
            this.btnEntregas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEntregas.UseVisualStyleBackColor = true;
            this.btnEntregas.Click += new System.EventHandler(this.btnEntregas_Click);
            // 
            // btnProyectos
            // 
            this.btnProyectos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProyectos.FlatAppearance.BorderSize = 0;
            this.btnProyectos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProyectos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnProyectos.ForeColor = System.Drawing.Color.White;
            this.btnProyectos.Location = new System.Drawing.Point(0, 45);
            this.btnProyectos.Name = "btnProyectos";
            this.btnProyectos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnProyectos.Size = new System.Drawing.Size(220, 45);
            this.btnProyectos.TabIndex = 1;
            this.btnProyectos.Text = "Proyectos";
            this.btnProyectos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProyectos.UseVisualStyleBackColor = true;
            this.btnProyectos.Click += new System.EventHandler(this.btnProyectos_Click);
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsuarios.FlatAppearance.BorderSize = 0;
            this.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnUsuarios.Location = new System.Drawing.Point(0, 0);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnUsuarios.Size = new System.Drawing.Size(220, 45);
            this.btnUsuarios.TabIndex = 0;
            this.btnUsuarios.Text = "Usuarios";
            this.btnUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsuarios.UseVisualStyleBackColor = true;
            this.btnUsuarios.Click += new System.EventHandler(this.btnUsuarios_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(220, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(780, 50);
            this.panelHeader.TabIndex = 1;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(193, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Módulo: Usuarios";
            // 
            // panelContenido
            // 
            this.panelContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenido.Location = new System.Drawing.Point(220, 50);
            this.panelContenido.Name = "panelContenido";
            this.panelContenido.Size = new System.Drawing.Size(780, 550);
            this.panelContenido.TabIndex = 2;
            // 
            // FormMenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Name = "FormMenuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unidad Educativa María Auxiliadora - Gestión de Proyectos";
            this.panelSidebar.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}