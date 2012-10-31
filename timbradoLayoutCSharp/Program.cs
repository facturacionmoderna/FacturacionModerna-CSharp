using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


namespace timbradoLayoutCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Argumentos esperados:  Ruta del layout, Nombre de los archivos de respuesta(Opcional en este ejemplo) y la ruta de almacenamiento
            Ruta del layout: Ruta absoluta del archivo de texto contenedor del layout, para efectos de pruebas se incluyó dentro de los parámetros de configuración del proyecto un solo argumento "C:/factura_en_texto_ejemplo.txt"
            Nombre de archivos de respuesta: El nombre con el que se almacenara los archivos xml y pdf retornados de la llamada al Web Service de Facturación Moderna
            ruta de almacenamiento : Ruta de almacenamiento de los archivos de respuesta (xml y pdf) por default se utilizó la partición "C:" */
            WSLayoutFacturacionModerna.Timbrado_ManagerPortClient WSLayoutFM = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(); //-->>Objeto encargado de las peticiones al Web Service
            String nameFileResponse, pathFile;
            nameFileResponse = pathFile = "";
            if (args.Length >= 1)
            {
                string pathLayout;
                pathLayout = args[0].ToString();
                if (!File.Exists(pathLayout))
                {
                    Console.WriteLine("El layout especificado no existe en el directorio de archivos: " + pathLayout);
                    Console.ReadLine();
                    return;
                }
                else {
                    TextReader textLayout;
                    textLayout = new StreamReader(pathLayout); //-->> Lectura del archivo contenedor del layout
                    requestTimbrarCFDI RequestTimbrarCFDI = new requestTimbrarCFDI(); //-->>Clase contenedora de los parámetros de conexión del Web service de timbrado (vease classRequestTimbrarCFDI.cs en el explorador de soluciones)
                    RequestTimbrarCFDI.text2CFDI = Convert.ToBase64String(new System.Text.UTF8Encoding().GetBytes(textLayout.ReadToEnd())); //-->>Conversión requerida del layout a Base64
                    Object objResponse; //-->> Objeto contenedor de la respuesta del web service
                    objResponse = WSLayoutFM.requestTimbrarCFDI(RequestTimbrarCFDI); //-->>Realizamos la petición enviando los parámetros al método del WS
                    //-->>Finalizada la llamada damos tratamiento al responseSoap como un objeto xml para una mayor flexibilidad
                    if (objResponse != null) { //-->>Respuesta satisfactoria del WS
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
                        //-->>Accedemos a los nodos de la respuesta del xml para obenter el cfdi y el pdf retornados en Base64
                        XmlElement xmlElementCFDI;
                        xmlElementCFDI = (XmlElement)xmlDoc.GetElementsByTagName("xml").Item(0); //-->>Xml certificado (CFDI)
                        XmlElement xmlElementPDF;
                        xmlElementPDF = (XmlElement)xmlDoc.GetElementsByTagName("pdf").Item(0); //-->>Representación impresa del CFDI en formato PDF
                        try
                        {
                            nameFileResponse = "";
                            pathFile = "C:\\"; //-->> Por default almacenamos los archivos en la partición C
                            if (args.Length >= 2) //-->>El argumento con índice 1 (Nombre del archivo de salida) se usa para nombrar los archivos contenedores de la respuesta del Web Service, para este ejemplo se omitió dicho parámetro y se utiliza el UUID del CFDI
                            {
                                nameFileResponse = args[1];
                            }
                            if (args.Length >= 3) //-->> El argumento con índice 2 (Ruta de almacenamiento) se usa como ruta de almacenamiento para los dos archivos de respuesta, para este ejemplo se omitió dicho parametro
                            {
                                nameFileResponse = args[2];
                            }
                            if (nameFileResponse.ToString().Length == 0)
                            { //-->> En caso de omitir el parametro "Nombre del archivo de salida" recuperamos el valor del atributo UUID del CFDI para nombrar los archivos de respuesta
                                XmlDocument cfdiXML = new XmlDocument();
                                byte[] binary = Convert.FromBase64String(xmlElementCFDI.InnerText);
                                String strOriginal = System.Text.Encoding.UTF8.GetString(binary);
                                cfdiXML.LoadXml(strOriginal);
                                XmlElement xmlElementTimbre;
                                xmlElementTimbre = (XmlElement)cfdiXML.GetElementsByTagName("tfd:TimbreFiscalDigital").Item(0);
                                nameFileResponse = xmlElementTimbre.GetAttribute("UUID");
                            }
                            FileStream stream = new FileStream(pathFile + nameFileResponse + ".xml", FileMode.Create); //-->>Almacenamiento del CFDI
                            BinaryWriter writerBinary = new BinaryWriter(stream);
                            writerBinary.Write(Convert.FromBase64String(xmlElementCFDI.InnerText));
                            writerBinary.Close();
                            stream = new FileStream(pathFile + nameFileResponse + ".pdf", FileMode.Create); //-->>Almacenamiento de la represetnacion impresa
                            writerBinary = new BinaryWriter(stream);
                            writerBinary.Write(Convert.FromBase64String(xmlElementPDF.InnerText));
                            writerBinary.Close();
                            Console.WriteLine("Proceso de certificación finalizado exitosamente");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }                                                                          
                }
            }
            else {
                Console.WriteLine("Proporcione los parametros del timbrado(Ruta del layout, Ruta de almacenamiento y opcionalmente el nombre de los archivos de salida)");
            }
            //-->> Ejemplo de cacancelación de un CFDI
            requestCancelarCFDI requestCancelarCFDI = new requestCancelarCFDI(); //-->>Clase contenedora de los parametros de conexion del Web service de cancelación (vease classRequestCancelarCFDI.cs en el explorador de soluciones)
            requestCancelarCFDI.CancelarCFDI.UUID = "453F8434-0CAB-4445-9027-A481F7B8B117"; //-->> UUID de ejemplo
            //requestCancelarCFDI.CancelarCFDI.UUID = nameFileResponse;
            string responseSoapCancelacion = WSLayoutFM.requestCancelarCFDI(requestCancelarCFDI); //-->>Realizamos la petición pasando los parametros al método del WS
            if (responseSoapCancelacion.Length != 0) { //-->> Respuesta satisfactoria del WS
                Console.WriteLine(responseSoapCancelacion);
            }
            Console.ReadLine();
        }
    }
}
