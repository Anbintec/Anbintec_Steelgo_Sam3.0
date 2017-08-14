using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ItemCode
    {
        public string ItemCodeID { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Familia { get; set; }
        public string Cedula { get; set; }
        public string TipoAcero { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal Cantidad { get; set; }
        public string Colada { get; set; }
        public string Certificado { get; set; }
        public string Material { get; set; }
        public string NumeroUnico { get; set; }
        public string ItemCodeSteelgoID { get; set; }
        public string Estatus { get; set; }
    }
}