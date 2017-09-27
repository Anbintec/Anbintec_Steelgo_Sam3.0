using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.Cedula
{
    public class DetalleCedula
    {
        public DetalleCedula()
        {
            CedulaID = 0;
            Nombre = "";
        }

        public int CedulaID { get; set; }
        public string Nombre { get; set; }
    }
}