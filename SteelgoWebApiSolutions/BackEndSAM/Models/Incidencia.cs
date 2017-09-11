using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Incidencia
    {
        public string FolioConfiguracionIncidenciaID { get; set; }
        public int FolioIncidenciaID { get; set; }
        public int ClasificacionID { get; set; }
        public int TipoIncidenciaID { get; set; }
        public string Version { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Respuesta { get; set; }
        public string MotivoCancelacion { get; set; }
        public string DetalleResolucion { get; set; }
        public string RegistradoPor { get; set; }
        public string FechaRegistro { get; set; }
        public string ResueltoPor { get; set; }
        public string FechaResolucion { get; set; }
        public string RespondidoPor { get; set; }
        public string FechaRespuesta { get; set; }
        public bool TieneIncidencia { get; set; }
        public List<ListaDocumentos> ArchivosIncidencia { get; set; }
        public List<ListaDocumentos> ArchivosResolver { get; set; }
        public List<ListaDocumentos> ArchivosResponder { get; set; }
        public List<ListaDocumentos> ArchivosCancelar { get; set; }
        public int ReferenciaID { get; set; }
        public string Estatus { get; set; }
        public string ValorReferencia { get; set; }
        public string NombreIncidencia { get; set; }
        public string FolioOriginalID { get; set; }
        public int IncidenciaInterna { get; set; }
    }

    public class IncicidenciaEnPaseSalida
    {
        public string IncidenciaID { get; set; }
        public string Descripcion { get; set; }
    }
}