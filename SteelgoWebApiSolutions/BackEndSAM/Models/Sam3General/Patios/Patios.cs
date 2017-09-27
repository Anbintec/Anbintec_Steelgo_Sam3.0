using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.Patios
{
    public class PatioObject
    {
        public PatioObject()
        {
            PatioID = 0;
            Nombre = "";
        }
        public int PatioID { get; set; }
        public string Nombre { get; set; }
    }
}