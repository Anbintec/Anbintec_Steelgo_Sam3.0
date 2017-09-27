using BackEndSAM.Models.Sam3General.Cuadrante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.Etiquetado
{
    public class DetalleEtiquetado
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public string Spool { get; set; }
        public int ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public int? CuadranteAnteriorSam2ID { get; set; }
        public int? CuadranteAnteriorSam3ID { get; set; }
        public int? CuadranteID { get; set; }
        public int? CuadranteSam2ID {get;set;}
        public string Cuadrante { get; set; }
        public int EtiquetadoID { get; set; }
        public bool Etiquetado { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool OkPintura { get; set; }
        public bool OkPnd { get; set; }
        public int? ZonaID { get; set; }
        public string Zona { get; set; }
        public string RutaEtiqueta { get; set; }
        public string RutaDibujo { get; set; }
        public string RutaIsometrico { get; set; }
        public string RutaPlano { get; set; }
        public List<UbicacionCuadrante> ListaCuadrantes { get; set; }
    }

    public class DetalleGuardadoEtiquetado
    {
        public int Accion { get; set; }
        public int EtiquetadoID { get; set; }
        public int SpoolID { get; set; }
        public bool Etiquetado { get; set; }
        public int CuadranteID { get; set; }
        public int CuadranteSam2ID { get; set; }
        public int CuadranteAnteriorID { get; set; }
        public int CuadranteAnteriorSam2ID { get; set; }
    }

    public class CapturaEtiquetado
    {
        public List<DetalleGuardadoEtiquetado> listaDetalle { get; set; }
    }

}