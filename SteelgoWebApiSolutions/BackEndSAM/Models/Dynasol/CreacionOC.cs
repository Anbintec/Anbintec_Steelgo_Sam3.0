using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class CreacionOC
    {

        public class MonedaClass
        {
            public MonedaClass()
            {
                MonedaID = 0;
                Moneda = "";
                Descripcion = "";
            }

            public int MonedaID { get; set; }
            public string Moneda { get; set; }
            public string Descripcion { get; set; }
        }
        public class ClienteClass
        {
            public ClienteClass()
            {
                ClienteID = 0;
                Cliente = "";
            }

            public int ClienteID { get; set; }
            public string Cliente { get; set; }
        }

        public class DataTableCreacion
        {
            public List<CapturaCreacion> Detalle { get; set; }
        }

        public class CapturaCreacion
        {
            public int Consecutivo { get; set; }
            public int OrdenCompraID { get; set; }
            public string Revision { get; set; }
            public string Descripcion { get; set; }
            public string MaterialNorma { get; set; }
            public float Diametro1 { get; set; }
            public float Diametro2 { get; set; }
            public string Shedule { get; set; }
            public string Rating { get; set; }
            public string PrepExt { get; set; }
            public float Cantidad { get; set; }
            public float PrecioCompra { get; set; }
            public float PrecioVenta { get; set; }
        }
    }
}