using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ValidacionRT
{
    public class ResultadoConciliacion
    {
        public int? ResultadoConciliacionID { get; set; }
        public string Descripcion { get; set; }

        public ResultadoConciliacion()
        {
            ResultadoConciliacionID = null;
            Descripcion = "";
        }
    }
}