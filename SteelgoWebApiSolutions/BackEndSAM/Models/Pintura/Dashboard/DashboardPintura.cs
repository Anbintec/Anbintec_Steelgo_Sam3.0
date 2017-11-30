using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.Dashboard
{

    public class CabeceraDashboar {

        public int Spools1 { get; set; }
        public decimal M21 { get; set; }
        public decimal  Toneladas1 { get; set; }

        public int Spools2 { get; set; }
        public decimal M22 { get; set; }
        public decimal Toneladas2 { get; set; }

        public int Spools3 { get; set; }
        public decimal M23 { get; set; }
        public decimal Toneladas3 { get; set; }

        public int Spools4 { get; set; }
        public decimal M24 { get; set; }
        public decimal Toneladas4 { get; set; }

        public int Spools5 { get; set; }
        public decimal M25 { get; set; }
        public decimal Toneladas5 { get; set; }

        public int Spools6 { get; set; }
        public decimal M26 { get; set; }
        public decimal Toneladas6 { get; set; }

        public int Spools7 { get; set; }
        public decimal M27 { get; set; }
        public decimal Toneladas7 { get; set; }

        public int Spools8 { get; set; }
        public decimal M28 { get; set; }
        public decimal Toneladas8 { get; set; }

        public int Spools9 { get; set; }
        public decimal M29 { get; set; }
        public decimal Toneladas9 { get; set; }

        public int Spools10 { get; set; }
        public decimal M210 { get; set; }
        public decimal Toneladas10 { get; set; }

        public int Spools11 { get; set; }
        public decimal M211 { get; set; }
        public decimal Toneladas11 { get; set; }

        public int Spools12 { get; set; }
        public decimal M212 { get; set; }
        public decimal Toneladas12 { get; set; }

    }

    public  class DetalleDashboardPintura
    {
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public string Proyecto { get; set; }
        public int Prioridad { get; set; }
        public string Cuadrante { get; set; }
        public decimal M2 { get; set; }
        public decimal Peso { get; set; }
        public int CantidadSpools { get; set; }
        public string Carro { get; set; }
        public string Color { get; set; }
        public string SistemaPintura { get; set; }
    }


}