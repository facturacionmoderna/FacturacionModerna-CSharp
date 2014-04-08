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
            this.SuspendLayout();
            // 
            // cmdTimbraXML
            // 
            this.cmdTimbraXML.Location = new System.Drawing.Point(389, 37);
            this.cmdTimbraXML.Name = "cmdTimbraXML";
            this.cmdTimbraXML.Size = new System.Drawing.Size(95, 27);
            this.cmdTimbraXML.TabIndex = 0;
            this.cmdTimbraXML.Text = "Timbrar XML";
            this.cmdTimbraXML.UseVisualStyleBackColor = true;
            this.cmdTimbraXML.Click += new System.EventHandler(this.cmdTimbraXML_Click);
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(12, 39);
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(371, 20);
            this.txtXML.TabIndex = 1;
            this.txtXML.Text = "C:\\FacturacionModernaCSharp\\ejemplos\\CFDI.xml";
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(12, 174);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.Size = new System.Drawing.Size(371, 20);
            this.txtUUID.TabIndex = 2;
            this.txtUUID.Text = "39DEC2FF-405E-4EB3-A94D-A838AEBD0762";
            // 
            // cmdCancelarUUID
            // 
            this.cmdCancelarUUID.Location = new System.Drawing.Point(389, 173);
            this.cmdCancelarUUID.Name = "cmdCancelarUUID";
            this.cmdCancelarUUID.Size = new System.Drawing.Size(94, 27);
            this.cmdCancelarUUID.TabIndex = 3;
            this.cmdCancelarUUID.Text = "Cancelar UUID";
            this.cmdCancelarUUID.UseVisualStyleBackColor = true;
            this.cmdCancelarUUID.Click += new System.EventHandler(this.cmdCancelarUUID_Click);
            // 
            // txtLayout
            // 
            this.txtLayout.Location = new System.Drawing.Point(12, 106);
            this.txtLayout.Name = "txtLayout";
            this.txtLayout.Size = new System.Drawing.Size(371, 20);
            this.txtLayout.TabIndex = 4;
            this.txtLayout.Text = "C:\\FacturacionModernaCSharp\\ejemplos\\ejemploTimbradoLayout.ini";
            this.txtLayout.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cmdTimbrarL
            // 
            this.cmdTimbrarL.Location = new System.Drawing.Point(389, 105);
            this.cmdTimbrarL.Name = "cmdTimbrarL";
            this.cmdTimbrarL.Size = new System.Drawing.Size(95, 27);
            this.cmdTimbrarL.TabIndex = 5;
            this.cmdTimbrarL.Text = "Timbrar Layout";
            this.cmdTimbrarL.UseVisualStyleBackColor = true;
            this.cmdTimbrarL.Click += new System.EventHandler(this.cmdTimbrarL_Click);
            // 
            // frmEjemplos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 229);
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
    }
}

