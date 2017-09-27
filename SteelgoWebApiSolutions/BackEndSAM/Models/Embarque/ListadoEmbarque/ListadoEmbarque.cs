

using BackEndSAM.Models.Sam3General.OpcionValidacion;
using System;
using System.Collections.Generic;

namespace BackEndSAM.Models.Embarque.ListadoEmbarque
{

    public class ListadoDestino
    {
        public ListadoDestino()
        {
            DestinoID = 0;
            Destino = "";
        }

        public int DestinoID { get; set; }
        public string Destino { get; set; }
    }

    public class DetalleListadoEmbarque
    {
        public int EmbarqueID { get; set; }
        public string Embarque { get; set; }
        public int EstatusEmbarqueID { get; set; }
        public int ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public string Planas { get; set; }
        public int DestinoAntID { get; set; }
        public int DestinoID { get; set; }
        public string Destino { get; set; }
        public string SolicitudPermisoAnt { get; set; }
        public string FolioSolicitudPermiso { get; set; }
        public string FechaSolicitudAnt { get; set; }
        public string FechaSolicitudPermiso { get; set; }
        public bool RequierePapCliente { get; set; }
        public bool RequierePermisoAduana { get; set; }
        public bool RequiereRevisionCliente { get; set; }
        public bool OkClienteAnt { get; set; }
        public bool OkCliente { get; set; }
        public bool OkClienteEmbarque{ get; set; }
        public int AprobadoAduanaAnt { get; set; }
        public int AprobadoAduana { get; set; }
        public string AprobadoAduanaDesc { get; set; }
        public bool OkEmbarqueAnt { get; set; }
        public bool OkEmbarque { get; set; }
        public bool Enviar { get; set; }
        public int CapturaEnvioID { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }
        public List<ListadoDestino> listaDestino { get; set; }
        public List<DetalleOpcionValidacion> listaEstatus { get; set; }

    }

    public class ElementosPorStatus
    {
        public int Pendientes { get; set; }
        public int Transito { get; set; }
    }

    public class DetalleJsonGuardar
    {
        public int EmbarqueID { get; set; }
        public int DestinoID { get; set; }
        public string SolicitudPermiso { get; set; }
        public string FechaPermiso { get; set; }
        public int AprobadoAduana { get; set; }
        public bool OkCliente { get; set; }
        public bool OkEmbarque { get; set; }
        public int EstatusCaptura { get; set; }
        public int BitacoraAduana { get; set; }
    }
    public class CapturaListadoEmbarque
    {
        public List<DetalleJsonGuardar> listaDetalle { get; set; }
    }

    public class DetalleJsonEnvio
    {
        public int EmbarqueID { get; set; }
        public int DestinoID { get; set; }
        public int ProyectoID { get; set; }
        public string SolicitudPermiso { get; set; }
        public string FechaPermiso { get; set; }
        public int AprobadoAduana { get; set; }
        public int BitacoraAduana { get; set; }
    }

    public class DetalleListadoEmbarqueEnviado
    {
        public int EmbarqueID { get; set; }
        public string Embarque { get; set; }
        public int EmbarqueEstatusID { get; set; }
        public int? ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public int DestinoID { get; set; }
        public string Destino { get; set; }
        public string FolioSolicitudPermiso { get; set; }
        public string FechaSolicitudPermiso { get; set; }
        public string Planas { get; set; }
        public Nullable<bool> RequierePapCliente { get; set; }
        public Nullable<bool> RequierePermisoAduana { get; set; }
        public Nullable<bool> RequiereRevisionCliente { get; set; }
        public Nullable<bool> OkEmbarque { get; set; }
        public int AprobadoAduana { get; set; }
        public string AprobadoAduanaDesc { get; set; }
        public Nullable<bool> OkCliente { get; set; }
        public int? CapturaEnvioID { get; set; }
    }
    public class DetalleListadoEmbarqueCerrado
    {
        public int EmbarqueID { get; set; }
        public string Embarque { get; set; }
        public int EmbarqueEstatusID { get; set; }
        public int? ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public int DestinoID { get; set; }
        public string Destino { get; set; }
        public string FolioSolicitudPermiso { get; set; }
        public string FechaSolicitudPermiso { get; set; }
        public string Planas { get; set; }
        public Nullable<bool> RequierePapCliente { get; set; }
        public Nullable<bool> RequierePermisoAduana { get; set; }
        public Nullable<bool> RequiereRevisionCliente { get; set; }
        public Nullable<bool> OkEmbarque { get; set; }
        public int AprobadoAduana { get; set; }
        public string AprobadoAduanaDesc { get; set; }
        public Nullable<bool> OkCliente { get; set; }
        public int? CapturaEnvioID { get; set; }
    }
}