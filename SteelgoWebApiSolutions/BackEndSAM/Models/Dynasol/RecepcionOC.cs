using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class RecepcionOC
    {
        public class Recepcion
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int OrdenCompraID { get; set; }
            public int ColadaID { get; set; }
            public string ItemCode { get; set; }
            public string Descripcion { get; set; }            
            public string MaterialNorma { get; set; }
            public float Diametro1 { get; set; }
            public float Diametro2 { get; set; }
            public string Schedule { get; set; }
            public string Rating { get; set; }
            public string PrepExt { get; set; }
            public string Partida { get; set; }
            public string Colada { get; set; }
            public int CantS { get; set; }
            public int CantAP { get; set; }            
            public int CantRDO { get; set; }
            public int CantCOND { get; set; }
            public bool ModificadoPorUsuario { get; set; }
            public bool RowOk { get; set; }
        }

        public class CapturaRecepcion
        {
            public List<CapturaDetalle> Detalle { get; set; }
        }
        public class CapturaDetalle
        {
            public int ColadaID { get; set; }
            public int CantAP { get; set; }
            public int CantRDO { get; set; }
            public int CantCOND { get; set; }
            public bool ModificadoPorUsuario { get; set; }
        }
    }
}