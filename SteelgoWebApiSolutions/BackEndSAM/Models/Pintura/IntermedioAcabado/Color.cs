using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.IntermedioAcabado
{
    public class Color
    {
        public int ColorID { get; set; }
        public string Nombre { get; set; }
        public int SistemaPinturaColorID { get; set; }
        public Color()
        {
            ColorID = 0;
            Nombre = "";
        }
    }
}