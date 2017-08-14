using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListaDocumentos
    {
        public string DocumentoID { get; set; }
        public string Nombre { get; set; }
        public string Extencion { get; set; }
        public string TipoArchivo { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> IncidenciaID { get; set; }
        public string NombreIncidencia { get; set; }
    }

    public class DocumentoPermisoAduana
    {
        public string PermisoID { get; set; }
        public string DocumentoID { get; set; }
        public string NumeroPermiso { get; set; }
        public bool PermisoAutorizado { get; set; }
        public string Url { get; set; }
        public string Nombre { get; set; }
        public string Extencion { get; set; }
        public string TipoArchivo { get; set; }
    }

    public class ListaDocumentosLlegadaMaterial
    {
        public string DocumentoID { get; set; }
        public string Nombre { get; set; }
        public string Extencion { get; set; }
        public string TipoArchivo { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
    }

    public class ListaDocumentosCatalogos
    {
        public string DocumentoID { get; set; }
        public string CatalogoID { get; set; }
        public string CatalogoNombre { get; set; }
        public string ElementoCatalogoID { get; set; }
        public string Nombre { get; set; }
        public string Extencion { get; set; }
        public string TipoArchivoID { get; set; }
        public string TipoArchivo { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
    }
}