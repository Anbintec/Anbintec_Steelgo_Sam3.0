using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoGenerarOrdenRecepcion
    {
        public string FolioConfiguracion { get; set; }
        public string FolioConfiguracionOrdenRecepcion { get; set; }
        public string AvisoEntradaID { get; set; }
        public List<ElementoItemCodeGenerarOrden> Tubos { get; set; }
        public List<ElementoItemCodeGenerarOrden> Accesorios { get; set; }

        public ListadoGenerarOrdenRecepcion()
        {
            Tubos = new List<ElementoItemCodeGenerarOrden>();
            Accesorios = new List<ElementoItemCodeGenerarOrden>();
        }
    }

    public class ElementoItemCodeGenerarOrden
    {
        public string ItemCodeID { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Cantidad { get; set; }
        public string TipoMaterial { get; set; }
        public string FolioAvisoLlegadaId { get; set; }
        public string RelFCId { get; set; }
        public string RelBID { get; set; }
        public int ItemCodeIDOriginal { get; set; } 
    }
}