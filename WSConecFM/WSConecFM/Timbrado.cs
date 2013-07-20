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
//* @package WSConecFM
//* @version 1.0
//*
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Xml;
using System.ServiceModel;
using System.Diagnostics;

namespace WSConecFM
{
    public class Timbrado
    {
        public string[] Timbrar(string layout, WSConecFM.requestTimbrarCFDI RequestTimbrarCFDI, string pathFile) 
        {
            string[] respuesta = { "0", "Timbrado del comprobante exitoso" };

            try
            {
                if (File.Exists(layout))
                {
                    StreamReader objReader = new StreamReader(layout, Encoding.UTF8);
                    layout = objReader.ReadToEnd();
                    objReader.Close();
                }

                // Codificar a base 64 el layout
                layout = Convert.ToBase64String(Encoding.UTF8.GetBytes(layout));

                // Agregar el XML codificado en base64 a la peticion SOAP
                RequestTimbrarCFDI.text2CFDI = layout;

                //  Conexion con el WS de Facturacion Moderna
                BasicHttpBinding binding = new BasicHttpBinding();
                setBinding(binding);

                // Direccion del servicio SOAP de Prueba
                EndpointAddress endpoint = new EndpointAddress(RequestTimbrarCFDI.urltimbrado);

                // Crear instancia al servisio SOAP de Timbrado
                WSLayoutFacturacionModerna.Timbrado_ManagerPort WSFModerna = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(binding, endpoint);

                // Ejecutar servicio de Timbrado
                Object objResponse = WSFModerna.requestTimbrarCFDI(RequestTimbrarCFDI);
                
                if (objResponse != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration;
                    XmlElement xmlElementBody;
                    xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "uft-8", "no");
                    xmlElementBody = xmlDoc.CreateElement("Container");
                    xmlDoc.InsertBefore(xmlElementBody, xmlDoc.DocumentElement);
                    XmlElement xmlParentNode;
                    xmlParentNode = xmlDoc.CreateElement("responseSoap");
                    xmlDoc.DocumentElement.PrependChild(xmlParentNode);
                    XmlNode[] nodosXmlResponse = (XmlNode[])objResponse;
                    foreach (XmlNode nodo in nodosXmlResponse)
                    {
                        if (nodo.InnerText.Length >= 1)
                        {
                            XmlElement xmlElemetResponse;
                            xmlElemetResponse = xmlDoc.CreateElement(nodo.Name.ToString());
                            XmlText xmlTextNode;
                            xmlTextNode = xmlDoc.CreateTextNode(nodo.InnerText.ToString());
                            xmlParentNode.AppendChild(xmlElemetResponse);
                            xmlElemetResponse.AppendChild(xmlTextNode);
                        }
                    }

                    //-->>Accedemos a los nodos de la respuesta del xml para obenter los valores retornados en base64 (xml, pdf, cbb, txt)
                    XmlElement xmlElementCFDI;
                    //-->>Xml certificado (CFDI)
                    xmlElementCFDI = (XmlElement)xmlDoc.GetElementsByTagName("xml").Item(0);

                    // Obtener UUID del Comprobante
                    string uuid;
                    XmlDocument cfdiXML = new XmlDocument();
                    byte[] binary = Convert.FromBase64String(xmlElementCFDI.InnerText);
                    String strOriginal = System.Text.Encoding.UTF8.GetString(binary);
                    cfdiXML.LoadXml(strOriginal);
                    XmlElement xmlElementTimbre;
                    xmlElementTimbre = (XmlElement)cfdiXML.GetElementsByTagName("tfd:TimbreFiscalDigital").Item(0);
                    uuid = xmlElementTimbre.GetAttribute("UUID");

                    // Guardar el comprobante a un archivo
                    //-->>Almacenamiento del Comprobante en XML
                    FileStream stream = new FileStream(pathFile + "\\" + uuid + ".xml", FileMode.Create); 
                    BinaryWriter writerBinary = new BinaryWriter(stream);
                    writerBinary.Write(Convert.FromBase64String(xmlElementCFDI.InnerText));
                    writerBinary.Close();
                    if (!File.Exists(pathFile + "\\" + uuid + ".xml"))
                    {
                        respuesta[0] = "1";
                        respuesta[1] = "Error: El comprobante en XML no se pudo escribir en " + pathFile + "\\" + uuid + ".xml";
                        return respuesta;
                    }

                    //-->>Representación impresa del CFDI en formato PDF
                    if (RequestTimbrarCFDI.generarPDF) 
                    {
                        XmlElement xmlElementPDF = (XmlElement)xmlDoc.GetElementsByTagName("pdf").Item(0);
                        //-->>Almacenamiento del Comprobante en PDF
                        stream = new FileStream(pathFile + "\\" + uuid + ".pdf", FileMode.Create);
                        writerBinary = new BinaryWriter(stream);
                        writerBinary.Write(Convert.FromBase64String(xmlElementPDF.InnerText));
                        writerBinary.Close();
                        if (!File.Exists(pathFile + "\\" + uuid + ".pdf"))
                        {
                            respuesta[0] = "1";
                            respuesta[1] = "Error: El comprobante en PDF no se pudo escribir en " + pathFile + "\\" + uuid + ".pdf";
                            return respuesta;
                        }
                    }

                    //-->>Representación impresa del CFDI en formato TXT
                    if (RequestTimbrarCFDI.generarTXT)
                    {
                        XmlElement xmlElementTXT = (XmlElement)xmlDoc.GetElementsByTagName("txt").Item(0);
                        //-->>Almacenamiento del Comprobante en PDF
                        stream = new FileStream(pathFile + "\\" + uuid + ".txt", FileMode.Create);
                        writerBinary = new BinaryWriter(stream);
                        writerBinary.Write(Convert.FromBase64String(xmlElementTXT.InnerText));
                        writerBinary.Close();
                        if (!File.Exists(pathFile + "\\" + uuid + ".txt"))
                        {
                            respuesta[0] = "1";
                            respuesta[1] = "Error: El comprobante en TXT no se pudo escribir en " + pathFile + "\\" + uuid + ".txt";
                            return respuesta;
                        }
                    }

                    //-->>Representación impresa del CFDI en formato PNG
                    if (RequestTimbrarCFDI.generarCBB)
                    {
                        XmlElement xmlElementCBB = (XmlElement)xmlDoc.GetElementsByTagName("png").Item(0);
                        //-->>Almacenamiento del Comprobante en PNG
                        stream = new FileStream(pathFile + "\\" + uuid + ".png", FileMode.Create);
                        writerBinary = new BinaryWriter(stream);
                        writerBinary.Write(Convert.FromBase64String(xmlElementCBB.InnerText));
                        writerBinary.Close();
                        if (!File.Exists(pathFile + "\\" + uuid + ".png"))
                        {
                            respuesta[0] = "1";
                            respuesta[1] = "Error: El comprobante en PNG no se pudo escribir en " + pathFile + "\\" + uuid + ".png";
                            return respuesta;
                        }
                    }

                    respuesta[1] = "Exito: Su comprobante lo encuentra en " + pathFile + "\\";
                    return respuesta;
                }
                else
                {
                    respuesta[0] = "1";
                    respuesta[1] = "El servicio de timbrado respondio con NULL";
                    return respuesta;
                }
            }
            catch(Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }
        }

