using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FolioLlegadaCuantificacion
    {
        public int FolioCuantificacionID { get; set; }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public string FolioConfiguracionCuantificacionID { get; set; }
    }
}