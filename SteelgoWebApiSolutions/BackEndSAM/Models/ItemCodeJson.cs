using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ItemCodeJson
    {
        public int ItemCodeID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPackingList { get; set; }
        public string TextoTipoPackingList { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeCliente { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionIngles { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public string DescripcionInterna { get; set; }
        public Nullable<decimal> Diametro1 { get; set; }
        public Nullable<decimal> Diametro2 { get; set; }
        public Nullable<int> FamiliaID { get; set; }
        public bool Activo { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<int> MM { get; set; }
        public int ColadaID { get; set; }
        public string ColadaNombre { get; set; }
        public string TipoUsoID { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string ItemCodeSteelgoID { get; set; }
        public string Cedula { get; set; }
        public string TipoAcero { get; set; }
        public string FamiliaAcero { get; set; }
        public int Diametro1ID { get; set; }
        public int Diametro2ID { get; set; }
        public int ItemCodeOrigenID { get; set; }
    }

    public class ItemCodeComplemento
    {
        public int TipoMaterialID { get; set; }
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string NumeroUnicoCliente { get; set; }
        public string ItemCode { get; set; }
        public int ItemCodeID { get; set; }
        public int ItemCodeSteelgoID { get; set; }
        public string Descripcion { get; set; }
        public string Cedula { get; set; }
        public string TipoAcero { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public string MM { get; set; }
        public string Colada { get; set; }
        public string EstatusFisico { get; set; }
        public string EstatusDocumental { get; set; }
        public string TipoUso { get; set; }
        public int ProyectoID { get; set; }
        public bool TieneError { get; set; }
        public string RelFCID { get; set; }
        public string RelBID { get; set; }
        public string RelNUFCBID { get; set; }
        public int ColadaID { get; set; }
        public string Titulo { get; set; }
        public string DescripcionIncidencia { get; set; }
        public string ColadaOriginal { get; set; }
        public string TieneComplementoRecepcion { get; set; }
        public string MTR { get; set; }
        public string MTRID { get; set; }
        public string CantidadPiezasMTR { get; set; }
        public string TituloMTR { get; set; }
        public string DescripcionIncidenciaMTR { get; set; }
        public int Diametro1ID { get; set; }
        public int Diametro2ID { get; set; }
        public string DescripcionError { get; set; }
    }


    public class DatosItemCode
    {
        public string ItemCodeID { get; set; }
        public string Codigo { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Descripcion { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string ItemCodeSteelgoID { get; set; }
        public string TipoMaterial { get; set; }
    }

    public class EditarItemCode
    {
        public List<int> NumerosUnicos { get; set; }
        public int ItemCodeID { get; set; }
        public decimal d1 { get; set; }
        public decimal d2 { get; set; }
        public string NumeroUnicoCliente { get; set; }
        public int RelFCID { get; set; }
    }
    public class EditarItemCodeMasDetalle
    {
        public List<int> NumerosUnicos { get; set; }
        public int ItemCodeID { get; set; }
        public decimal d1 { get; set; }
        public decimal d2 { get; set; }
        public string NumeroUnicoCliente { get; set; }
        public int RelFCID { get; set; }
        public string Colada { get; set; }
        public int ColadaID { get; set; }
        public string EstatusFisico { get; set; }
        public string EstatusDocumental { get; set; }
        public string Mtr { get; set; }
        public int MTRID { get; set; }
    }
}