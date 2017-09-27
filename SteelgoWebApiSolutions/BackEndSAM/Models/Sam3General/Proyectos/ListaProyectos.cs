using System.Collections.Generic;

namespace BackEndSAM.Models.Sam3General
{
    public class Proyectos
    {
        public Proyectos() {
            ProyectoID = 0;
            Nombre = "";
            PatioID = 0;
            PrefijoOrdenTrabajo = "";
        }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int PatioID { get; set; }
        public string PrefijoOrdenTrabajo { get; set; }
    }

    public class IdOrdenTrabajo
    {
        public string OrdenTrabajo { get; set; }
        public List<Spool> ListaSpools { get; set; }
    }

    public class Spool
    {
        public string OrdenTrabajo { get; set; }
        public string Status { get; set; }

        public string Nombre { get; set; }//IDValido

        public int SpoolID { get; set; } //valor

        public string Proyecto { get; set; }

        public int ProyectoID { get; set; }
        public int HabilitadoHoldFecha { get; set; }


        public Spool()
        {
            Status = "";
            Nombre = "";
            SpoolID = 0;
            Proyecto = "";
            ProyectoID = 0;
            HabilitadoHoldFecha = 0;
        }
    }

    public class DetalleProyectoIngeneria
    {
        public DetalleProyectoIngeneria()
        {
            ProyectoID = 0;
            Nombre = "";
            ProyectoSpoolID = 0;
            ProyectoSoporteID = 0;
        }

        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int ProyectoSpoolID { get; set; }
        public int ProyectoSoporteID { get; set; }
    }

}