using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.Cuadrante
{
    public class UbicacionCuadrante
    {
        public UbicacionCuadrante()
        {
            CuadranteID = 0;
            CuadranteSam2ID = 0;
            Nombre = "";
            PatioID = 0;
            ProyectoID = 0;
            ZonaID = 0;
            CuadranteAnterior = 0;
        }
        public int? CuadranteID { get; set; }
        public int? CuadranteSam2ID { get; set; }
        public string Nombre { get; set; }
        public int? PatioID { get; set; }
        public int? ProyectoID { get; set; }
        public int? ZonaID { get; set; }
        public int? CuadranteAnterior { get; set; }
    }
}