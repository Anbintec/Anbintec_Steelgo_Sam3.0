using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class InfoFolioCuantificacion
    {
        public int FolioCuantificacionID { get; set; }
        public int FolioAvisoEntradaID { get; set; }
        public int ProyectoID { get; set; }
        public string PackingList { get; set; }
        public string Factura { get; set; }
        public string OrdenDeCompra { get; set; }
        public TipoPackingList TipoPackingList { get; set; }
        public TipoUso TipoUso { get; set; }
        public string TipoUsoNombre { get; set; }
        public string Estatus { get; set; }
        public int FolioLlegadaHijo { get; set; }
        public string FolioConfiguracionCuantificacionID { get; set; }
        public bool MostrarComboPackingList { get; set; }
        public string NombreFolioAvisoLlegada { get; set; }
        public string NombreFolioCuantificacion { get; set; }
        public int FolioAvisoLlegadaID { get; set; }
        public int ConsecutivoFolioCuanificacion { get; set; }
        public int ConsecutivoFolioLlegada { get; set; }
        public string NombreProyecto { get; set; }
    }

  
}