using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Almacenaje
    {
        public List<ListaCombos> ListadoItemCode { get; set; }
        public List<ListadoAlmacenaje> ListadoAlmacenaje { get; set; }
    }

    public class ListadoAlmacenaje 
    {
        public string ItemCodeID { get; set; }
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string Rack { get; set; }
    }

    public class ListaNumerosUnicos {
        public string NumeroUnicoID { get; set; }
        public string Rack { get; set; }
    }

    public class Items 
    {
        public string OrdenAlmacenajeID { get; set; }
        public List<ListaNumerosUnicos> NumerosUnicos { get; set; }

        public Items()
        {
            NumerosUnicos = new List<ListaNumerosUnicos>();
        }
    }

}