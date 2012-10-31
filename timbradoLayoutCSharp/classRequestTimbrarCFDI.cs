using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timbradoLayoutCSharp
{
    [Serializable]
    public class requestTimbrarCFDI
    {
        public string text2CFDI;
        public string UserID;
        public string UserPass;
        public string emisorRFC;

        public requestTimbrarCFDI()
        {
            //-->>Credenciales de acceso al WS
            this.UserID = "UsuarioPruebasWS";
            this.UserPass = "b9ec2afa3361a59af4b4d102d3f704eabdf097d4";
            this.emisorRFC = "ESI920427886";
        }
    }
}
