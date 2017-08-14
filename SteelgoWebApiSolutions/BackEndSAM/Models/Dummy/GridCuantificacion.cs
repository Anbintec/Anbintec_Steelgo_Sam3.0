using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class GridCuantificacion
    {
        public PackingList PackingList { get; set; }
        public List<ListadoCuantificacion> ListadoCuantificacion { get; set; }
    }
}