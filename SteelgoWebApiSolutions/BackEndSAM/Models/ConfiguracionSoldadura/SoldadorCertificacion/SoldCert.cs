using BackEndSAM.Models.ConfiguracionSoldadura.PQR;
using BackEndSAM.Models.ConfiguracionSoldadura.WPS;
using BackEndSAM.Models.Sam3General;
using System.Collections.Generic;

namespace BackEndSAM.Models.ConfiguracionSoldadura.SoldadorCertificacion
{
    public class SoldadorCertificacion
    {
        public int Accion { get; set; }
        public int SoldadorCertificacionID { get; set; }
        public int ObreroID { get; set; }
        public string CodigoObrero { get; set; }
        public int WPSID { get; set; }
        public string NombreWPS { get; set; }
        public int ProcesoSoldaduraID { get; set; }
        public string ProcesoSoldadura { get; set; }
        public List<TipoProcesosSoldadura> ListaTipoProcesosSoldadura { get; set; }
        public int TipoDePruebaID { get; set; }
        public string TipoDePrueba { get; set; }
        public int PosicionID { get; set; }//no se requiere
        public int Posicion { get; set; }
        public string FechaInicioCertificado { get; set; }
        public string FechaFinCertificado { get; set; }
        public int CedulaTuboCalificadoID { get; set; }
        public string CedulaTuboCalificado { get; set; }
        public List<CedulaTuboCalificado> ListaCedulaTuboCalificado { get; set; }
        public string DiametroCalificado { get; set; }
        public string DiametroMinimo { get; set; }
        public string EspesorMinimo { get; set; }
        public string EspesorMaximo { get; set; }
        public int PorcentajeJuntasRequiere { get; set; }//no se requiere
        public string CertificadoActivo { get; set; }//no se requiere
        public string PasosSoldadura { get; set; }
        public int UsuarioModificacion { get; set; }//no se requiere
        public List<TipoPrueba> ListaTipoPrueba { get; set; }
        public List<DetallePQR> listadoPQR { get; set; }
        public List<ObreroSteelGo> listaObreros { get; set; }

        public bool RowOk { get; set; }

    }

    public class CedulaTuboCalificado
    {
        public string CedulaTuboCalificadoID { get; set; }
        public string CedulaTuboCalificadoDesc { get; set; }

        public CedulaTuboCalificado()
        {
            CedulaTuboCalificadoID = "0";
            CedulaTuboCalificadoDesc = "";

        }
    }

    public class TipoProcesosSoldadura
    {
        public string TipoProcesoSoldaduraID { get; set; }
        public string TipoProcesoSoldaduraDesc { get; set; }

        public TipoProcesosSoldadura()
        {
            TipoProcesoSoldaduraID = "0";
            TipoProcesoSoldaduraDesc = "";
        }
    }

    public class TipoPrueba
    {
        public string TipoPruebaID { get; set; }

        public string TipoDePrueba { get; set; }

        public TipoPrueba()
        {
            TipoPruebaID = "0";
            TipoDePrueba = "";
        }
    }

    public class GuardarSoldadorCertificacion
    {
        public int SoldadorCertificacionID { get; set; }
        public int Accion { get; set; }
        public int ObreroID { get; set; }
        public int WPSID { get; set; }
        public int ProcesoSoldaduraID { get; set; }
        public int TipoDePruebaID { get; set; }
        public int Posicion { get; set; }
        public string FechaInicioCertificado { get; set; }
        public string FechaFinCertificado { get; set; }
        public int CedulaTuboCalificadoID { get; set; }
        public decimal DiametroCalificado { get; set; }
        public decimal DiametroMinimo { get; set; }
        public decimal EspesorMinimo { get; set; }
        public decimal EspesorMaximo { get; set; }
        public decimal PasosSoldadura { get; set; }
    }

    public class Captura
    {
        public List<GuardarSoldadorCertificacion> Detalles { get; set; }
    }

    public class NuevoSoldadorCertificacion
    {
        public int Accion { get; set; }

        public int NuevoIDSoldadorCertificacion { get; set; }
        public List<TipoProcesosSoldadura> ListaTipoProcesosSoldadura { get; set; }
        public List<CedulaTuboCalificado> ListaCedulaTuboCalificado { get; set; }
        public List<TipoPrueba> ListaTipoPrueba { get; set; }
        public List<BackEndSAM.Models.ConfiguracionSoldadura.WPS.WPS> ListaWPS { get; set; }
        public List<ObreroSteelGo> ListaObrero { get; set; }
    }
}