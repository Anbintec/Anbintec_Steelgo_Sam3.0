using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Coladas
    {
        public int ColadaID { get; set; }
        public string Nombre { get; set; }
    }

    public class ColadaJson
    {
        public int ItemCodeID { get; set; }
        public int FabricanteID { get; set; }
        public int AceroID { get; set; }
        public int ProyectoID { get; set; }
        public string NumeroColada { get; set; }
        public string NumeroCertificado { get; set; }
        public bool HoldCalidad { get; set; }
    }
}