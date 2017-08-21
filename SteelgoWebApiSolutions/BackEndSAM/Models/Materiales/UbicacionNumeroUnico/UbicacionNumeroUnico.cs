using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Materiales.UbicacionNumeroUnico
{
    public class UbicacionNumeroUnico
    {
    }
    public class RackUbicacion
    {        
        public RackUbicacion()
        {
            RackID = 0;
            Ubicacion = "";
        }
        public int RackID { get; set; }
        public string Ubicacion { get; set; }
    }
    public class RackPasillo
    {
        public RackPasillo()
        {
            RackID = 0;
            Pasillo = "";
        }
        public int RackID { get; set; }
        public string Pasillo { get; set; }
    }
    public class RackNivel
    {
        public RackNivel()
        {
            RackID = 0;
            Nivel = "";
        }
        public int RackID { get; set; }
        public string Nivel { get; set; }
    }
    public class Rack
    {
        public int RackID { get; set; }
        public string Nombre { get; set; }
    }

    public class DetalleGrid
    {
        public int NumeroUnicoID { get; set; }
        public string NombreNU { get; set; }
        public string ItemCode { get; set; }
        public string DescipcionItemCode { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public string Rack { get; set; }

        public int Consecutivo { get; set; }
    }
    public class Captura
    {
        public List<NumeroUnico> Detalles { get; set; }
    }

    public class NumeroUnico
    {
        public int Consecutivo { get; set; }
    }
}