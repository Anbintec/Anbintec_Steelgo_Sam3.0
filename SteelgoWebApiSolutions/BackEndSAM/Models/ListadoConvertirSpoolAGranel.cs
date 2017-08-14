using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoConvertirSpoolAGranel
    {
        public string Spool { get; set; }
        public string Junta { get; set; }
        public string TipoJunta { get; set; }
        public string Status { get; set; }
        public string FabArea { get; set; }
    }


    public class ItemsSpoolAGranel
    {
        public List<ListadoConvertirSpoolAGranel> Items { get; set; }

        public ItemsSpoolAGranel()
        {
            Items = new List<ListadoConvertirSpoolAGranel>();
        }
    }
}