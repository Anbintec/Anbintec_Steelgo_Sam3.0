using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.RevisionEmbarque
{
    public class RevisionEmbarque
    {
    }

    public class DetallePaquete {
        public DetallePaquete()
        {
            PaqueteID = 0;
            Nombre = "";
        }

        public int PaqueteID { get; set; }
        public string Nombre { get; set; }
    }

    public class DetalleProyecto
    {
        public DetalleProyecto()
        {
            ProyectoID = 0;
            Nombre = "";
        }

        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
    }

    public class EmbarqueEnviado
    {
        public EmbarqueEnviado ()
        {
            EmbarqueID = 0;
            Nombre = "";
            ProyectoID = 0;
            Estatus = 0;
            Destino = "";
            RevisionEmbarqueID = 0;
            RevisionCerrada = false;
        }

        public int EmbarqueID { get; set; }
        public string Nombre { get; set; }
        public int ProyectoID { get; set; }
        public int Estatus { get; set; }
        public string Destino { get; set; }
        public int RevisionEmbarqueID { get; set; }
        public bool RevisionCerrada { get; set; }
    }

    public class DetalleRevisionEmbarque
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public int EmbarqueID { get; set; }
        public int EmbarqueEstatusID { get; set; }
        public string Paquete { get; set; }
        public int PaqueteID { get; set; }
        public int DetalleRevisionID { get; set; }
        public bool Llego { get; set; }
        public bool NoLlego { get; set; }
        public bool LlegoComentario { get; set; }
        public string EstatusSpool { get; set; }
        public string Comentario { get; set; }
        public string ComentarioActual { get; set; }
        public bool CapturaManual { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool Empaquetado { get; set; }
        
        public bool RowOk { get; set; }
    }

    public class DetalleSpoolAgregar
    {
        public int Accion { get; set; }
        public int ProyectoID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public int CuadranteID { get; set; }
        public int EmpaquetadoID { get; set; }
        public int DetalleCargaID { get; set; }
        public int EmbarqueID { get; set; }
        public string Embarque { get; set; }
        public int EmbarqueEstatusID { get; set; }
        public string Plana { get; set; }
        public int PaqueteID { get; set; }
        public string Paquete { get; set; }
        public int DetalleRevisionID { get; set; }
        public bool Llego { get; set; }
        public bool NoLlego { get; set; }
        public bool LlegoComentario { get; set; }
        public string EstatusSpool { get; set; }
        public string Comentario { get; set; }
        public string ComentarioActual { get; set; }
        public bool CapturaManual { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }        
    }

    public class DetallePaqueteAgregar
    {
        public int Accion { get; set; }
        public int ProyectoID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public int EmpaquetadoID { get; set; }
        public int CargaPlanaID { get; set; }
        public string Plana { get; set; }
        public int EmbarqueID { get; set; }
        public string Embarque { get; set; }
        public int EmbarqueEstatusID { get; set; }
        public int PaqueteID { get; set; }
        public string Paquete { get; set; }
        public int DetalleRevisionID { get; set; }
        public bool Llego { get; set; }
        public bool NoLlego { get; set; }
        public bool LlegoComentario { get; set; }
        public string EstatusSpool { get; set; }
        public string Comentario { get; set; }
        public string ComentarioActual { get; set; }
        public bool CapturaManual { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }        
        public bool EstuvoEnPaquete { get; set; }
    }

    public class DetalleJsonGuardar
    {
        public int Accion { get; set; }
        public int DetalleRevisionID { get; set; }
        public int SpoolID { get; set; }
        public bool Llego { get; set; }
        public bool NoLlego { get; set; }
        public bool LlegoComentario { get; set; }
        public string Comentario { get; set; }
        public bool CapturaManual { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool EstuvoEnPaquete { get; set; }
    }

    public class CapturaRevisionEmbarque
    {
        public int EmbarqueID { get; set; }
        public int Cerrado { get; set; }
        public List<DetalleJsonGuardar> listaDetalle { get; set; }
    }
    public class IDS
    {
        public int CuadranteID { get; set; }
        public int PaqueteID { get; set; }
    }
}