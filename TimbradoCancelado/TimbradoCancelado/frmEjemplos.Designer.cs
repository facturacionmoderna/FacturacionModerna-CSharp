namespace TimbradoCancelado
{
    partial class frmEjemplos
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdTimbraXML = new System.Windows.Forms.Button();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.cmdCancelarUUID = new System.Windows.Forms.Button();
            this.txtLayout = new System.Windows.Forms.TextBox();
            this.cmdTimbrarL = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdTimbraXML
            // 
            this.cmdTimbraXML.Location = new System.Drawing.Point(12, 61);
            this.cmdTimbraXML.Name = "cmdTimbraXML";
            this.cmdTimbraXML.Size = new System.Drawing.Size(175, 37);
            this.cmdTimbraXML.TabIndex = 0;
            this.cmdTimbraXML.Text = "Timbrar XML";
            this.cmdTimbraXML.UseVisualStyleBackColor = true;
            this.cmdTimbraXML.Click += new System.EventHandler(this.cmdTimbraXML_Click);
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(12, 12);
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(371, 20);
            this.txtXML.TabIndex = 1;
            this.txtXML.Text = "---- Seleccionar archivo xml ----";
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(12, 225);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUUID.Size = new System.Drawing.Size(478, 20);
            this.txtUUID.TabIndex = 2;
            this.txtUUID.Text = "39DEC2FF-405E-4EB3-A94D-A838AEBD0762";
            this.txtUUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmdCancelarUUID
            // 
            this.cmdCancelarUUID.Location = new System.Drawing.Point(159, 251);
            this.cmdCancelarUUID.Name = "cmdCancelarUUID";
            this.cmdCancelarUUID.Size = new System.Drawing.Size(175, 37);
            this.cmdCancelarUUID.TabIndex = 3;
            this.cmdCancelarUUID.Text = "Cancelar UUID";
            this.cmdCancelarUUID.UseVisualStyleBackColor = true;
            this.cmdCancelarUUID.Click += new System.EventHandler(this.cmdCancelarUUID_Click);
            // 
            // txtLayout
            // 
            this.txtLayout.Location = new System.Drawing.Point(12, 129);
            this.txtLayout.Name = "txtLayout";
            this.txtLayout.Size = new System.Drawing.Size(371, 20);
            this.txtLayout.TabIndex = 4;
            this.txtLayout.Text = "---- Selecciona archivo layout ----";
            // 
            // cmdTimbrarL
            // 
            this.cmdTimbrarL.Location = new System.Drawing.Point(12, 155);
            this.cmdTimbrarL.Name = "cmdTimbrarL";
            this.cmdTimbrarL.Size = new System.Drawing.Size(175, 37);
            this.cmdTimbrarL.TabIndex = 5;
            this.cmdTimbrarL.Text = "Timbrar Layout";
            this.cmdTimbrarL.UseVisualStyleBackColor = true;
            this.cmdTimbrarL.Click += new System.EventHandler(this.cmdTimbrarL_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML Files|*.xml";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(384, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 28);
            this.button1.TabIndex = 6;
            this.button1.Text = "Examinar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(384, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 30);
            this.button2.TabIndex = 7;
            this.button2.Text = "Examinar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Filter = "TXT Files (*.txt)|*.txt|INI Files (*.ini)|*.ini";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 38);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(191, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Timbrar Archivo xml de retenciones";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // frmEjemplos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 304);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdTimbrarL);
            this.Controls.Add(this.txtLayout);
            this.Controls.Add(this.cmdCancelarUUID);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.cmdTimbraXML);
            this.Name = "frmEjemplos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ejemplos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdTimbraXML;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Button cmdCancelarUUID;
        private System.Windows.Forms.TextBox txtLayout;
        private System.Windows.Forms.Button cmdTimbrarL;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

