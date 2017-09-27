using System.Collections.Generic;

namespace BackEndSAM.Models.Calidad.OkDocumental.OkDocumental
{
    public class OkDocumental
    {
        public class ElementosOK
        {
            public ElementosOK()
            {
                SpoolWorkStatusID = 0;
                NombreSpool = "";
                Cuadrante = "";
                Prioridad = 0;
                ProyectoID = 0;
                SpoolID = 0;
                OrdenTrabajoSpoolID = 0;
                OkDocumental = false;                         
            }

            public int SpoolWorkStatusID { get; set; }
            public string NombreSpool { get; set; }
            public string Cuadrante { get; set; }
            public int Prioridad { get; set; }
            public int ProyectoID { get; set; }
            public int SpoolID { get; set; }
            public int OrdenTrabajoSpoolID { get; set; }                        
            public bool OkDocumental { get; set; }
            public bool ModificadoPorUsuario { get; set; }
            public string Detalle { get; set; }
        }        

        public class Datos
        {
            public List<ListaElementos> Detalle { get; set; }
        }

        public class CapturaOKMasiva
        {
            public string Detalle { get; set; }
        }

        public class ListaElementos
        {
            public int SpoolID { get; set; }
            public int OrdenTrabajoSpoolID { get; set; }
            public bool OkDocumental { get; set; }
            public bool ModificadoPorUsuario { get; set; }

        }

        public class ListaElementosMasivo
        {
            public List<ElementosOKMasivo> Elementos;
        }

        public class ElementosOKMasivo
        {
            public string NumeroControl { get; set; }
            public string OK { get; set; }
            public string OE { get; set; }
        }

        public class ElementosMasivoResult
        {
            public string NumeroControl { get; set; }
            public string OK { get; set; }
            public string Actualizado { get; set; }
        }      
    }
}