using BackEndSAM.Models.ConfiguracionSoldadura.WPS;
using BackEndSAM.Models.Sam3General.Consumible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Fabricacion.Soldadura
{
    public class CapturaSoldadura
    {
        public string NumeroControl { get; set; }
        public string Junta { get; set; }
        public string FechaControl { get; set; }
        public string Tubero { get; set; }
        public string Taller { get; set; }

    }

    public class TrabajosAdicionalesSoldadura
    {
        public int Accion { get; set; }
        public int JuntaSpoolID { get; set; }
        public int SoldaduraTrabajoAdicionalID { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int TrabajoAdicionalID { get; set; }
        public string TrabajoAdicional { get; set; }
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public string Observacion { get; set; }
        public string SignoInformativo { get; set; }
    }

    public class CapturaJuntas
    {
        List<DetalleDatosJsonSoldadura> Detalles { get; set; }
    }

    public class DetalleSoldadoresRaizCapturados
    {

    }

    public class DetalleCapturaSoldadura
    {
        public int IDProyecto { get; set; }
        public string Proyecto { get; set; }
        public string IdOrdenTrabajo { get; set; }
        public string OrdenTrabajo { get; set; }
        public string idVal { get; set; }
        public string idText { get; set; }
        public string SpoolID { get; set; }
        public string JuntaID { get; set; }
        public string Junta { get; set; }
        public string FechaSoldadura { get; set; }
        public string SinCaptura { get; set; }
        public int JuntaSoldaduraID { get; set; }
    }

    public class DetalleDatosJsonSoldadura
    {
        public int Accion { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int IDProyecto { get; set; }
        public string Proyecto { get; set; }
        public string IdOrdenTrabajo { get; set; }
        public string OrdenTrabajo { get; set; }
        public string idVal { get; set; }
        public string idText { get; set; }
        public string SpoolID { get; set; }
        public string DetalleJunta { get; set; }
        public string TipoJuntaID { get; set; }
        public string TipoJunta { get; set; }
        public string TallerID { get; set; }
        public string Taller { get; set; }
        public List<Taller> ListaTaller { get; set; }
        public int WPSID { get; set; }
        public string WPSNombre { get; set; }
        public List<WPS> ListaWPS { get; set; }
        public decimal Diametro { get; set; }
        public decimal Espesor { get; set; }
        public string FechaSoldadura { get; set; }
        public int ProcesoSoldaduraRaizID { get; set; }
        public string ProcesoSoldaduraRaiz { get; set; }
        public List<ProcesoSoldadura> ListadoProcesoSoldaduraRaiz { get; set; }
        public List<Soldadores> ListaSoldadoresRaizCapturados { get; set; }
        public int ProcesoSoldaduraRellenoID { get; set; }
        public string ProcesoSoldaduraRelleno { get; set; }
        public List<ProcesoSoldadura> ListadoProcesoSoldaduraRelleno { get; set; }
        public List<Soldadores> ListaSoldadoresRellenoCapturados { get; set; }
        public string TemplateTrabajosAdicionales { get; set; }
        public List<TrabajosAdicionalesSoldadura> listaTrabajosAdicionalesSoldadura { get; set; }
        public string SinCaptura { get; set; }
        public string JuntaID { get; set; }
        public string Junta { get; set; }
        public string TemplateSoldadoresRaiz { get; set; }
        public string TemplateSoldadoresRelleno { get; set; }
        public List<ObreroSoldador> ListadoSoldadoresRaiz { get; set; }
        public List<ObreroSoldador> ListadoSoldadoresRelleno { get; set; }
        public List<Consumible> ListadoColadas { get; set; }
        public int FamiliaMaterialID { get; set; }
        public List<TrabajosAdicionalesSoldadura> ListaDetalleTrabajoAdicional { get; set; }
        public bool RowOk { get; set; }
        public bool RequierePwht { get; set; }
    }

    public class Taller
    {
        public int TallerID { get; set; }
        public string Nombre { get; set; }

        public Taller()
        {
            TallerID = 0;
            Nombre = "";
        }
    }

    public class ProcesoSoldadura
    {
        public int ProcesoSoldaduraID { get; set; }
        public string Codigo { get; set; }

        public ProcesoSoldadura()
        {
            ProcesoSoldaduraID = 0;
            Codigo = "";
        }
    }

    public class TrabajoAdicional
    {
        public string Trabajo { get; set; }
        public string Observacion { get; set; }
    }
    public class ObreroSoldador
    {
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public string Certificado { get; set; }

        public ObreroSoldador()
        {
            ObreroID = 0;
            Soldador = "";
            Certificado = "";
        }
      

    }
    public class Consumible
    {
        public int ConsumibleID { get; set; }
        public string Colada { get; set; }
        public string Certificado { get; set; }

        public Consumible()
        {
            ConsumibleID = 0;
            Colada = "";
            Certificado = "";
        }
    }

    public class Raiz
    {
        public int Accion { get; set; }
        public int JuntaSoldaduraSoldadoID { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public string wps { get; set; }
    }

    public class Soldadores
    {
        public int Accion { get; set; }
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public int ColadaID { get; set; }
        public string Colada { get; set; }
        public string Observaciones { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int JuntaSoldaduraSoldadoID { get; set; }
        public List<ObreroSoldador> ListaSoldador {get; set;}

        public Soldadores()
        {
            Accion = 0;
            ObreroID = 0;
            Soldador = "";
            ColadaID = 0;
            Colada = "";
            Observaciones = "";
            ListaSoldador = new List<ObreroSoldador>();
        }

    }
    public class SoldadorRaizCertificado
    {
        public int Accion { get; set; }
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public int PQRID { get; set; }
        public int ColadaID { get; set; }
        public string Colada { get; set; }
        public string Observaciones { get; set; }
    }

    public class Relleno
    {
        public int Accion { get; set; }
        public int JuntaSoldaduraSoldadoID { get; set; }
        public int JuntasoldaduraID { get; set; }
        public int ObreroID { get; set; }
        public string Soldador { get; set; }
        public string wps { get; set; }
    }
    public class CamposPredeterminados
    {
        public string FechaSoldadura { get; set; }
        public string Muestra { get; set; }
        public string Llena { get; set; }
        public string FormatoFecha { get; set; }
        public string TipoCaptura { get; set; }
    }

    public class IdOrdenTrabajo
    {
        public string OrdenTrabajo { get; set; }
        public List<IDS> idStatus { get; set; }
    }

    public class IDS
    {
        public IDS()
        {
            Status = "";
            IDValido = "";
            Valor = 0;
            Proyecto = "";
            ProyectoID = 0;
        }

        public string Status { get; set; }
        public string IDValido { get; set; }
        public int Valor { get; set; }
        public string Proyecto { get; set; }
        public int ProyectoID { get; set; }
    }

    public class Captura
    {
        public List<DetalleGuardarJson> Detalles { get; set; }
    }

    public class DetalleGuardarJson
    {
        public int Accion { get; set; }
        public string OrdenTrabajoSpoolID { get; set; }
        public string JuntaSpoolID { get; set; }
        public int TipoJuntaID { get; set; }
        public string EtiquetaJunta { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int JuntaTrabajoID { get; set; }
        public string TallerID { get; set; }
        public int WPSID { get; set; }
        public Nullable<int> ProcesoSoldaduraRaizID { get; set; }
        public Nullable<int> ProcesoSoldaduraRellenoID { get; set; }
        public string FechaSoldadura { get; set; }
        public string FechaReporte { get; set; }
        public List<DetalleGuardarTrabajoAdicional> ListaDetalleTrabajoAdicional { get; set; }
        public List<GuardarSoldaduraSoldado> ListaSoldaduraRaiz { get; set; }
        public List<GuardarSoldaduraSoldado> ListaSoldaduraRelleno { get; set; }
    }

    public class GuardarSoldaduraSoldado
    {
        public int Accion { get; set; }
        public int JuntaSpoolID { get; set; }
        public int JuntaSoldaduraSoldadoID { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int EsRaiz { get; set; }
        public int ObreroID { get; set; }
        public string Comentario { get; set; }
        public int ConsumibleID { get; set; }
    }


    public class DetalleGuardarTrabajoAdicional
    {
        public int Accion { get; set; }
        public int JuntaSpoolID { get; set; }
        public int SoldaduraTrabajoAdicionalID { get; set; }
        public int JuntaSoldaduraID { get; set; }
        public int TrabajoAdicionalID { get; set; }
        public int ObreroID { get; set; }
        public string Observacion { get; set; }
    }


    public class WPS
    {
        public WPS()
        {
            WPSID = 0;
            WPSNombre = "";
            Certificado = "}";
            PQRRaizId = 0;
            NombrePQRRaiz = "";
            PQRRellenoId = 0;
            NombrePQRRelleno = "";
            SignoInformativo = "";
        }

        public int WPSID { get; set; }
        public string WPSNombre { get; set; }
        public int PQRRaizId { get; set; }
        public string NombrePQRRaiz { get; set; }
        public int PQRRellenoId { get; set; }
        public string NombrePQRRelleno { get; set; }
        public bool PWHTId { get; set; }
        public string PWHT { get; set; }
        public bool PREHEATId { get; set; }
        public string PREHEAT { get; set; }
        public int GrupoMaterialBase1RaizUID { get; set; }
        public string GrupoMaterialBase1RaizU { get; set; }
        public int GrupoMaterialBase1RaizDID { get; set; }
        public string GrupoMaterialBase1RaizD { get; set; }
        public int GrupoMaterialBase1RellenoUID { get; set; }
        public string GrupoMaterialBase1RellenoU { get; set; }
        public int GrupoMaterialBase1RellenoDID { get; set; }
        public string GrupoMaterialBase1RellenoD { get; set; }
        public decimal RaizEspesorRaiz { get; set; }
        public decimal RaizEspesorRelleno { get; set; }
        public decimal RellenoEspesorRaiz { get; set; }
        public decimal RellenoEspesorRelleno { get; set; }
        public string ProcesoSoldaduraRaiz { get; set; }
        public string ProcesoSoldaduraRelleno { get; set; }
        public double EspesorMaximo { get; set; }
        public double EspesorMinimo { get; set; }
        public string Certificado { get; set; }
        public string SignoInformativo { get; set; }
    }

   
}