using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Comprobante;
using ConnectionWSFM;
using System.IO;

namespace TimbradoCancelado
{
    public partial class frmEjemplos : Form
    {
        public frmEjemplos()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo para generar el comprobante fiscal
        /// </summary>
        private void cmdTimbraXML_Click(object sender, EventArgs e)
        {
            /*
              * * Puedes encontrar más ejemplos y documentación sobre estos archivos aquí. (Factura, Nota de Crédito, Recibo de Nómina y más...)
              * * Link: https://github.com/facturacionmoderna/Comprobantes
              * * Nota: Si deseas información adicional contactanos en www.facturacionmoderna.com
            */

            // Especificación de rutas especificas
            string currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string keyfile = currentPath + "\\utilerias\\certificados\\20001000000300022759.key";
            string certfile = currentPath + "\\utilerias\\certificados\\20001000000300022759.cer";
            string password = "12345678a";
            string xsltPath;
            
            if (checkBox1.Checked)
                xsltPath = currentPath + "\\utilerias\\xslt_retenciones\\retenciones.xslt";
            else
                xsltPath = currentPath + "\\utilerias\\xslt3_2\\cadenaoriginal_3_2.xslt";
            
            string xmlfile = txtXML.Text;
            string resultPath = currentPath + "\\resultados";

            Cursor.Current = Cursors.WaitCursor;
            if (!System.IO.File.Exists(xmlfile)) {
                MessageBox.Show("El archivo "+ xmlfile + " No existe");
                Environment.Exit(-1);
            }
            if (!System.IO.File.Exists(keyfile))
            {
                MessageBox.Show("El archivo " + keyfile + " No existe");
                Environment.Exit(-1);
            }
            if (!System.IO.File.Exists(certfile))
            {
                MessageBox.Show("El archivo " + certfile + " No existe");
                Environment.Exit(-1);
            }

            /* Crear instancia al objeto comprobante */
            Comprobante.Utilidades obj = new Comprobante.Utilidades();

            /*  OBTENER LA INFORMACION DEL CERTIFICADO
             * *    Los parametros enviados son:
             * *    1.- Ruta del certificado
            */
            string cert_b64 = "";
            string cert_No = "";
            if (obj.getInfoCertificate(certfile))
            {
                cert_b64 = obj.getCertificate();
                cert_No = obj.getCertificateNumber();
            }
            else
            {
                MessageBox.Show(obj.getMessage());
                Environment.Exit(-1);
            }

            /*  AGREGAR INFORMACION DEL CERTIFICADO AL XML ANTES DE GENERAR LA CADENA ORIGINAL
             * *    Los parametros enviados son:
             * *    1.- Xml (Puede ser una cadena o una ruta)
             * *    2.- Certificado codificado en base 64
             * *    3.- Numero de certificado
             * Retorna el XML Modificado
            */
            string newXml = obj.addDigitalStamp(xmlfile, cert_b64, cert_No);
            if (newXml.Equals(""))
            {
                MessageBox.Show(obj.getMessage());
                Environment.Exit(-1);
            }
            xmlfile = newXml;


            /* GENERAR CADENA ORIGINAL
             * *   Los paramteros enviado son:
             * *    1.- xml (Puede ser una cadena o una ruta)
             * *    2.- xslt (Ruta del archivo xslt, con el cual se construye la cadena original)
             * *   Retorna la cadena original
            */
            
            string cadenaO = obj.createOriginalChain(xmlfile, xsltPath);
            if (cadenaO.Equals(""))
            {
                MessageBox.Show(obj.getMessage());
                Environment.Exit(-1);
            }

            /* GENERAR EL SELLO DEL COMPROBANTE
             * *    Los parametros enviado son:
             * *    1.- archivo de llave privada (.key)
             * *    2.- Contraseña del archivo de llave privada
             * *    3.- Cadena Original (Puede ser una cadena o una ruta)
             * Retorna el sello en r_comprobante.message
            */
            string sello = obj.createDigitalStamp(keyfile, password, cadenaO);
            if (sello.Equals(""))
            {
                MessageBox.Show(obj.getMessage());
                Environment.Exit(-1);
            }
            

            /*  AGREGAR LA INFORMACION DEL SELLO AL XML
             * *    Los parametros enviados son:
             * *    1.- Xml (Puede ser una cadena o una ruta)
             * *    2.- Sello del comprobante
             * Retorna el XML Modificado
            */
            newXml = obj.addDigitalStamp(xmlfile, sello);
            if (newXml.Equals(""))
            {
                MessageBox.Show(obj.getMessage());
                Environment.Exit(-1);
            }

            /*  CREAR LA CONFIGURACION DE CONEXION CON EL SERVICIO SOAP
             * *    Los parametros configurables son:
             * *    1.- Nombre de usuario que se utiliza para la conexion al Web Service
             * *    2.- Contraseña del usuario que se utiliza para la conexion al Web Service
             * *    3.- RFC Emisor
             * *    4.- Habilitar el retorno del CBB
             * *    5.- Habilitar el retorno del TXT
             * *    6.- Habilitar el retorno del PDF
             * *    7.- URL del Web Service (endpoint)
             * *    8.- Habilitar debug para guardar Request y Response (Si se habilita, se debe de especificar una ruta del archivo log)
             * * La configuracion inicial es para el ambiente de pruebas
            */
            ConnectionFM conX = new ConnectionFM();
            conX.setDebugMode(true);
            conX.setLogFilePath(currentPath + "\\logs\\log.txt");
            conX.setGenerarPdf(true);


            /*  Timbrar Layout
             * *   Se envia el layout a timbrar, puede ser una xml o un txt, especificando la ruta del archivo
             * *   o un string conteniendo todo el layout
             */
            if (conX.timbrarLayout(newXml) == true)
            {
                byte[] byteXML = System.Convert.FromBase64String(conX.getXmlB64());
                System.IO.FileStream swxml = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".xml"))), System.IO.FileMode.Create);
                swxml.Write(byteXML, 0, byteXML.Length);
                swxml.Close();

                if (conX.getCbbB64() != "")
                {
                    byte[] byteCBB = System.Convert.FromBase64String(conX.getCbbB64());
                    System.IO.FileStream swcbb = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".png"))), System.IO.FileMode.Create);
                    swcbb.Write(byteCBB, 0, byteCBB.Length);
                    swcbb.Close();
                }
                if (conX.getPdfB64() != "")
                {
                    byte[] bytePDF = System.Convert.FromBase64String(conX.getPdfB64());
                    System.IO.FileStream swpdf = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".pdf"))), System.IO.FileMode.Create);
                    swpdf.Write(bytePDF, 0, bytePDF.Length);
                    swpdf.Close();
                }
                if (conX.getTxtB64() != "")
                {
                    byte[] byteTXT = System.Convert.FromBase64String(conX.getTxtB64());
                    System.IO.FileStream swtxt = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".txt"))), System.IO.FileMode.Create);
                    swtxt.Write(byteTXT, 0, byteTXT.Length);
                    swtxt.Close();
                }
                MessageBox.Show("Comprobante guardado en " + resultPath + "\\");
            }
            else
            {
                MessageBox.Show("[" + conX.getErrorCode() + "] " + conX.getErrorMessage());
            }

            Cursor.Current = Cursors.Default;
            Close();
        }

        /// <summary>
        /// Metodo para cancelar un comprobante por medio de su UUID
        /// </summary>
        private void cmdCancelarUUID_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string uuid = txtUUID.Text;
            string currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            ConnectionFM conX = new ConnectionFM();
            conX.setDebugMode(true);
            conX.setLogFilePath(currentPath + "\\logs\\log.txt");
            if (conX.cancelarCfdi(uuid) == true)
            {
                MessageBox.Show("[" + conX.getSuccessCode() + "] " + conX.getSuccessMessage());
            }
            else
            {
                MessageBox.Show("[" + conX.getErrorCode() + "] " + conX.getErrorMessage());
            }

            Cursor.Current = Cursors.Default;
            Environment.Exit(-1);
            Close();
        }

        /// <summary>
        /// Metodo para generar el comprobante mediante un Layout
        /// </summary>
        private void cmdTimbrarL_Click(object sender, EventArgs e)
        {
            /*
              * * Puedes encontrar más ejemplos y documentación sobre estos archivos aquí. (Factura, Nota de Crédito, Recibo de Nómina y más...)
              * * Link: https://github.com/facturacionmoderna/Comprobantes
              * * Nota: Si deseas información adicional contactanos en www.facturacionmoderna.com
            */

            Cursor.Current = Cursors.WaitCursor;
            string layoutFile = txtLayout.Text;
            string currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string resultPath = currentPath + "\\resultados";

            /*  CREAR LA CONFIGURACION DE CONEXION CON EL SERVICIO SOAP
             * *    Los parametros configurables son:
             * *    1.- Nombre de usuario que se utiliza para la conexion al Web Service
             * *    2.- Contraseña del usuario que se utiliza para la conexion al Web Service
             * *    3.- RFC Emisor
             * *    4.- Habilitar el retorno del CBB
             * *    5.- Habilitar el retorno del TXT
             * *    6.- Habilitar el retorno del PDF
             * *    7.- URL del Web Service (endpoint)
             * *    8.- Habilitar debug para guardar Request y Response (Si se habilita, se debe de especificar una ruta del archivo log)
             * * La configuracion inicial es para el ambiente de pruebas
            */
            ConnectionFM conX = new ConnectionFM();
            conX.setDebugMode(true);
            conX.setLogFilePath(currentPath + "\\logs\\log.txt");
            conX.setGenerarPdf(true);

            /*  Timbrar Layout
             * *   Se envia el layout a timbrar, puede ser una xml o un txt, especificando la ruta del archivo
             * *   o un string conteniendo todo el layout
             */
            if (conX.timbrarLayout(layoutFile) == true)
            {
                byte[] byteXML = System.Convert.FromBase64String(conX.getXmlB64());
                System.IO.FileStream swxml = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".xml"))), System.IO.FileMode.Create);
                swxml.Write(byteXML, 0, byteXML.Length);
                swxml.Close();

                if (conX.getCbbB64() != "")
                {
                    byte[] byteCBB = System.Convert.FromBase64String(conX.getCbbB64());
                    System.IO.FileStream swcbb = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".png"))), System.IO.FileMode.Create);
                    swcbb.Write(byteCBB, 0, byteCBB.Length);
                    swcbb.Close();
                }
                if (conX.getPdfB64() != "")
                {
                    byte[] bytePDF = System.Convert.FromBase64String(conX.getPdfB64());
                    System.IO.FileStream swpdf = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".pdf"))), System.IO.FileMode.Create);
                    swpdf.Write(bytePDF, 0, bytePDF.Length);
                    swpdf.Close();
                }
                if (conX.getTxtB64() != "")
                {
                    byte[] byteTXT = System.Convert.FromBase64String(conX.getTxtB64());
                    System.IO.FileStream swtxt = new System.IO.FileStream((resultPath + ("\\" + (conX.getUuid() + ".txt"))), System.IO.FileMode.Create);
                    swtxt.Write(byteTXT, 0, byteTXT.Length);
                    swtxt.Close();
                }
                MessageBox.Show("Comprobante guardado en " + resultPath + "\\");
            }
            else
            {
                MessageBox.Show("[" + conX.getErrorCode() + "] " + conX.getErrorMessage());
            }

            Cursor.Current = Cursors.Default;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtXML.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtLayout.Text = openFileDialog2.FileName;
            }
        }
    }
}
