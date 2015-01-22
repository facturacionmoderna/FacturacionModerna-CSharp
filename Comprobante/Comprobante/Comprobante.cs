using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Diagnostics;
using JavaScience;
using System.Security.Cryptography;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Comprobante
{
    ///<summary>
    ///Contiene funciones para la generacion del sello del comprobante.
    ///</summary>
    ///<remarks>
    ///Provee de herramientas que facilitan la generacion de la cadena orignal y sello digital,así como la extracción de 
    ///información del certificado (número de certificado y contenido del certificado en base 64)
    ///</remarks>
    public class Utilidades
    {
        private string originalChain;
        private string message;
        private string code;
        private string digitalStamp;
        private string certificateNumber;
        private string certificate;
        private string newXml;

        ///<summary>
        ///Constructor
        ///</summary>
        public Utilidades()
        {
            this.originalChain = "";
            this.message = "Cadena original creada con éxito";
            this.code = "C-000";
            this.digitalStamp = "";
            this.certificateNumber = "";
            this.certificate = "";
            this.newXml = "";
        }

        ///<summary>
        ///Construye la cadena original del comprobante.
        ///</summary>
        ///<return>
        ///Devuelve cadena original como un string
        ///</return>
        ///<param name="xml">
        ///Ruta del archivo xml en disco a leer, o xml como cadena.
        ///</param>
        ///<param name="xslt">
        ///Ruta del archivo xslt en disco a leer
        ///</param>
        public string createOriginalChain(string xml, string xslt)
        {
            string currentPath = Directory.GetCurrentDirectory();
            string newPath = Path.GetDirectoryName(xslt);

            if (xml.Equals(""))
            {
                this.code = "XML-001";
                this.message = "Error: El xml esta vacio o no existe el archivo";
                return this.originalChain;
            }

            if (!System.IO.File.Exists(xslt))
            {
                this.code = "XSLT-001";
                this.message = "Error: No se encuentra el xslt en la ruta " + xslt;
                return this.originalChain;
            }

            try
            {
                StreamReader objReader = new StreamReader(xslt, Encoding.UTF8);
                xslt = objReader.ReadToEnd();
                objReader.Close();

                if (File.Exists(xml))
                {
                    StreamReader objReader2 = new StreamReader(xml, Encoding.UTF8);
                    xml = objReader2.ReadToEnd();
                    objReader2.Close();
                }

                string output = String.Empty;
                using (StringReader srt = new StringReader(xslt))
                using (StringReader sri = new StringReader(xml))
                {
                    using (XmlReader xrt = XmlReader.Create(srt))
                    using (XmlReader xri = XmlReader.Create(sri))
                    {
                        Environment.CurrentDirectory = (newPath);
                        XslCompiledTransform xsltT = new XslCompiledTransform();
                        xsltT.Load(xrt);
                        using (StringWriter sw = new StringWriter())
                        using (XmlWriter xwo = XmlWriter.Create(sw, xsltT.OutputSettings))
                        {
                            xsltT.Transform(xri, xwo);
                            output = sw.ToString();
                            this.originalChain = output;
                            this.code = "S-001";
                        }
                        Environment.CurrentDirectory = (currentPath);

                        if (this.originalChain.Equals("|||"))
                        {
                            this.code = "E-0003";
                            this.message = "Error al generar la cadena original, Compruebe que el XSLT corresponda al xml enviado";
                            this.originalChain = "";
                            return this.originalChain;
                        }
                    }
                }

                if (this.originalChain.Equals(""))
                {
                    this.code = "E-0001";
                    this.message = "Error al generar la cadena original";
                }
            }
            catch (Exception e)
            {
                this.code = "EXCEPTION-0001";
                this.message = "Error: " + e.Message;
            }
            /* Regresar cadena original */
            return this.originalChain;
        }

        ///<summary>
        ///Genera el sello del comprobante.
        ///</summary>
        ///<return>
        ///Devuelve sello del comprobante como un string
        ///</return>
        ///<param name="keyfile">
        ///Ruta del archivo .key en disco a leer.
        ///</param>
        ///<param name="password">
        ///Contraseña del archivo .key, en string
        ///</param>
        ///<param name="originalChain">
        ///Cadena original del comprobante en cadena, o bien la ruta del archivo a leer en disco
        ///</param>
        public string createDigitalStamp(string keyfile, string password, string originalChain)
        {
            if (password.Equals(""))
            {
                this.code = "PASS-001";
                this.message = "Error: Contraseña vacia";
                return this.digitalStamp;
            }
            if (!System.IO.File.Exists(keyfile))
            {
                this.code = "KEY-001";
                this.message = "Error: No se encuentra el archivo key en la ruta " + keyfile;
                return this.digitalStamp;
            }
            if (originalChain.Equals(""))
            {
                this.code = "CHAIN-001";
                this.message = "Error: Cadena original vacia";
                return this.digitalStamp;
            }

            try
            {
                string strSello = "";
                string strPathLlave = @keyfile;
                string strLlavePwd = password;
                if (File.Exists(originalChain))
                {
                    StreamReader objReader = new StreamReader(originalChain, Encoding.UTF8);
                    originalChain = objReader.ReadToEnd();
                    objReader.Close();
                }
                string strCadenaOriginal = originalChain;
                System.Security.SecureString passwordSeguro = new System.Security.SecureString();
                passwordSeguro.Clear();
                foreach (char c in strLlavePwd.ToCharArray())
                    passwordSeguro.AppendChar(c);
                byte[] llavePrivadaBytes = System.IO.File.ReadAllBytes(strPathLlave);
                RSACryptoServiceProvider rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivadaBytes, passwordSeguro);
                SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider();
                byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(originalChain), hasher);
                strSello = Convert.ToBase64String(bytesFirmados);
                if (strSello == "")
                {
                    this.code = "E-0001";
                    this.message = "Error: El sello esta vacio";
                }
                this.code = "S-0001";
                this.digitalStamp = strSello;
                this.message = "Sello del comprobante creado con éxito";
            }
            catch (Exception e)
            {
                this.code = "EXCEPTION-0001";
                this.message = "Error: " + e.Message;
            }

            return this.digitalStamp;
        }

        ///<summary>
        ///Obtiene el numero de certificado y certificado codificado en base 64.
        ///</summary>
        ///<return>
        ///Devueve true, cuando es exitoso, false cuando ocurre algun error, la información la almacena en las variables 
        ///privadas certificadeNumber y certificate, las cuales son accedidas  por los metodos get
        ///</return>
        ///<param name="certificate">
        ///Ruta del archivo .cer en disco a leer.
        ///</param>
        public Boolean getInfoCertificate(string certificate)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(certificate);
                string ncert = ReverseString(Encoding.Default.GetString(cert.GetSerialNumber()));
                string str_cert = Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
                str_cert = str_cert.Replace("\r\n", "");
                this.code = "CERT-0001";
                this.message = "Número de certificado obtenido con éxito";
                this.certificateNumber = ncert;
                this.certificate = str_cert;
            }
            catch (Exception e)
            {
                this.code = "EX-001";
                this.message = "Error: " + e.Message;
                return false;
            }
            return true;
        }

        ///<summary>
        ///Agrega el certificado y numero de certificado al archivo xml del comprobante.
        ///</summary>
        ///<return>
        ///Devuelve cadena con el nuevo xml
        ///</return>
        ///<param name="xmlfile">
        ///Rutal del archivo xml en disco, o bien string del xml, al que se le agregará los elementos.
        ///</param>
        ///<param name="certificate">
        ///String que contiene el certificado (cadena)
        ///</param>
        ///<param name="certificateNumber">
        ///String que contiene el numero de certificado (cadena)
        ///</param>
        public string addDigitalStamp(string xmlfile, string certificate, string certificateNumber)
        {
            try
            {
                if (File.Exists(xmlfile))
                {
                    StreamReader objReader = new StreamReader(xmlfile, Encoding.UTF8);
                    xmlfile = objReader.ReadToEnd();
                    objReader.Close();
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlfile);

                XmlNodeList xmlNodoLista = xmlDoc.GetElementsByTagName("cfdi:Comprobante");
                foreach (XmlNode nodo in xmlNodoLista)
                {
                    nodo.SelectSingleNode("@certificado").InnerText = certificate;
                    nodo.SelectSingleNode("@noCertificado").InnerText = certificateNumber;
                }

                XmlNodeList xmlNodoLista2 = xmlDoc.GetElementsByTagName("retenciones:Retenciones");
                foreach (XmlNode nodo in xmlNodoLista2)
                {
                    nodo.SelectSingleNode("@Cert").InnerText = certificate;
                    nodo.SelectSingleNode("@NumCert").InnerText = certificateNumber;
                }

                this.code = "C-00005";
                this.message = "Archivo xml modificado con exito, se agregó información del certificado";
                this.newXml = xmlDoc.InnerXml;
            }
            catch (Exception e)
            {
                this.code = "EX-001";
                this.message = "Error: " + e.Message;
            }
            return this.newXml;
        }


        ///<summary>
        ///Agrega el sello al xml del comprobante.
        ///</summary>
        ///<return>
        ///Devuelve cadena con el nuevo xml
        ///</return>
        ///<param name="xmlfile">
        ///Rutal del archivo xml en disco, o bien string del xml, al que se le agregará el sello.
        ///</param>
        ///<param name="digitalStamp">
        ///String que contiene el sello del comprobante (cadena)
        ///</param>
        public string addDigitalStamp(string xmlfile, string digitalStamp)
        {
            this.newXml = "";
            try
            {
                if (File.Exists(xmlfile))
                {
                    StreamReader objReader = new StreamReader(xmlfile, Encoding.UTF8);
                    xmlfile = objReader.ReadToEnd();
                    objReader.Close();
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlfile);

                XmlNodeList xmlNodoLista = xmlDoc.GetElementsByTagName("cfdi:Comprobante");
                foreach (XmlNode nodo in xmlNodoLista)
                {
                    nodo.SelectSingleNode("@sello").InnerText = digitalStamp;
                }

                XmlNodeList xmlNodoLista2 = xmlDoc.GetElementsByTagName("retenciones:Retenciones");
                foreach (XmlNode nodo in xmlNodoLista2)
                {
                    nodo.SelectSingleNode("@Sello").InnerText = digitalStamp;
                }

                this.code = "C-00005";
                this.message = "Archivo xml modificado con exito, se agregó el sello";
                this.newXml = xmlDoc.InnerXml;
            }
            catch (Exception e)
            {
                this.code = "EX-001";
                this.message = "Error: " + e.Message;
            }
            return this.newXml;
        }


        public string getMessage()
        {
            return this.message;
        }


        public string getCertificate()
        {
            return this.certificate;
        }

        public string getCertificateNumber()
        {
            return this.certificateNumber;
        }

        private string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}