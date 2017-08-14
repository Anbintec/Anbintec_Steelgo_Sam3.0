using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class GridGeneracionOrdenAlmacenaje
    {
        public int ItemCode { get; set; }
        public string Descripcion { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public List<int> NumeroUnico { get; set; }
        public int? Cantidad { get; set; }
        public string PackingList { get; set; }
    }
}