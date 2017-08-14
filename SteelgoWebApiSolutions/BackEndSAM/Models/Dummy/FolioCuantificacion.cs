using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FolioCuantificacion
    {
        public int FolioCuantificacionID { get; set; }
        public int FolioAvisoEntradaID { get; set; }
        public int ProyectoID { get; set; }
        public string PackingList { get; set; }
        public int TipoUsoID { get; set; }
        public string Estatus { get; set; }
        public int FolioLlegadaHijo { get; set; }
    }

    public class InfoFolioAvisoEntrada
    { 
        public List<Proyecto> Proyecto { get; set; }
        public List<FolioLlegada1> FolioLlegada { get; set; }
        public int FolioLlegadaHijo { get; set; }
        public TipoPackingList TipoPackingList { get; set; }
        public TipoUso TipoUso { get; set; }
        public string PackingList { get; set; }
        public string OrdenDeCompra { get; set; }
        public string Factura { get; set; }
    }

    public class FolioLlegada1
    {
        public int FolioCuantificacionID { get; set; }
        public int FolioAvisoEntradaID { get; set; }
        public string FolioConfiguracionCuantificacionID { get; set; }
        public string NombreFolioAvisoLlegada { get; set; }
        public string NombreFolioCuantificacion { get; set; }
        public int FolioAvisoLlegadaID { get; set; }
        public int ConsecutivoFolioCuanificacion { get; set; }
        public int ConsecutivoFolioLlegada { get; set; }
        public int TipoUsoID { get; set; }
        public string TipoUso { get; set; }
    }

    public class FolioEntradaYLlegada
    {
        public int FolioAvisoEntradaID;
        public int? FolioAvisoLlegadaID;
    }

    public class TipoPackingList
    {
        public string id { get; set; }
        public string Nombre { get; set; }
        public bool MostrarTipoPackingList { get; set; }
    }

    public class TipoUso
    {
        public string id { get; set; }
        public string Nombre { get; set; }
    }

    public class AvisoLlegada_Proyecto
    {
        public string ProyectoID { get; set; }
        public string Nombre { get; set; }
    }

    public class InfoFolioLlegadaMaterial
    {
        public int ProyectoID { get; set; }
        public int FolioLlegadaHijo { get; set; }
        public TipoPackingList TipoPackingList { get; set; }
        public TipoUso TipoUso { get; set; }
        public string PackingList { get; set; }
        public string Estatus { get; set; }
    }

}