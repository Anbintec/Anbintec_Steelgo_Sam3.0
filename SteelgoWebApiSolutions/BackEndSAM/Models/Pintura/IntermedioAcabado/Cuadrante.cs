using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.IntermedioAcabado
{
    public class Cuadrante
    {
        public int? CuadranteID { get; set; }
        public string Nombre { get; set; }

        public Cuadrante()
        {
            CuadranteID = 0;
            Nombre = "";
        }
    }
}