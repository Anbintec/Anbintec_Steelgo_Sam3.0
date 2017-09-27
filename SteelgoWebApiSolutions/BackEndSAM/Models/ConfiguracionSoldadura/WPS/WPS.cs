using BackEndSAM.Models.ConfiguracionSoldadura.PQR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ConfiguracionSoldadura.WPS
{
    public class WPS
    {
        public WPS()
        {
            Accion = 0;
            WPSID = 0;
            WPSNombre = "";
        }


        public int Accion { get; set; }
        public int WPSID { get; set; }
        public string WPSNombre { get; set; }
        public string WPSNombreOriginal { get; set; }

        public int PQRRaizId { get; set; }
        public string NombrePQRRaiz { get; set; }
        public int PQRRellenoId { get; set; }
        public string NombrePQRRelleno { get; set; }

        public int GrupoPRellenoId { get; set; }
        public string GrupoPRelleno { get; set; }
        public int GrupoPRaizId { get; set; }
        public string GrupoPRaiz { get; set; }

        public int PWHTRaizId { get; set; }
        public bool PWHTRaiz { get; set; }
        public int PWHTRellenoId { get; set; }
        public bool PWHTRelleno { get; set; }

        public int PREHEATRellenoId { get; set; }
        public bool PREHEATRelleno { get; set; }
        public int PREHEATRaizId { get; set; }
        public bool PREHEATRaiz { get; set; }

        public bool CVNRaiz { get; set; }
        public bool CVNRelleno { get; set; }

        public double EspesorMaximo { get; set; }
        public double EspesorMinimo { get; set; }
        public double EspesorMaximoRaiz { get; set; }
        public double EspesorMaximoRelleno { get; set; }

        public decimal RaizEspesorRaiz { get; set; }
        public decimal RaizEspesorRelleno { get; set; }
        public decimal RellenoEspesorRaiz { get; set; }
        public decimal RellenoEspesorRelleno { get; set; }

        public string ProcesoSoldaduraRaiz { get; set; }
        public string ProcesoSoldaduraRelleno { get; set; }

        public int GrupoMaterialBase1RaizUID { get; set; }
        public string GrupoMaterialBase1RaizU { get; set; }
        public int GrupoMaterialBase1RaizDID { get; set; }
        public string GrupoMaterialBase1RaizD { get; set; }
        public int GrupoMaterialBase1RellenoUID { get; set; }
        public string GrupoMaterialBase1RellenoU { get; set; }
        public int GrupoMaterialBase1RellenoDID { get; set; }
        public string GrupoMaterialBase1RellenoD { get; set; }

        public List<DetallePQR> listadoRellenoPQR { get; set; }
        public List<DetallePQR> listadoRaizPQR { get; set; }
        public List<DetallePQR> listadoGrupoP { get; set; }
        public bool RowOk { get; set; }
        public bool EditadoUsuario { get; set; }
        public int RegistrosWPS { get; set; }
        public int QuitarCVN { get; set; }
    }




    public class Captura
    {
        public List<WPSGuardar> Detalles { get; set; }
    }
    public class WPSGuardar
    {
        public int Accion { get; set; }
        public int WPSId { get; set; }
        public string WPSNombre { get; set; }
        public int PQRRaizId { get; set; }
        public int PQRRellenoId { get; set; }
        public int PWHTId { get; set; }
        public int PREHEAT { get; set; }
        public double EspesorMaximoRaiz { get; set; }
        public double EspesorMinimoRaiz { get; set; }
        public double EspesorMaximoRelleno { get; set; }
        public double EspesorMinimoRelleno { get; set; }
        public string GrupoP1 { get; set; }
        public string GrupoP2 { get; set; }
        public int QuitarCVN { get; set; }
        public List<GruposP> gruposCorrectos { get; set; }
    }


    public class GruposP
    {
        public int Accion { get; set; }
        public int WPSID { get; set; }
        public string GrupoP { get; set; }
    }

    public class DetalleWPS
    {
        public DetalleWPS()
        {
            WPSID = 0;
            Nombre = "";
            EspesorRaiz = 0;
            EspesorRelleno = 0;
        }
        public int WPSID { get; set; }
        public string Nombre { get; set; }
        public double EspesorRaiz { get; set; }
        public double EspesorRelleno { get; set; }
    }
}