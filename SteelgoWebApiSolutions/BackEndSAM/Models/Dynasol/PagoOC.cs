using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class PagoOC
    {
        public class Pago
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int OrdenCompraID { get; set; }
            public int ColadaID { get; set; }
            public string Partida { get; set; }
            public string ItemCode { get; set; }
            public string Descripcion { get; set; }
            public string MaterialNorma { get; set; }
            public float Diametro1 { get; set; }
            public float Diametro2 { get; set; }
            public string Colada { get; set; }
            public int CantOC { get; set; }
            public int CantC { get; set; }
            public int CantG { get; set; }
            public int CantS { get; set; }
            public int Pagado { get; set; }
            public int Deficit { get; set; }
            public bool ModificadoPorUsuario { get; set; }
            public bool RowOk { get; set; }
        }

        public class DataTablePago
        {
            public List<CapturaPago> Detalle { get; set; }
        }
        public class CapturaPago
        {
            public int ColadaID { get; set; }
            public int Pagado { get; set; }
            public bool ModificadoPorUsuario { get; set; }
        }
    }
}