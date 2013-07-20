///***************************************************************************
//* Descripción: Ejemplo del uso de la clase WSConecFM de Facturacion Moderna para el
//* Timbrado y cancelacion de un comprobante generando un
//* archivo XML de un CFDI 3.2 y enviandolo a certificar.
//*
//* Nota: Esté ejemplo pretende ilustrar de manera general el proceso de sellado y
//* timbrado de un XML que cumpla con los requerimientos del SAT.
//*
//* Facturación Moderna :  http://www.facturacionmoderna.com
//* @author Benito Arango <benito.arango@facturacionmoderna.com>
//* @version 1.0
//*
//*****************************************************************************


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

namespace TimbradoCancelado
{
    public partial class frmEjemplos : Form
    {
        public frmEjemplos()
        {
            InitializeComponent();
        }

        private void cmdTimbraXML_Click(object sender, EventArgs e)
        {
            // Especificación de rutas especificas
            string keyfile = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.key";
            string certfile = "C:\\FacturacionModernaCSharp\\utilerias\\certificados\\20001000000200000192.cer";
            string password = "12345678a";
            string xsltfile = "C:\\FacturacionModernaCSharp\\utilerias\\xslt32\\cadenaoriginal_3_2.xslt";
            string xmlfile = txtXML.Text;
            string originalPath = "C:\\FacturacionModernaCSharp\\resultados\\cadenaOriginal.txt";
            string path = "C:\\FacturacionModernaCSharp\\resultados";
            string numCert = "20001000000200000192";

            // Generar la cadena original
            // Crear instancias de la clase cadena
            Cadena cadena = new Cadena();
            //Llamar la funcion generar cadena, (Regresa un arreglo, status y mensaje), enviando como parametros:
            // 1.- Ruta del archivo xslt, el cual contiene el esquema de la cadena original
            // 2.- Ruta del archivo xml, del cual se tomaran los datos para generar la cadena original
            // 3.- Ruta donde se guardará el archivo de la cadena original
            string [] r_cadena = cadena.GeneraCadena(xsltfile, xmlfile, originalPath);
            if (r_cadena[0] == "1")
            {
                MessageBox.Show(r_cadena[1]);
                this.Close();
            }

            // Generar el sello del comprobante
            // Crear instancia de la clase Sello
            Sello sello = new Sello();
            //Llamar la funcion generar sello, (Regresa un arreglo, status y mensaje), enviando como parametros:
            // 1.- Ruta de archivo de llave privada (.key)
            // 2.- Contraseña del archivo de llave privada
            // 3.- Ruta del archivo que contiene la cadena original
            string [] r_sello = sello.GeneraSello(keyfile, password, originalPath);
            if (r_sello[0] == "1")
            {
                MessageBox.Show(r_sello[1]);
                this.Close();
            }
            string str_sello = r_sello[1];

            // Obtener el contenido del certificado
            // Extrae el contenido del certificado (.cer)
            // Enviar como parametro la ruta del archivo del certificado
            string [] r_certificado = sello.obtenCertificado(certfile);
            if (r_certificado[0] == "1")
            {
                MessageBox.Show(r_certificado[1]);
                this.Close();
            }
            string str_certificado = r_certificado[1];

            // Agregar el sello generado al comprobante
            // Los parametros son:
            // 1.- Ruta del archivo XML
            // 2.- Cadena del sello digital
            // 3.- Cadena del certificado
            // 4.- Número de certificado (.cer)
            string[] r_agregasello = sello.agregaSello(xmlfile, str_sello, str_certificado, numCert);
            if (r_agregasello[0] == "1")
            {
                MessageBox.Show(r_agregasello[1]);
                this.Close();
            }

            // Crear instancia a la clase de timbrado
            Timbrado conex = new Timbrado();

            // Crear instancia a requestTimbrarCFDI, Regresa los parametros necesarios para poder realziar la conexion SOAP (Ya cuenta con los parametros necesarios para la conexion)
            // Los posibles valores son:
            // string text2CFDI: Archivo XML codificado en base 64 para ser timbrado
            // string UserID: Nombre de usuario para conexion con SOAP
            // string UserPass: Contraseña de usuario para conexion con SOAP
            // string emisorRFC: RFC del emisor
            // Boolean generarCBB: Retorna el Codigo de Barras Bidimension, Si esta es TRUE, generarPDF y generarTXT se convierten en FALSE
            // Boolean generarTXT: Retorna el comprobante en TXT
            // Boolean generarPDF: Retorna el comprobante en PDF
            // string urltimbrado: URL de acceso al servisio SOAP
            requestTimbrarCFDI reqt = new requestTimbrarCFDI();
            // Para cambiar un valor hacer lo del siguiente ejemplo:
            // reqt.generarPDF = true;

            // Ejecutar Timbrado del comprobante
            string[] r_timbrar = conex.Timbrar(xmlfile, reqt, path);
            MessageBox.Show(r_timbrar[1]);

            // Fin de Timbrado
            this.Close();
        }

        private void cmdCancelarUUID_Click(object sender, EventArgs e)
        {
            // Cancelar un comprobante por medio de su UUID
            // Crear la instancia de la clase Cancelado
            Cancelado conex = new Cancelado();
            // Crear instancia a requestCancelarCFDI, Regresa los parametros necesarios para poder realziar la conexion SOAP (Ya cuenta con los parametros necesarios para la conexion)
            // Los posibles valores son:
            // string UserID: Nombre de usuario para conexion con SOAP
            // string UserPass: Contraseña de usuario para conexion con SOAP
            // string emisorRFC: RFC del emisor
            // Boolean uuid: UUID que se va a cancelar
            // string urlcancelado: URL de acceso al servisio SOAP
            requestCancelarCFDI reqc = new requestCancelarCFDI();
            reqc.uuid = txtUUID.Text;

            // Ejecutar Cancelado del comprobante
            string[] r_cancelar = conex.Cancelar(reqc);
            MessageBox.Show(r_cancelar[1]);

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdTimbrarL_Click(object sender, EventArgs e)
        {
            string layoutFile = txtLayout.Text;
            string path = "C:\\FacturacionModernaCSharp\\resultados";

            // Crear instancia a la clase de timbrado
            Timbrado conex = new Timbrado();

            // Crear instancia, para los para metros enviados a requestTimbradoCFDI
            requestTimbrarCFDI reqt = new requestTimbrarCFDI();

            // Ejecutar Timbrado del comprobante
            string[] r_timbrar = conex.Timbrar(layoutFile, reqt, path);
            MessageBox.Show(r_timbrar[1]);

            // Fin de Timbrado
            this.Close();
        }
    }
}
