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
    public class Cadena
    {
        public Resultados GeneraCadena(string version, string xmlFile)
        {
            version = version.Replace(".", "_");
            string cadenaO = "";
            string outputtxt = "output.txt";
            string outputxml = "output.xml";
            Resultados result = new Resultados();
            string xsltFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\utilerias\\xslt" + version + "\\cadenaoriginal_" + version + ".xslt";
            
            try
            {
                if (! File.Exists(xmlFile))
                {
                    FileStream stream = new FileStream(outputxml, FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(xmlFile);
                    writer.Close();
                    xmlFile = outputxml;
                }
                XslCompiledTransform xsltDoc = new XslCompiledTransform(true);
                xsltDoc.Load(xsltFile);
                xsltDoc.Transform(xmlFile, outputtxt);
                
                if (File.Exists(outputtxt))
                {
                    StreamReader objReader = new StreamReader(outputtxt, Encoding.UTF8);
                    cadenaO = objReader.ReadToEnd();
                    objReader.Close();
                    result.code = "C000";
                    result.message = cadenaO;
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "C001";
                    result.message = "Error: No se creo la cadena original, Vuelva a intentarlo";
                    result.status = false;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.code = "EX-001";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }
    }

    public class Sello
    {
        public Resultados GeneraSello(string keyfile, string password, string originalchain)
        {
            Resultados result = new Resultados();

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
                    result.code = "S000";
                    result.message = "Error: El sello esta vacio";
                    result.status = false;
                    return result;
                }
                result.code = "S001";
                result.message = strSello;
                result.status = true;
                return result;
            }
            catch (Exception e)
            {
                result.code = "EX-001";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

        public Resultados obtenCertificado(string certificado)
        {
            Resultados result = new Resultados();

            try
            {
                X509Certificate2 cert = new X509Certificate2(certificado);
                string ncert = ReverseString(Encoding.Default.GetString(cert.GetSerialNumber()));
                string str_cert = Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
                str_cert = str_cert.Replace("\r\n", "");
                result.code = "C000";
                result.message = str_cert;
                result.ncert = ncert;
                result.status = true;
                return result;
            }
            catch (Exception e)
            {
                result.code = "EX-001";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

        private string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public Resultados agregaSello(string xmlFile, string sello, string certificado, string numCertificado)
        {
            string outputxml = "output.xml";
            Resultados result = new Resultados();

            try
            {
                if (!File.Exists(xmlFile))
                {
                    FileStream stream = new FileStream(outputxml, FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(xmlFile);
                    writer.Close();
                    xmlFile = outputxml;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNodeList xmlNodoLista = xmlDoc.GetElementsByTagName("cfdi:Comprobante");

                foreach (XmlNode nodo in xmlNodoLista)
                {
                    nodo.SelectSingleNode("@sello").InnerText = sello;
                    nodo.SelectSingleNode("@certificado").InnerText = certificado;
                    nodo.SelectSingleNode("@noCertificado").InnerText = numCertificado;
                }
                xmlDoc.Save(xmlFile);

                if (File.Exists(xmlFile))
                {
                    StreamReader objReader = new StreamReader(xmlFile, Encoding.UTF8);
                    xmlFile = objReader.ReadToEnd();
                    objReader.Close();
                    result.code = "C000";
                    result.message = xmlFile;
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "C001";
                    result.message = "Error: No se creo el XML con el sello, Vuelva a intentarlo";
                    result.status = false;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.code = "EX-001";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

        public Resultados agregaSello(string xmlFile, string sello)
        {
            string outputxml = "output.xml";
            Resultados result = new Resultados();

            try
            {
                if (!File.Exists(xmlFile))
                {
                    FileStream stream = new FileStream(outputxml, FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(xmlFile);
                    writer.Close();
                    xmlFile = outputxml;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNodeList xmlNodoLista = xmlDoc.GetElementsByTagName("cfdi:Comprobante");

                foreach (XmlNode nodo in xmlNodoLista)
                {
                    nodo.SelectSingleNode("@sello").InnerText = sello;
                }
                xmlDoc.Save(xmlFile);

                if (File.Exists(xmlFile))
                {
                    StreamReader objReader = new StreamReader(xmlFile, Encoding.UTF8);
                    xmlFile = objReader.ReadToEnd();
                    objReader.Close();
                    result.code = "C000";
                    result.message = xmlFile;
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "C001";
                    result.message = "Error: No se creo el XML con el sello, Vuelva a intentarlo";
                    result.status = false;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.code = "EX-001";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

    }
}