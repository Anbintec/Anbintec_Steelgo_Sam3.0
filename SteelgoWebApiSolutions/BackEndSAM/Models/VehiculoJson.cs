using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class VehiculoJson
    {
        public string TipoVehiculoID { get; set; }
        public string Placas { get; set; }
        public string TarjetaCirculacion { get; set; }
        public string PolizaSeguro { get; set; }
        public string Unidad { get; set; }
        public string Modelo { get; set; }
        public string VehiculoID { get; set; }
        public string TransportistaID { get; set; }
        public string transportistaNombre { get; set; }
        public string ChoferID { get; set; }
        public string choferNombre { get; set; }
        public string TractoID { get; set; }
    }
}