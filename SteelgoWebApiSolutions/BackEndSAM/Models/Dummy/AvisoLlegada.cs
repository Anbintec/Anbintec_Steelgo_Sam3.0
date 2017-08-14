using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class AvisoLlegada
    {
        public int FolioAvisoLlegadaID { get; set; }
        public List<Proyecto> Proyectos { get; set; }
        public string Factura { get; set; }
        public Transportista Transportistas { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public Proveedor Proveedores { get; set; }
        public List<PlanaEntradaMat> Planas { get; set; }
        public string OrdenCompra { get; set; }
        public Patio Patios { get; set; }
        public Chofer Choferes { get; set; }
        public List<Files> Archivos { get; set; }
        public PermisoAduana PermisoAduana { get; set; }
        public PaseSalida PaseSalida { get; set; }
    }
}