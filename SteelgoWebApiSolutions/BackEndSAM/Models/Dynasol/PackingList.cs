using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class PackingList
    {
        public class RevisionClass
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int OrdenCompraID { get; set; }
            public int DetallePackingListID { get; set; }
            public int PackingListID { get; set; }
            public string Rev { get; set; }
            public string Descripcion { get; set; }
            public string MaterialNorma { get; set; }
            public float Diametro1 { get; set; }
            public float Diametro2 { get; set; }
            public string Schedule { get; set; }
            public string Rating { get; set; }
            public string PreparacionExtremos { get; set; }
            public float Cant { get; set; }
            public decimal PrecioUnidad { get; set; }
            public decimal Total { get; set; }
            public string Partida { get; set; }
            public float CantC { get; set; }
            public float CantG { get; set; }
            public float CantS { get; set; }
            public int ColadaID { get; set; }
            public string Colada { get; set; } 
            public float CantPL { get; set; }   
            public float CantDisponible { get; set; }
            public string ListadoPacking { get; set; }
            public bool Agregar { get; set; }
            public bool RowOk { get; set; }
        }
        public class Captura
        {
            public List<CapturaPL> Detalles { get; set; }
        }
        public class CapturaPL
        {
            public int Accion { get; set; }
            public int DetallePackingListID { get; set; }
            public int OrdenCompraID { get; set; }
            public int PackingListID { get; set; }
            public int ColadaID { get; set; }
            public float Cant { get; set; }         
         
        }
        
    }
}