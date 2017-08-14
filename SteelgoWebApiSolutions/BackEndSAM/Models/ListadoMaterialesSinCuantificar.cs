using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoMaterialesSinCuantificar
    {
        public string FolioConfiguracion { get; set; }
        public string FolioAvisoEntrada { get; set; }
        public string FechaDescarga { get; set; }
        public string Cliente { get; set; }
    }

    /// <summary>
    /// Folios de cuantificacion que no han sido cerrados
    /// </summary>
    public class ListadoPLporCuantificar
    {
        public string Proyecto { get; set; }
        public string FolioAvisoEntrada { get; set; }
        public string FechaDescarga { get; set; }
        public string FechaCreacionPackingList { get; set; }
        public string PackingList { get; set; }
        public string FolioCuantificacionID { get; set; }
        public string FolioConfiguracion { get; set; }
        public string NombreFolioCuantificacion { get; set; }
    }

    /// <summary>
    /// ItemCodes que no tienen un relacion con un ItemCodeSteelgo
    /// </summary>
    public class ListadoMTLSinICS
    {
        public string Proyecto { get; set; }
        public string FechaCreacionPackingList { get; set; }
        public string PackingList { get; set; }
        public string CantidadTotalItems { get; set; }
        public string CantidadSinICS { get; set; }
        public string FolioCuantificacionID { get; set; }
        public string FolioAvisoLlegadaID { get; set; }
    }

    /// <summary>
    /// Numeros unicos con Orden de recepcion pero sin complemento de recepcion
    /// </summary>
    public class ListadoNUPorRecepcionar
    {
        public string FechaOrdenRecepcion { get; set; }
        public string OrdenRecepcion { get; set; }
        public string CantidadNUEnOrdenRecepcion { get; set; }
        public string CantidadNUSinComplemento { get; set; }
        public string Folio { get; set; }
        public int OrdenRecepcionID { get; set; }
    }

    /// <summary>
    /// Numeros Unicos con orden de almacenaje pero que no se ha registrado el campo Rack
    /// </summary>
    public class ListadoNUSinAlmacenar
    {
        public string FechaOrdenRecepcion { get; set; }
        public string OrdenRecepcion { get; set; }
        public string CantidadNUEnOrdenRecepcion { get; set; }
        public string CantidadNUporAlmacenar { get; set; }
        public int OrdenRecepcionID { get; set; }
    }

    public class ListadoIncidenciasAbiertas
    {
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string CantidadIncidencias { get; set; }
    }

    public class ListadoPkList
    {
        public string FechaFolioAvisoEntrada { get; set; }
        public string FolioEntrada { get; set; }
        public string PackingList { get; set; }
        public string FolioCuantificacion { get; set; }
        public string TipoPackingList { get; set; }
        public string TipoUso { get; set; }
        public string Estatus { get; set; }
        public string FolioConfiguracion { get; set; }
        public string FolioConfiguracionCuantificacionID { get; set; }
    }
}