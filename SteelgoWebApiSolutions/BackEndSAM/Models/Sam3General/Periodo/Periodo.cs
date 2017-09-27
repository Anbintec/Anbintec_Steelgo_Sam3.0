using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.Periodo
{
    public class DetallePeriodo
    {
        public DetallePeriodo()
        {
            PeriodoID = 0;
            Periodo = "";
            Minuendo = "";
            Sustraendo = 0;
        }
        public int PeriodoID { get; set; }
        public string Periodo { get; set; }
        public string Minuendo { get; set; }
        public int Sustraendo { get; set; }
    }

    public class RangoFechas
    {
        public RangoFechas(string fechaInicio, string fechaFin)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}