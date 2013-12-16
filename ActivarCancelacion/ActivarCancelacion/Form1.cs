using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSConecFM;

namespace ActivarCancelacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            string fileCer = txtCer.Text;
            string fileKey = txtKey.Text;
            string keyPass = txtPass.Text;

            Cursor.Current = Cursors.WaitCursor;
            WSConecFM.Resultados r_wsconect = new WSConecFM.Resultados();

            /*  CREAR LA CONFIGURACION DE CONEXION CON EL SERVICIO SOAP
             * *    Los parametros configurables son:
             * *    1.- string UserID; Nombre de usuario que se utiliza para la conexion con SOAP
             * *    2.- string UserPass; Contraseña del usuario para conectarse a SOAP
             * *    3.- string emisorRFC; RFC del contribuyente
             * *    4.- string archivoKey; archivo llave
             * *    5.- string strCer; archivo de certificado
             * *    6.- string clave; contraseña del archivo llave del certificado
             * *    7.- string urlActivarCancelacion; URL de la conexion con SOAP
             * La configuracion inicial es para el ambiente de pruebas
            */
            activarCancelacion activarC = new activarCancelacion();
            /*
             * Si desea cambiar alguna configuracion realizarla solo realizar lo siguiente
             * activarC.UserID = "Miusuario";  Por poner un ejemplo
            */
            activarC.archivoCer = fileCer;
            activarC.archivoKey = fileKey;
            activarC.clave = keyPass;

            ActivarCancelado activation = new ActivarCancelado();
            r_wsconect = activation.Activacion(activarC);
            Cursor.Current = Cursors.Default;
            MessageBox.Show(r_wsconect.message);
            Close();
        }
    }
}
