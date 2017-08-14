using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Catalogos
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }

    public class CatalogoPatio
    {
        public string PatioID { get; set; }
        public string Nombre { get; set; }
        public string Propietario { get; set; }
        public string Descripcion { get; set; }
        public string RequierePermisoAduana { get; set; }
    }

    public class CatalogoChofer
    {
        public string ChoferID { get; set; }
        public string Nombre { get; set; }
        public string TransportistaID { get; set; }
        public string TransportistaNombre { get; set; }
    }
    
    public class CatalogoTransportista
    {
        public string TransportistaID { get; set; }
        public string ContactoID { get; set; }
        public string Contacto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

    public class CatalogoTracto
    {
        public string VehiculoID { get; set; }
        public string Placas { get; set; }
        public string TarjetaCirculacion { get; set; }
        public string PolizaSeguro { get; set; }
        public string choferID {get;set;}
        public string choferNombre { get; set; }
        public string transportistaID { get; set; }
        public string transportistaNombre { get; set; }
        public string relVehiculoChoferID { get; set; }
        public string relVehiculoTransportistaID { get; set; }
    }

    public class CatalogoPlana
    {
        public string VehiculoID { get; set; }
        public string TractoID { get; set; }
        public string Placas { get; set; }
        public string Unidad { get; set; }
        public string Modelo { get; set; }
        public string choferID { get; set; }
        public string choferNombre { get; set; }
        public string transportistaID { get; set; }
        public string transportistaNombre { get; set; }
        public string relVehiculoChoferID { get; set; }
        public string relVehiculoTransportistaID { get; set; }
    }

    public class CatalogoProveedor
    {
        public string ProveedorID { get; set; }
        public string ContactoID { get; set; }
        public string Contacto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

    public class CatalogoAcero
    {
        public string AceroID { get; set; }
        public string FamiliaAceroID { get; set; }
        public string FamiliaAcero { get; set; }
        public string Nomenclatura { get; set; }
        public string VerificadoPorCalidad { get; set; }

    }

    public class CatalogoColadas
    {
        public string ColadaID { get; set; }
        public string FabricanteID { get; set; }
        public string Fabricante { get; set; }
        public string AceroID { get; set; }
        public string Acero { get; set; }
        public string ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public string NumeroColada { get; set; }
        public string NumeroCertificado { get; set; }
        public string HoldCalidad { get; set; }
    }

    public class CatalogoFamiliaMaterial
    {
        public string FamiliaMaterialID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoFamiliaAcero
    {
        public string FamiliaAceroID { get; set; }
        public string FamiliaMaterialID { get; set; }
        public string FamiliaMaterial { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string VerificadoPorCalidad { get; set; }
    }

    public class CatalogoFabricante
    {
        public string FabricanteID { get; set; }
        public string ContactoID { get; set; }
        public string Contacto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

    public class CatalogoCedulas
    {
        public string CedulaID { get; set; }
        public string Diametro1ID { get; set; }
        public string Diametro1 { get; set; }
        public string CedulaA { get; set; }
        public string CedulaB { get; set; }
        public string CedulaC { get; set; }
        public string CedulaIn { get; set; }
        public string CedulaMM { get; set; }
        public string EspesorMM { get; set; }
        public string EspesorIn { get; set; }
        public bool Correcta { get; set; }
        public string FactorConversion { get; set; }
        public string MensajeError { get; set; }
        public List<ListaCombos> EspesoresIn { get; set; }
        public List<ListaCombos> EspesoresMm { get; set; }
        public int? CatalogoCedulaID { get; set; }
        public CatalogoCedulas()
        {
            EspesoresIn = new List<ListaCombos>();
            EspesoresMm = new List<ListaCombos>();
        }
    }

    public class CatalogoMTR
    {
        public int MTRID { get; set; }
        public string NumeroMTR { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeID { get; set; }
        public string Colada { get; set; }
        public string ColadaID { get; set; }
        public string CantidadPiezas { get; set; }
        public string Proyecto { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
    }

    public class ValidarCedulas
    {
        public bool HayConflictos { get; set; }
        public List<CatalogoCedulas> CedulasNuevas { get; set; }
        public List<CatalogoCedulas> CedulasExistentes { get; set; }

        public ValidarCedulas()
        {
            CedulasNuevas = new List<CatalogoCedulas>();
            CedulasExistentes = new List<CatalogoCedulas>();
        }
    }
}