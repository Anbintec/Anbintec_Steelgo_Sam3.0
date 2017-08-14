using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class DocumentoPosteado
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extencion { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
        public Guid DocGuid { get; set; }
        public Nullable<int> FolioAvisoLlegadaID { get; set; }
        public int UserId { get; set; }
        public int TipoArchivoID { get; set; }
        public string NumeroPermisoAduana { get; set; }
        public Nullable<int> PermisoAduanaID { get; set; }
        public int FolioAvisoEntradaID { get; set; }
        public string Descripcion { get; set; }
        public string TipoArchivoPaseSalida { get; set; }
        public Nullable<int> IncidenciaID { get; set; }
    }

    public class EstatusFolio
    {
        public int FolioAvisoLlegada { get; set; }
        public string NumeroPermiso { get; set; }
    }
}