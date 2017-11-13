
using BackEndSAM.Models.Pintura.SistemaPinturaAplicable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.PinturaGeneral
{
    public class MedioTransporte
    {
        public int MedioTransporteID { get; set; }
        public int MedioTransporteCargaID { get; set; }
        public string Nombre { get; set; }
        public bool CarroCerrado { get; set; }
        public int  ProyectoID { get; set; }

        public MedioTransporte()
        {
            MedioTransporteID = 0;
            MedioTransporteCargaID = 0;
            Nombre = "";
            CarroCerrado = false;
            ProyectoID = 0;
        }
    }

    public class CapturaMedioTransporte
    {
        public string Nombre { get; set; }
        public int UsuarioID { get; set; }
        public int PatioID { get; set; }
        public int ProyectoID { get; set; }
    }

    public class Color
    {
        public Color()
        {
            ColorID = 0;
            Nombre = "";
            CodigoHexadecimal = "";
    }

        public int ColorID { get; set; }
        public string Nombre { get; set; }
        public string CodigoHexadecimal { get; set; }
    }

    public class Captura
    {
        public List<ImgSerializadas> Detalles { get; set; }
    }

    public class ImgSerializadas
    {
        public string imgSerializada { get; set; }
    }

    public class PinturaRevision
    {
        public int? SpoolID { get; set; }
        public string NombreSpool { get; set; }
        public string NumeroControl { get; set; }
        public int? SistemaPinturaID { get; set; }
        public string SistemaPintura { get; set; }
        public  List<BackEndSAM.Models.Pintura.RevisionPintura.SistemaPintura> ListadoSistemaPinturaPorProyecto { get; set; }
        public int? SistemaPinturaColorID { get; set; }
        public List<ColorPintura> ListaColorPintura { get; set; }
        public string Color { get; set; }
        public decimal? Area { get; set; }
        public bool? GenerarRevision { get; set; }
        public int? ComentarioID { get; set; }
        public string Comentario { get; set; }
        public List<TiposRechazo> ListaMotivosRechazo { get; set; }
        public int? Version { get; set; }
        public int Accion { get; set; }
        
        public bool? NoPintable { get; set; }

        public bool? CargaCarroID { get; set; }

        public int? CarroID { get; set; }

        public int? CuadranteID { get; set; }
        

        public PinturaRevision()
        {
            this.Accion = 0;
            this.SpoolID = 0;
            this.NombreSpool = "";
            this.NumeroControl = "";
            this.SistemaPintura = "";
            this.Color = "";
            this.Area = 0;
            this.GenerarRevision = false;
            this.Comentario = "";
            this.Version = 0;
            this.NoPintable = false;
            this.CargaCarroID = false;
            this.CarroID = 0;
            this.CuadranteID = 0;
        }
    }

    public class TiposRechazo
    {
        public int TipoRechazoID { get; set; }
        public string Rechazo { get; set; }

        public TiposRechazo()
        {
            TipoRechazoID = 0;
            Rechazo = "";
        }

    }
    public class GuardarRevisionPintura
    {
        public List<GuardarRevision> Detalles { get; set; }
    }

    public class GuardarRevision
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int SistemaPinturaID { get; set; }
        public int ComentarioID { get; set; }
        public int SistemaPinturaColorID { get; set; }
    }

    

    public class ElementosCapturados
    {
        public List<ElementosSpool> Detalles { get; set; }
    }

    public class ElementosSpool
    {
        public int SpoolID { get; set; }
        public int SistemaPinturaColorID { get; set; }
    }
}