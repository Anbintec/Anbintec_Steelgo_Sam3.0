using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ComboNumeroControl
    {
        public string NumeroControlID { get; set; }
        public string NumeroControl { get; set; }
    }


    public class Despacho
    {
        public List<DespachoItems> ListaDespachos { get; set; }

        public Despacho()
        {
            ListaDespachos = new List<DespachoItems>();
        }
    }
    public class DespachoItems
    {
        public string NumeroControl { get; set; }
        public string ItemCode { get; set; }
        public string NumeroUnico { get; set; }
        public string Etiqueta { get; set; }
        public string Baston { get; set; }
        public string ProyectoID { get; set; }
        public string DespachoID { get; set; }
    }
}