using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ItemCodeSteelgoJson
    {
        public int ItemCodeSteelgoID { get; set; }
        public string DescripcionEspanol { get; set; }
        public string DescripcionIngles { get; set; }
        public decimal Peso { get; set; }
        public decimal Diametro1 { get; set; }
        public decimal Diametro2 { get; set; }
        public int FamiliaAceroID { get; set; }
        public string Familia { get; set; }
        public decimal Area { get; set; }
        public string Cedula { get; set; }
        public int CedulaID { get; set; }
        public string Codigo { get; set; }
        public string TipoAcero { get; set; }
        public int? Cantidad { get; set; }
        public string ColadaNombre { get; set; }
    }

    public class ICSDatosAsociacion
    {
        public string ItemCodeSteelgoID { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionLarga { get; set; }
        public string DescripcionIngles { get; set; }
        public string DescripcionLargaIngles { get; set; }
        public string Rel_ICS_DiametroID { get; set; }
        public string Diametro1ID { get; set; }
        public string Diametro1 { get; set; }
        public string Diametro2ID { get; set; }
        public string Diametro2 { get; set; }
        public string Grupo { get; set; }
        public string GrupoID { get; set; }
        public string FamiliaMaterialID { get; set; }
        public string FamiliaMaterial { get; set; }
        public string Acero { get; set; }
        public string AceroID { get; set; }
        public string CedulaID { get; set; }
        public string CedulaA { get; set; }
        public string CedulaB { get; set; }
        public string Libra { get; set; }
        public string Inch { get; set; }
        public string InchID { get; set; }
        public string MM { get; set; }
        public string Espesor { get; set; }
        public string Peso { get; set; }
        public string Area { get; set; }
        public string TipoMaterial { get; set; }
        public string TieneD2 { get; set; }
        public Boolean Asociado { get; set; }
    }

    public class Captura
    {
        public List<Elementos> Detalles { get; set; }
    }
    public class Elementos
    {
        public string cadena { get; set; }
    }

    public class ItemCodeCliente
    {
        public string ItemCode { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public int MM { get; set; }
        public int Cantidad { get; set; }
    }


    public class ItemCodeClienteRetorno
    {
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string Familia { get; set; }
        public string Cedula { get; set; }
        public string TipoAcero { get; set; }
        public string Colada { get; set; }
        public int? Cantidad { get; set; }
        public int MM { get; set; }
        public string Detallar { get; set; }
        public string TieneNU { get; set; }
        public bool TieneError { get; set; }
        public string BultoID { get; set; }
        public string RelFCId { get; set; }
        public string RelBID { get; set; }
        public int? ItemCodeID { get; set; }
        public string ItemCodeSteelgoID { get; set; }
        public int? ItemCodeOrigenID { get; set; }
        public int? TipoMaterial { get; set; }
        public string TextoTipoMaterial { get; set; }


        //ItemCode: data.ItemCode,
        //    Descripcion: itemCodesJsonSteelgo.Descripcion, 
        //    D1: itemCodesJsonSteelgo.Diametro1, 
        //    D2: itemCodesJsonSteelgo.Diametro2, 
        //    ItemCodeSteelgo: itemCodesJsonSteelgo.ItemCodeSteelgo, 
        //    Familia: itemCodesJsonSteelgo.FamiliaAcero,
        //    Cedula: itemCodesJsonSteelgo.Cedula,
        //    TipoAcero: itemCodesJsonSteelgo.TipoAcero, 
        //    Colada: itemCodesJsonSteelgo.ColadaNombre, 
        //    Cantidad: dataJson[key].Cantidad, 
        //    MM: itemCodesJsonSteelgo.MM, 
        //    Detallar: "No", 
        //    TieneNU: "No", 
        //    TieneError: false, 
        //    BultoID: "",
        //    RelFCId: "", 
        //    RelBID: "", 
        //    ItemCodeID: itemCodesJsonSteelgo.ItemCodeID,
        //    ItemCodeSteelgoID: itemCodesJsonSteelgo.ItemCodeSteelgoID,
        //    ItemCodeOrigenID: itemCodesJsonSteelgo.ItemCodeOrigenID,
        //    TipoMaterial: itemCodesJsonSteelgo.TipoPackingList, 
        //    TextoTipoMaterial: itemCodesJsonSteelgo.TextoTipoPackingList
    }

    public class ElementoValidoItemcode
    {
        public int ItemCodeID { get; set; }
        public int D1 { get; set; }
        public int D2 { get; set; }
        public int MM { get; set; }
    }
}

