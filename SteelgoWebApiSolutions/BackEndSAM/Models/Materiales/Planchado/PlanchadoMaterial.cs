using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Materiales.Planchado
{
    public class PlanchadoMaterial
    {
        public class Captura
        {
            public string Detalle { get; set; }
        }
        
        public class DatosPlanchado
        {
            public string Nu { get; set; }
            public string Diametro1 { get; set; }
            public string Diametro2 { get; set; }
            public string TotalRecibido { get; set; }
            public string FisicoCondicionado { get; set; }
            public string FisicoDanado { get; set; }
            public string FisicoAprobado { get; set; }
            public string TotalFisico { get; set; }
            public string InventarioCongelado { get; set; }
            public string DocumentalAprobado { get; set; }
            public string DocumentalRechazado { get; set; }
            public string NumeroColada { get; set; }
            public string NumeroMtr { get; set; }
            public string Fabricante { get; set; }
            public string NumeroUnicoCliente { get; set; }
            public string Rack { get; set; }
            public bool AplicaColada { get; set; }
            public bool AplicaMtr { get; set; }
            public bool AplicaInventario { get; set; }
            public bool AplicaEstatusFisico { get; set; }
            public bool AplicaEstatusDocumental { get; set; }
            public bool AplicaFabricante { get; set; }
            public bool AplicaNumeroUnicoCliente { get; set; }
            public bool AplicaRack { get; set; }
        }
    }
}