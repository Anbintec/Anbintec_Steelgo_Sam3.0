using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class DocumentosEnCatalogos
    {
        public string CatalogoID { get; set; }
        public string ElementoCatalogoID { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
        public Guid DocGuid { get; set; }
        public int UserId { get; set; }
        public int TipoArchivoID { get; set; }
        public string Descripcion { get; set; }
    }
}