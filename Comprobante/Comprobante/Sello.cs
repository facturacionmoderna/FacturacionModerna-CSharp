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

namespace Comprobante
{
    public class Cadena
    {
        public string[] GeneraCadena(string xsltFile, string xmlFile, string cadenaPath)
        {
            string[] respuesta = { "0", "Generacion de Cadena Original con Exito" };
            try
            {
                XslCompiledTransform xsltDoc = new XslCompiledTransform(true);
                xsltDoc.Load(xsltFile);
                xsltDoc.Transform(xmlFile, cadenaPath);
                if (File.Exists(cadenaPath))
                {
                    return respuesta;
                }
                else
                {
                    respuesta[0] = "1";
                    respuesta[1] = "Error: No se creo la cadena original, Vuelva a intentarlo";
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }
        }
    }

    public class Sello
    {
        public string[] GeneraSello(string keyfile, string password, string originalchain) 
        {
            string[] respuesta = { "0", "Generacion de Sello con Exito" };
            try
            {
                string strSello = "";
                string strPathLlave = @keyfile;
                string strLlavePwd = password;
                if (File.Exists(originalchain))
                {
                    StreamReader objReader = new StreamReader(originalchain, Encoding.UTF8);
                    originalchain = objReader.ReadToEnd();
                    objReader.Close();
                }  else {
                    respuesta[0] = "1";
                    respuesta[1] = "Error: No se encuentra el archivo con la cadena original";
                    return respuesta;
                }
                string strCadenaOriginal = originalchain;
                System.Security.SecureString passwordSeguro = new System.Security.SecureString();
                passwordSeguro.Clear();
                foreach (char c in strLlavePwd.ToCharArray())
                passwordSeguro.AppendChar(c);
                byte[] llavePrivadaBytes = System.IO.File.ReadAllBytes(strPathLlave);
                RSACryptoServiceProvider rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivadaBytes, passwordSeguro);
                SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider();
                byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(originalchain), hasher);
                strSello = Convert.ToBase64String(bytesFirmados);
                if (strSello == "")
                {
                    respuesta[0] = "1";
                    respuesta[1] = "Error: El sello esta vacio";
                    return respuesta;
                }
                respuesta[1] = strSello;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }
        }

        public string[] obtenCertificado(string certificado) 
        {
            string[] respuesta = { "0", "Certificado Extraido con Exito" };
            try
            {
                string opensslPath = "OpenSSl.exe";
                Process process1 = new Process();
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process1.StartInfo.FileName = opensslPath;
                process1.StartInfo.Arguments = "enc -A -base64 -in " + certificado;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.ErrorDialog = false;
                process1.StartInfo.RedirectStandardOutput = true;
                process1.Start();
                String str_cert = process1.StandardOutput.ReadToEnd();
                respuesta[1] = str_cert;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }

        }

        public string[] agregaSello(string xmlfile, string sello, string certificado, string numCertificado)
        {
            string[] respuesta = { "0", "El sello fue agregado la comprobante con exito" };
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlfile);
                XmlNodeList xmlNodoLista = xmlDoc.GetElementsByTagName("cfdi:Comprobante");

                foreach (XmlNode nodo in xmlNodoLista)
                {
                    nodo.SelectSingleNode("@sello").InnerText = sello;
                    nodo.SelectSingleNode("@certificado").InnerText = certificado;
                    nodo.SelectSingleNode("@noCertificado").InnerText = numCertificado;
                }
                xmlDoc.Save(xmlfile);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }
        }
    }
}