using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class CobroOC
    {
        public class Cobro
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
            public int Cobrado { get; set; }
            public int Deficit { get; set; }
            public bool ModificadoPorUsuario { get; set; }
            public bool RowOk { get; set; }
        }

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
            public int ClienteID { get; set; }
            public string Cliente { get; set; }
        }

        public class DataTableCobro
        {
            public List<CapturaCobro> Detalle { get; set; }
        }
        public class CapturaCobro
        {
            public int ColadaID { get; set; }            
            public int Cobrado { get; set; }
            public bool ModificadoPorUsuario { get; set; }
        }
    }
}