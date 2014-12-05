using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSConecFM
{
    [Serializable]
    public class requestCancelarCFDI
    {
        public CancelarCFDI CancelarCFDI;
        public string UserID;
        public string UserPass;
        public string emisorRFC;
        public string urlCancelado;
        public string uuid;
        public string proxy_user;
        public string proxy_pass;
        public string proxy_url;
        public int proxy_port;

        public requestCancelarCFDI()
        {
            // Configuracion Inicial del a conexion cone l servios SOAP de cancelacion
            this.CancelarCFDI = new CancelarCFDI();
            //-->>Credenciales de acceso al servidor de timbrado de Facturación moderna            
            this.UserID = "UsuarioPruebasWS";
            this.UserPass = "b9ec2afa3361a59af4b4d102d3f704eabdf097d4";
            this.emisorRFC = "ESI920427886";
            this.urlCancelado = "https://t1demo.facturacionmoderna.com/timbrado/soap";
            this.proxy_url = "";
            this.proxy_pass = "";
            this.proxy_port = 80;
            this.proxy_user = "";
        }
    }

    [Serializable]
    public class CancelarCFDI
    {
        public String UUID; //-->>Folio fiscal del CFDI que se cancelará (UUID)

        public CancelarCFDI()
        {

        }
    }
}
