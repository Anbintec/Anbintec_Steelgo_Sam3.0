using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.OpcionValidacion
{
    public class DetalleOpcionValidacion
    {
        public DetalleOpcionValidacion()
        {
            OpcionValidacionID = 0;
            Descripcion = "";
        }

        public int OpcionValidacionID { get; set; }
        public string Descripcion { get; set; }
    }
}