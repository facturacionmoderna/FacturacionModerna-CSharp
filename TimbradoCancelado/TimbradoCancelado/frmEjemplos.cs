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
using WSConecFM;
using System.IO;

namespace TimbradoCancelado
{
    /* NOTAS
     * *    Todas las peticiones a los metodos de los Dlls retornan un objeto de tipo Resultados
     * *    que contiene los siguientes atributos:
     * *    1.- code; Codigo de error
     * *    2.- message; Mensaje de error, mensaje de exito o resultado
     * *    3.- status; solo toma los valores de True o False
     * *    4.- ncert; contiene el numero de certificado, solo en algunos casos
    */
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
            string keyfile = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.key";
            string certfile = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.cer";
            string password = "12345678a";
            string version = "3.2";
            string xmlfile = txtXML.Text;
            string path = "C:\\FacturacionModernaCSharp\\resultados";

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

            Comprobante.Resultados r_comprobante = new Comprobante.Resultados();
            WSConecFM.Resultados r_wsconect = new WSConecFM.Resultados();

            /* GENERAR CADENA ORIGINAL
             * *    Los paramteros enviado son:
             * *    1.- version del comprobante
             * *    2.- xml (Puede ser una cadena o una ruta)
             * Retorna la cadena original en r_comprobante.message
            */
            Cadena cadena = new Cadena();
            r_comprobante = cadena.GeneraCadena(version, xmlfile);
            if (!r_comprobante.status)
            {
                MessageBox.Show(r_comprobante.message);
                Environment.Exit(-1);
            }
            string cadenaO = r_comprobante.message;

            /* GENERAR EL SELLO DEL COMPROBANTE
             * *    Los parametros enviado son:
             * *    1.- archivo de llave privada (.key)
             * *    2.- Contraseña del archivo de llave privada
             * *    3.- Cadena Original (Puede ser una cadena o una ruta)
             * Retorna el sello en r_comprobante.message
            */
            Sello sello = new Sello();
            r_comprobante = sello.GeneraSello(keyfile, password, cadenaO);
            if (!r_comprobante.status)
            {
                MessageBox.Show(r_comprobante.message);
                Environment.Exit(-1);
            }
            string str_sello = r_comprobante.message;

            /*  OBTENER LA INFORMACION DEL CERTIFICADO
             * *    Los parametros enviados son:
             * *    1.- Ruta del certificado
             * Retorna el numero de certificado en r_comprobante.ncert
             * Retorna el certificado en base 64 en r_comprobante.message
            */
            r_comprobante = sello.obtenCertificado(certfile);
            if (!r_comprobante.status)
            {
                MessageBox.Show(r_comprobante.message);
                Environment.Exit(-1);
            }
            string cert_b64 = r_comprobante.message;
            string cert_No = r_comprobante.ncert;

            /*  AGREGAR LA INFORMACION DEL SELLO AL XML
             * *    Los parametros enviados son:
             * *    1.- Xml (Puede ser una cadena o una ruta)
             * *    2.- Sello del comprobante
             * *    3.- Certificado codificado en base 64
             * *    4.- Numero de certificado
             * Retorna el XML en r_comprobante.message
            */
            r_comprobante = sello.agregaSello(xmlfile, str_sello, cert_b64, cert_No);
            if (!r_comprobante.status)
            {
                MessageBox.Show(r_comprobante.message);
                Environment.Exit(-1);
            }
            xmlfile = r_comprobante.message;

            /*  CREAR LA CONFIGURACION DE CONEXION CON EL SERVICIO SOAP
             * *    Los parametros configurables son:
             * *    1.- string UserID; Nombre de usuario que se utiliza para la conexion con SOAP
             * *    2.- string UserPass; Contraseña del usuario para conectarse a SOAP
             * *    3.- string emisorRFC; RFC del contribuyente
             * *    4.- Boolean generarCBB; Indica si se desea generar el CBB
             * *    5.- Boolean generarTXT; Indica si se desea generar el TXT
             * *    6.- Boolean generarPDF; Indica si se desea generar el PDF
             * *    7.- string urlTimbrado; URL de la conexion con SOAP
             * La configuracion inicial es para el ambiente de pruebas
            */
            requestTimbrarCFDI reqt = new requestTimbrarCFDI();
            /*
             * Si desea cambiar alguna configuracion realizarla solo realizar lo siguiente
             * reqt.generarPDF = true;  Por poner un ejemplo
            */

