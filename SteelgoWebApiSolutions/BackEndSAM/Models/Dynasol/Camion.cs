using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class Camion
    {
        public class CamionClass
        {
            public int CamionID { get; set; }
            public string NombreCamion { get; set; }

            public CamionClass()
            {
                CamionID = 0;
                NombreCamion = "";
            }
        }

        public class Captura
        {
            public List<CapturaCamion> Detalles { get; set; }
        }
        public class CapturaCamion
        {
            public int Accion { get; set; }
            public int DetalleCamionID { get; set; }
            public int OrdenCompraID { get; set; }
            public int CamionID { get; set; }
            public int ColadaID { get; set; }
            public float Cantidad { get; set; }
        }

        public class DetalleCamion
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int OrdenCompraID { get; set; }
            public int DetalleCamionID { get; set; }
            public int CamionID { get; set; }
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
            public float CantCamion { get; set; }
            public float CantDisponible { get; set; }
            public string ListadoCamion { get; set; }
            public bool Agregar { get; set; }
            public bool RowOk { get; set; }
        }
    }
}