using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Entrega
    {
        public string NoPickingTicket { get; set; }
        public string NoEmpleadoEntrega { get; set; }
        public string NoEmpleadoRecibe { get; set; }
        public string FechaEntrega { get; set; }
        public int EntregaID { get; set; }
    }

    public class ListadoEntrega
    {
        public List<Entrega> Entregas { get; set; }
        public ListadoEntrega()
        {
            Entregas = new List<Entrega>();
        }
    }
}