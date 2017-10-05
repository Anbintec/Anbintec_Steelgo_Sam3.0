using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.DescargaCarro
{
    public class DescargaCarro
    {
    }

    public class DetalleCarro
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public string NombreSpool { get; set; }
        public string SistemaPintura { get; set; }
        public string Color { get; set; }
        public decimal? M2 { get; set; }
        public int? CuadranteID { get; set; }
        public string NombreCuadrante { get; set; }
        public bool Modificado { get; set; }
    }

    public class Captura
    {
        public List<DetalleGuardado> Detalles { get; set; }
    }

    public class DetalleGuardado
    {
        public int SpoolID { get; set; }
        public int CuadranteID { get; set; }
        public string NombreCuadrante { get; set; }
        
    }
}