            /*  TIMBRAR XML
             * *    Los parametros enviados son:
             * *    1.- XML; (Acepta una ruta o una cadena)
             * *    2.- Objeto con las configuraciones de conexion con SOAP
             * Retorna un objeto con los siguientes valores codificado en base 64:
             * *    1.- xml en base 64
             * *    2.- pdf en base 64
             * *    3.- png en base 64
             * *    4.- txt en base 64
             * Los valores de retorno van a depender de la configuracion enviada a la función
             */
            Timbrado timbra = new Timbrado();
            r_wsconect = timbra.Timbrar(xmlfile, reqt);
            if (!r_wsconect.status)
            {
                MessageBox.Show(r_wsconect.message);
                Environment.Exit(-1);
            }
            byte[] byteXML = System.Convert.FromBase64String(r_wsconect.xmlBase64);
            System.IO.FileStream swxml = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".xml"))), System.IO.FileMode.Create);
            swxml.Write(byteXML, 0, byteXML.Length);
            swxml.Close();
            if (reqt.generarCBB) {
                byte[] byteCBB = System.Convert.FromBase64String(r_wsconect.cbbBase64);
                System.IO.FileStream swcbb = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".png"))), System.IO.FileMode.Create);
                swcbb.Write(byteCBB, 0, byteCBB.Length);
                swcbb.Close();
            }
            if (reqt.generarPDF)
            {
                byte[] bytePDF = System.Convert.FromBase64String(r_wsconect.pdfBase64);
                System.IO.FileStream swpdf = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".pdf"))), System.IO.FileMode.Create);
                swpdf.Write(bytePDF, 0, bytePDF.Length);
                swpdf.Close();
            }
            if (reqt.generarTXT)
            {
                byte[] byteTXT = System.Convert.FromBase64String(r_wsconect.txtBase64);
                System.IO.FileStream swtxt = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".txt"))), System.IO.FileMode.Create);
                swtxt.Write(byteTXT, 0, byteTXT.Length);
                swtxt.Close();
            }

            MessageBox.Show("Comprobante guardado en "+ path + "\\");
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

            WSConecFM.Resultados r_wsconect = new WSConecFM.Resultados();

            requestCancelarCFDI reqc = new requestCancelarCFDI();

            Cancelado conex = new Cancelado();
            r_wsconect = conex.Cancelar(reqc, uuid);

            MessageBox.Show(r_wsconect.message);
            Cursor.Current = Cursors.Default;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
            string path = "C:\\FacturacionModernaCSharp\\resultados";
            WSConecFM.Resultados r_wsconect = new WSConecFM.Resultados();

            // Crear instancia, para los para metros enviados a requestTimbradoCFDI
            requestTimbrarCFDI reqt = new requestTimbrarCFDI();

            Timbrado timbra = new Timbrado();
            r_wsconect = timbra.Timbrar(layoutFile, reqt);
            if (!r_wsconect.status)
            {
                MessageBox.Show(r_wsconect.message);
                Close();
            }
            byte[] byteXML = System.Convert.FromBase64String(r_wsconect.xmlBase64);
            System.IO.FileStream swxml = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".xml"))), System.IO.FileMode.Create);
            swxml.Write(byteXML, 0, byteXML.Length);
            swxml.Close();
            if (reqt.generarCBB)
            {
                byte[] byteCBB = System.Convert.FromBase64String(r_wsconect.cbbBase64);
                System.IO.FileStream swcbb = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".png"))), System.IO.FileMode.Create);
                swcbb.Write(byteCBB, 0, byteCBB.Length);
                swcbb.Close();
            }
            if (reqt.generarPDF)
            {
                byte[] bytePDF = System.Convert.FromBase64String(r_wsconect.pdfBase64);
                System.IO.FileStream swpdf = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".pdf"))), System.IO.FileMode.Create);
                swpdf.Write(bytePDF, 0, bytePDF.Length);
                swpdf.Close();
            }
            if (reqt.generarTXT)
            {
                byte[] byteTXT = System.Convert.FromBase64String(r_wsconect.txtBase64);
                System.IO.FileStream swtxt = new System.IO.FileStream((path + ("\\" + (r_wsconect.uuid + ".txt"))), System.IO.FileMode.Create);
                swtxt.Write(byteTXT, 0, byteTXT.Length);
                swtxt.Close();
            }

            MessageBox.Show("Comprobante guardado en " + path + "\\");
            Cursor.Current = Cursors.Default;
            Close();
        }
    }
}
