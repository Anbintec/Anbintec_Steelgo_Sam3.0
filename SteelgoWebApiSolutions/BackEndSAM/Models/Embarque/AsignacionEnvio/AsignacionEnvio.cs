using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.AsignacionEnvio
{
   
    public class AsignacionEnvio
    {
        public class EmbarqueClass
        {
            public int EmbarqueID { get; set; }
            public string Embarque { get; set; }
            public List<DatosEmbarque> ListaDatosEmbarque { get; set; }
            public List<ProveedorEnvioClass> ListaProveedorEnvio { get; set; }
            
        }        

        public class DatosEmbarque
        {           
            public int ProveedorID { get; set; }
            public string Proveedor { get; set; }
            public int TractoID { get; set; }
            public string Tracto { get; set; }
            public int ChoferID { get; set; }
            public string Chofer { get; set; }
            public int ProveedorEnvioID { get; set; }
            public string ProveedorEnvio { get; set; }
            public int TractoEnvioID { get; set; }
            public string TractoEnvio { get; set; }
            public int ChoferEnvioID { get; set; }
            public string ChoferEnvio { get; set; }
            public List<TractoEnvioClass> ListaTractoEnvio { get; set; }
            public List<ChoferEnvioClass> ListaChoferEnvio { get; set; }
        }
        
        public class ProveedorEnvioClass
        {
            public ProveedorEnvioClass()
            {
                ProveedorEnvioID = 0;
                ProveedorEnvio = "";
            }
            public int ProveedorEnvioID { get; set; }
            public string ProveedorEnvio { get; set; }

        }
        public class TractoEnvioClass
        {
            public TractoEnvioClass()
            {
                TractoEnvioID = 0;
                TractoEnvio = "";
            }
            public int TractoEnvioID { get; set; }
            public string TractoEnvio { get; set; }
        }
        public class ChoferEnvioClass
        {
            public ChoferEnvioClass()
            {
                ChoferEnvioID = 0;
                ChoferEnvio = "";
            }
            public int ChoferEnvioID { get; set; }
            public string ChoferEnvio { get; set; }
        }
    }
}