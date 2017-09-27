using BackEndSAM.Models.ConfiguracionSoldadura.SoldadorCertificacion;
using BackEndSAM.Models.Sam3General.TipoJunta;
using System.Collections.Generic;

namespace BackEndSAM.Models.ConfiguracionSoldadura.PQR
{
    public class PQR
    {
        //Grid PQR
        public int PQRID { get; set; }
        public string Nombre { get; set; }
        public int PREHEAT { get; set; }
        public int PWHT { get; set; }
        public int CVN { get; set; }
        public int FN { get; set; }
        public int MacroTest { get; set; }
        public double EspesorRelleno { get; set; }
        public double EspesorRaiz { get; set; }
        public string CodigoRelleno { get; set; }
        public string CodigoRaiz { get; set; }
        public string NumeroP { get; set; }
        //RAIZ
        public string Aporte { get; set; }
        public string GrupoF { get; set; }
        //RELLENO
        public string AporteRelleno { get; set; }
        public string GrupoFRelleno { get; set; }

        public string Mezcla { get; set; }
        public string Respaldo { get; set; }
        public string Especificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        //Grupo P Materiales base
        public int GrupoPMaterialBase1 { get; set; }
        public string GrupoPMaterialBase1Nombre { get; set; }
        public int GrupoPMaterialBase2 { get; set; }
        public string GrupoPMaterialBase2Nombre { get; set; }
        //ProcesoSoldadura
        public int ProcesoSoldaduraRellenoID { get; set; }
        public int ProcesoSoldaduraRaizID { get; set; }
        //Codigo
        public int CodigoASMEID { get; set; }

        //ValidaNombre
        public string Existe { get; set; }
        public int Accion { get; set; }
        public int RegistrosWPS { get; set; }
        public bool RowOk { get; set; }
        public int TipoPruebaID { get; set; }
        public int TipoJuntaID { get; set; }
        public string TipoPrueba { get; set; }
        public string TipoJunta { get; set; }

        public List<ListaProcesoSoldadura> ListaProcesosSoldadura { get; set; }
        public List<ListaMaterialesBase> ListaMaterialesBase { get; set; }
        public List<ListaCodigos> ListaCodigos { get; set; }
        public List<TipoPrueba> listaTipoPrueba { get; set; }
        public List<DetalleTipoJunta> listaTipoJunta { get; set; }


        public PQR()
        {
            PQRID = 0;
            Nombre = "";
        }
    }

    public class ListaProcesoSoldadura
    {
        public ListaProcesoSoldadura()
        {
            this.ProcesoSoldaduraID = 0;
            this.Codigo = "";
        }

        public int ProcesoSoldaduraID { get; set; }
        public string Codigo { get; set; }
    }

    public class ListaMaterialesBase
    {
        public ListaMaterialesBase()
        {
            GrupoPID = 0;
            GrupoP = "";
        }
        public int GrupoPID { get; set; }
        public string GrupoP { get; set; }
    }

    public class ListaCodigos
    {
        public ListaCodigos()
        {
            CodigoAsmeID = 0;
            TipoPruebaId = 0;
            Especificacion = "";
        }
        public int CodigoAsmeID { get; set; }
        public int TipoPruebaId { get; set; }
        public string Especificacion { get; set; }
    }

    public class Captura
    {
        public List<PQRGuardar> Detalles { get; set; }
    }

    public class PQRGuardar
    {
        public int PQRID { get; set; }
        public int Accion { get; set; }
        public string Nombre { get; set; }
        public int PREHEAT { get; set; }
        public int PWHT { get; set; }
        public int CVN { get; set; }
        public int FN { get; set; }
        public int MacroTest { get; set; }
        public int TipoPruebaID { get; set; }
        public int TipoJuntaID { get; set; }
        public double EspesorRelleno { get; set; }
        public double EspesorRaiz { get; set; }
        public int ProcesoSoldaduraRellenoID { get; set; }
        public int ProcesoSoldaduraRaizID { get; set; }
        public int NumeroP { get; set; }
        public int GrupoPMaterialBase1 { get; set; }
        public int GrupoPMaterialBase2 { get; set; }
        public string Aporte { get; set; }
        public string AporteRelleno { get; set; }
        public string Mezcla { get; set; }
        public string Respaldo { get; set; }
        public string GrupoF { get; set; }
        public string GrupoFRelleno { get; set; }
        public int Codigo { get; set; }

    }

    public class IDNuevoRegistro
    {
        public int nuevoID { get; set; }
    }

    public class DetallePQR
    {
        public DetallePQR()
        {
            PQRID = 0;
            Nombre = "";
            PREHEAT = 0;
            PWHT = 0;
            CVN = false;
            FN = 0;
            MacroTest = 0;
            EspesorRaiz = 0;
            EspesorRelleno = 0;
            GrupoPMaterialBase1 = 0;
            GrupoPMaterialBase1Nombre = "";
            GrupoPMaterialBase2 = 0;
            GrupoPMaterialBase2Nombre = "";
            ProcesoSoldaduraRaizID = 0;
            ProcesoSoldaduraRellenoID = 0;
            CodigoRaiz = "";
            CodigoRelleno = "";
        }

        //Grid PQR
        public int PQRID { get; set; }
        public string Nombre { get; set; }
        public int PREHEAT { get; set; }
        public int PWHT { get; set; }
        public bool CVN { get; set; }
        public int FN { get; set; }
        public int MacroTest { get; set; }
        public double EspesorRelleno { get; set; }
        public double EspesorRaiz { get; set; }
        public int GrupoPMaterialBase1 { get; set; }
        public string GrupoPMaterialBase1Nombre { get; set; }
        public int GrupoPMaterialBase2 { get; set; }
        public string GrupoPMaterialBase2Nombre { get; set; }
        public int ProcesoSoldaduraRellenoID { get; set; }
        public int ProcesoSoldaduraRaizID { get; set; }
        public string CodigoRelleno { get; set; }
        public string CodigoRaiz { get; set; }
    }


}