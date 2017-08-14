using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndSAM.Models
{
    public class FiltrosJson
    {
        public string FolioAvisoEntradaID { get; set; }
        public string FolioAvisoLlegadaID { get; set; }
        public string PatioID { get; set; }
        public string ClienteID { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string token { get; set; }
        public bool Creados { get; set; }
        public bool SinPermiso { get; set; }
        public bool SinAutorizacion { get; set; }
        public bool Completos { get; set; }
        public string TipoListado { get; set; }
        public string ParametroBusqueda { get; set; }
        public bool PorLlegar { get; set; }
        public bool PorDescargar { get; set; }
        public bool PorSalir { get; set; }
        public string ProyectoID { get; set; }
        public string PackingListID { get; set; }
        public string TipoMaterialID { get; set; }
        public string ItemCodeID { get; set; }
        public string FolioCuantificacionID { get; set; }
        public string PaginaID { get; set; }
        public string Idioma { get; set; }
        public string UnidadDeMedida { get; set; }
        public int OrdenRecepcionID { get; set; }
    }
}