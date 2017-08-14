using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class InfoEtiquetaNumeroUnico
    {
        /// <summary>
        /// Codigo del numero unico
        /// </summary>
        public string NumeroUnico { get; set; }
        /// <summary>
        /// Codigo del ItemCode
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// Codigo de Colada
        /// </summary>
        public string Colada { get; set; }
        /// <summary>
        /// Codigo del certificado
        /// </summary>
        public string Certificado { get; set; }

        public string Cedula { get; set; }
        public string Descripcion { get; set; }
        public string Diametro1 { get; set; }
        public string Diametro2 { get; set; }
        public string Proyecto { get; set; }
    }
}