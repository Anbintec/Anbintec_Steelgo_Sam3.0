using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.IntermedioAcabado
{
    public class Zona
    {
        public int ZonaID { get; set; }
        public string Nombre { get; set; }

        public Zona()
        {
            ZonaID = 0;
            Nombre = "";
        }
    }
}