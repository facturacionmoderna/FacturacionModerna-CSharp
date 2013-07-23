using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comprobante
{
    public class Resultados
    {
        public string code;
        public string message;
        public Boolean status;
        public string ncert;

        public Resultados()
        {
            this.code = "0";
            this.message = "";
            this.status = true;
        }
    }
}
