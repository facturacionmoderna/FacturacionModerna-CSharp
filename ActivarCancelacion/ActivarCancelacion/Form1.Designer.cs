namespace ActivarCancelacion
{
    partial class Form1
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
            this.btnActivar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCer = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnActivar
            // 
            this.btnActivar.Location = new System.Drawing.Point(231, 210);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(257, 37);
            this.btnActivar.TabIndex = 0;
            this.btnActivar.Text = "Activar";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Certificado";
            // 
            // txtCer
            // 
            this.txtCer.Location = new System.Drawing.Point(187, 26);
            this.txtCer.Name = "txtCer";
            this.txtCer.Size = new System.Drawing.Size(485, 20);
            this.txtCer.TabIndex = 2;
            this.txtCer.Text = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.cer";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(187, 85);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(485, 20);
            this.txtKey.TabIndex = 3;
            this.txtKey.Text = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.key";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(215, 147);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(457, 20);
            this.txtPass.TabIndex = 4;
            this.txtPass.Text = "12345678a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Llave del certificado";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Contraseña del archivo llave";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 278);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.txtCer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnActivar);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activar Cancelación";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCer;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