        private void setBinding(BasicHttpBinding binding)
        {
            // Crear archivo app.config de forma manual
            binding.Name = "Timbrado_ManagerBinding";
            binding.CloseTimeout = System.TimeSpan.Parse("00:01:00");
            binding.OpenTimeout = System.TimeSpan.Parse("00:01:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:01:00");
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 65536;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 65536;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        }
    }

    public class Cancelado
    {
        public string[] Cancelar(WSConecFM.requestCancelarCFDI RequestCancelarCFDI)
        {
            string[] respuesta = { "0", "Exito: el UUID " + RequestCancelarCFDI.uuid + "ha sido cancelado" };

            try
            {
                //  Conexion con el WS de Facturacion Moderna
                BasicHttpBinding binding = new BasicHttpBinding();
                setBinding(binding);

                // Direccion del servicio SOAP de Prueba
                EndpointAddress endpoint = new EndpointAddress(RequestCancelarCFDI.urlcancelado);

                // Crear instancia al servisio SOAP de cancelado
                WSLayoutFacturacionModerna.Timbrado_ManagerPort WSFModerna = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(binding, endpoint);

                // Ejecutar servicio de Cancelado
                Object response = WSFModerna.requestCancelarCFDI(RequestCancelarCFDI);
                if (response != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration;
                    XmlElement xmlElementBody;
                    xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "uft-8", "no");
                    xmlElementBody = xmlDoc.CreateElement("Container");
                    xmlDoc.InsertBefore(xmlElementBody, xmlDoc.DocumentElement);
                    XmlElement xmlParentNode;
                    xmlParentNode = xmlDoc.CreateElement("responseSoap");
                    xmlDoc.DocumentElement.PrependChild(xmlParentNode);
                    XmlNode[] nodosXmlResponse = (XmlNode[])response;
                    foreach (XmlNode nodo in nodosXmlResponse)
                    {
                        if (nodo.InnerText.Length >= 1)
                        {
                            XmlElement xmlElemetResponse;
                            xmlElemetResponse = xmlDoc.CreateElement(nodo.Name.ToString());
                            XmlText xmlTextNode;
                            xmlTextNode = xmlDoc.CreateTextNode(nodo.InnerText.ToString());
                            xmlParentNode.AppendChild(xmlElemetResponse);
                            xmlElemetResponse.AppendChild(xmlTextNode);
                        }
                    }
                    XmlElement xmlElementMsg = (XmlElement)xmlDoc.GetElementsByTagName("Message").Item(0);
                    respuesta[1] = xmlElementMsg.InnerText;
                    return respuesta;
                }
                else
                {
                    respuesta[0] = "1";
                    respuesta[1] = "El servicio de Cancelado respondio con NULL";
                    return respuesta;
                }
                //respuesta[1] = response;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta[0] = "1";
                respuesta[1] = "Error: " + e.Message;
                return respuesta;
            }
        }

        private void setBinding(BasicHttpBinding binding)
        {
            // Crear archivo app.config de forma manual
            binding.Name = "Timbrado_ManagerBinding";
            binding.CloseTimeout = System.TimeSpan.Parse("00:01:00");
            binding.OpenTimeout = System.TimeSpan.Parse("00:01:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:01:00");
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 65536;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 65536;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        }
    }
}