using BackEndSAM.Models.ServiciosTecnicos.AsignarRequisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.DashboardPND
{
    public class DetalleDashboard
    {
        public int RequisicionID { get; set; }
        public string Requisicion { get; set; }
        public int TipoPruebaID { get; set; }
        public string TipoPrueba { get; set; }
        public string Fecha { get; set; }
        public int TurnoLaboralID { get; set; }
        public string Turno { get; set; }
        public int ProveedorID { get; set; }
        public string Proveedor { get; set; }
        public int EquipoID { get; set; }
        public string Equipo { get; set; }
        public int EstatusRequisicion { get; set; }
        public int NumeroElementos { get; set; }
        public string Url { get; set; }
        public List<ElementosRequisicion> listaElementosRequisicion { get; set; }
        
    }

    

}