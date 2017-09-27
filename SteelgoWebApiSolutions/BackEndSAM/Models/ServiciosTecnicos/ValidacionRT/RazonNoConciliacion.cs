using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ValidacionRT
{
    public class RazonNoConciliacion
    {
        public int? RazonNoConciliacionID { get; set; }
        public string Descripcion { get; set; }

        public RazonNoConciliacion()
        {
            RazonNoConciliacionID = null;
            Descripcion = "";
        }
    }
}