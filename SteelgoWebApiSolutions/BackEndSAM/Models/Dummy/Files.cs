using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Files
    {
        public int id { get; set; }
        public string Archivo { get; set; }
        public string Extension { get; set; }
        public string Descripcion { get; set; }
        public string TipoArchivo { get; set; }
        public string status { get; set; }
    }
}