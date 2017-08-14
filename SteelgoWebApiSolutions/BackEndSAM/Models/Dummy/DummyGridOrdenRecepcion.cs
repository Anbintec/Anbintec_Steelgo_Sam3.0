using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class DummyGridOrdenRecepcion
    {
        public int AvisoEntradaID { get; set; }
        public List<DummyMaterial> Tubos { get; set; }
        public List<DummyMaterial> Accesorios { get; set; }
    }

    public class DummyMaterial
    {
        public int AvisoEntradaID { get; set; }
        public int ItemCodeID { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public int Cantidad { get; set; }
        public string TipoMaterial { get; set; }
    }
}