using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoOrdeRecepcion
    {
        public string FechaOrdenRecepcion { get; set; }
        public string OrdenRecepcion { get; set; }
        public string OrdenRecepcionID { get; set; }
        public List<ElementoItemCode> Detalle { get; set; }

        public ListadoOrdeRecepcion()
        {
            Detalle = new List<ElementoItemCode>();
        }
    }

    public class ElementoItemCode 
    {
        public string FolioConfiguracion { get; set; }
        public string AvisoEntrada { get; set; }
        public string Itemcode { get; set; }
        public int Cantidad { get; set; }
    }
}