using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.TipoJunta
{
    public class DetalleTipoJunta
    {
        public DetalleTipoJunta()
        {
            TipoJuntaID = 0;
            Nombre = "";
        }

        public int TipoJuntaID { get; set; }
        public string Nombre { get; set; }
    }
}