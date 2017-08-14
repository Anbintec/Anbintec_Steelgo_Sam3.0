using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class PreDespacho
    {
        public string NumeroControlID { get; set; }
        public string NumeroControl { get; set; }
        public string ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string Etiqueta { get; set; }
        public List<NumerosUnicos> NumerosUnicos { get; set; }
        public int MaterialSpoolID { get; set; }
        public string PreDespachoID { get; set; }

        public PreDespacho()
        {
            NumerosUnicos = new List<NumerosUnicos>();
        }
    }

    public class DatosPredespacho
    {
        public List<PreDespachoItems> lista { get; set; }

        public DatosPredespacho()
        {
            lista = new List<PreDespachoItems>();
        }
    }

    public class PreDespachoItems
    {
        public string NumeroControl { get; set; }
        public string ItemCode { get; set; }
        public string NumeroUnico { get; set; }
        public string Etiqueta { get; set; }
        public string PreDespachoID { get; set; }
    }
}