using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoCuantificacion
    {
        public string ItemCode { get; set; }
        public string BultoID { get; set; }
        public string Descripcion { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string Familia { get; set; }
        public int FamiliaMaterialID { get; set; }
        public string Cedula { get; set; }
        public string TipoAcero { get; set; }
        public string Colada { get; set; }
        public int? Cantidad { get; set; }
        public int? MM { get; set; }
        public string Detallar { get; set; }
        public bool TieneNU { get; set; }
        public bool TieneError { get; set; }
    }
}