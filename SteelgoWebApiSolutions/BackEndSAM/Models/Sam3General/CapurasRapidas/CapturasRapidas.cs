using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.CapurasRapidas
{
    public class CapturasRapidas
    {
        public class IdOrdenTrabajo
        {
            public string OrdenTrabajo { get; set; }
            public List<IDS> idStatus { get; set; }
        }
    }
    public class IDS
    {
        public string Status { get; set; }

        public string IDValido { get; set; }

        public int Valor { get; set; }

        public string Proyecto { get; set; }

        public int ProyectoID { get; set; }

        public IDS()
        {
            Status = "";
            IDValido = "";
            Valor = 0;
            Proyecto = "";
            ProyectoID = 0;
        }
    }
    public class IdOrdenTrabajo
    {
        public string OrdenTrabajo { get; set; }
        public List<IDS> idStatus { get; set; }
    }
    
}