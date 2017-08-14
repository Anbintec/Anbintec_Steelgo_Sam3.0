using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadosPreDespacho
    {
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public string Cedula { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadPreDespachada { get; set; }
        public int CantidadDespachada { get; set; }
        public int CantidadEntregada { get; set; }
        public int CantidadPorPreDespachar { get; set; }
        public int CantidadPorDespachar { get; set; }
        public int CantidadPorEntregar { get; set; }
    }
}