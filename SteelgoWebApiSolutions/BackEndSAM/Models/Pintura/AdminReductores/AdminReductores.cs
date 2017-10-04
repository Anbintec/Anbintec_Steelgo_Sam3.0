using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.AdminReductores
{
    public class DetalleGrid
    {
        public int? ReductorID { get; set; }
        public string Reductor { get; set; }
        public string Lote { get; set; }
        public int? Cantidad { get; set; }
        public string Unidad { get; set; }
        public bool RowOk { get; set; }
        public int Accion { get; set; }
        public int AdminReductoresID { get; set; }
    }

    public class Reductores
    {
        public int ReductorID { get; set; }
        public string Reductor { get; set; }
        public string Unidad { get; set; }
        public Reductores()
        {
            ReductorID = 0;
            Reductor = "";
            Unidad = "";
        }
    }

    public class GuardarGrid
    {
        public int ReductorID { get; set; }
        public string Lote { get; set; }
        public int Cantidad { get; set; }
        public int Accion { get; set; }
        public int AdminReductoresID { get; set; }
    }

    public class Captura
    {
        public List<GuardarGrid> Detalles { get; set; }
    }
}