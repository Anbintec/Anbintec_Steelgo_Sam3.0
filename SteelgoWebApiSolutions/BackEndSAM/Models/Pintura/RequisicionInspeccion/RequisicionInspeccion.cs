using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.RequisicionInspeccion
{
    public class RequisicionInspeccion
    {        
    }
    
    
    public class Datos
    {
        public List<InfoGrid> Detalle { get; set; }
    }
    public class InfoGrid
    {
        public int Accion { get; set; }
        public int RequisicionInspeccionID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }        
        public string SistemaPintura { get; set; }
        public string Color { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaInspeccion { get; set; }
        public string Resultado { get; set; }
        public bool ModificadoPorUsuario { get; set; }
    }
    public class Spool
    {        
        public int SpoolID { get; set; }

        public string OrdenTrabajo { get; set; }

        public string ID { get; set; }

        public int OrdenTrabajoSpoolID { get; set; }
        

        public Spool()
        {
            SpoolID = 0;
            OrdenTrabajo = "";
            ID = "";
            OrdenTrabajoSpoolID = 0;
        }
    }
    public class IdOrdenTrabajoPintura
    {
        public string OrdenTrabajo { get; set; }
        public List<Spool> idStatus { get; set; }
    }
    
    public class GetSpools
    {
        public List<Spools> Detalle { get; set; }
    }
    public class Spools
    {
        public string NumeroControl { get; set; }
    }
}