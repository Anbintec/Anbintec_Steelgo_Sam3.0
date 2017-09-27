using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BackEndSAM.Models.Armado
{
    public class Armado
    {
        public string NumeroControl { get; set; }
        public string Junta { get; set; }

        public string FechaControl { get; set; }
        public string Tubero { get; set; }
        public string Taller { get; set; }

    }

    public class IdOrdenTrabajo
    {
        public string OrdenTrabajo { get; set; }
        public List<IDS> idStatus { get; set; }
    }

    public class IDS
    {
        public string Status { get; set; }
        public string IDValido { get; set; }
        public int Valor { get; set; }
        public string Proyecto { get; set; }
        public int ProyectoID { get; set; }
        public int HabilitadoHoldFecha { get; set; }

        public IDS()
        {
            Status = "";
            IDValido = "";
            Valor = 0;
            Proyecto = "";
            ProyectoID = 0;
            HabilitadoHoldFecha = 0;
        }


    }

    public class CamposPredeterminados
    {
        public string FechaArmado { get; set; }
        public string Muestra { get; set; }
        public string Llena { get; set; }
        public string FormatoFecha { get; set; }
        public string TipoCaptura { get; set; }
    }

    public class JuntaSpool
    {
        public int JuntaSpoolID { get; set; }
        public string Etiqueta { get; set; }

        public JuntaSpool()
        {
            JuntaSpoolID = 0;
            Etiqueta = "";
        }
    }

    public class DetalleCaptura
    {
        public int? IDProyecto { get; set; }
        public string Proyecto { get; set; }
        public string IdOrdenTrabajo { get; set; }
        public string OrdenTrabajo { get; set; }
        public string IdVal { get; set; }
        public string IdText { get; set; }
        public string SpoolID { get; set; }
        public int? JuntaID { get; set; }
        public string Junta { get; set; }

        public string FechaArmado { get; set; }
        public string TuberoID { get; set; }
        public string Tubero { get; set; }

        public string TallerID { get; set; }
        public string Taller { get; set; }
        public string SinCaptura { get; set; }
    }

    public class DetalleDatosJson
    {
        public int AccionNumeroUnico { get; set; }
        public int Accion { get; set; }
        public int? IDProyecto { get; set; }
        public string Proyecto { get; set; }
        public int? IdOrdenTrabajo { get; set; }
        public string OrdenTrabajo { get; set; }
        public int? IdVal { get; set; }
        public string IdText { get; set; }
        public string SpoolID { get; set; }
        public int? JuntaID { get; set; }
        public string Junta { get; set; }
        public int TipoJuntaID { get; set; }
        public string FechaArmado { get; set; }
        public string TuberoID { get; set; }
        public string Tubero { get; set; }
        public List<Tubero> ListaTubero { get; set; }
        public string TallerID { get; set; }
        public string Taller { get; set; }
        public List<Taller> ListaTaller { get; set; }
        public string TipoJunta { get; set; }
        public string Diametro { get; set; }
        public string Cedula { get; set; }
        public string Localizacion { get; set; }
        public string FamiliaAcero { get; set; }
        public string NumeroUnico1ID { get; set; }
        public string NumeroUnico1 { get; set; }
        public string NumeroUnico2ID { get; set; }
        public string NumeroUnico2 { get; set; }
        public string TemplateMensajeTrabajosAdicionales { get; set; }
        public List<NumeroUnico> ListaNumerosUnicos1 { get; set; }
        public List<NumeroUnico> ListaNumerosUnicos2 { get; set; }
        public string SinCaptura { get; set; }
        public List<DetalleTrabajoAdicional> ListaDetalleTrabajoAdicional { get; set; }
        public List<TrabajosAdicionalesXJunta> listadoTrabajosAdicionalesXJunta { get; set; }
        public string DetalleJunta { get; set; }
        public string JuntaAnteriorNumeroUnicoGuardado { get; set; }
        public int? LongitudMaterial1 { get; set; }
        public int? LongitudMaterial2 { get; set; }
        public bool? RevisionNU { get; set; }
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

    public class Tubero
    {
        public int ObreroID { get; set; }
        public string Codigo { get; set; }
        public string TipoObrero { get; set; }
        public string NombreCompleto { get; set; }
        public Tubero()
        {
            ObreroID = 0;
            Codigo = "";
            TipoObrero = "";
            NombreCompleto = "";
        }
    }

    public class NumeroUnico
    {
        public int NumeroUnicoID { get; set; }
        public string Clave { get; set; }

        public int EtiquetaMaterial { get; set; }

        public string Etiqueta { get; set; }

        public string JuntasEncontradas { get; set; }

        public int? LongitudMaterial { get; set; }

        public int? ItemCodeID { get; set; }

        public int? TipoMaterialID { get; set; }

        public string Nombre { get; set; }

        public NumeroUnico()
        {
            NumeroUnicoID = 0;
            Clave = "";
            EtiquetaMaterial = 0;
            Etiqueta = "";
            JuntasEncontradas = "";
            LongitudMaterial = 0;
            ItemCodeID = 0;
            TipoMaterialID = 0;
            Nombre = "";
        }
    }
    public class DetalleTrabajoAdicional
    {
        [Key]
        public int? Accion { get; set; }
        public int? TrabajoAdicionalID { get; set; }
        public string TrabajoAdicional { get; set; }
        public string Observacion { get; set; }
        public int? ArmadoTrabajoAdicionalID { get; set; }
    }

    public class TrabajosAdicionalesXJunta
    {
        public int TrabajoAdicionalID { get; set; }
        public string NombreCorto { get; set; }
        public string SignoInformativo { get; set; } //para saber si 

        public TrabajosAdicionalesXJunta()
        {
            TrabajoAdicionalID = 0;
            NombreCorto = "";
            SignoInformativo = "";
        }
    }

    public class Captura
    {
        public List<DetalleGuardarJson> Detalles { get; set; }
    }

    public class ElementosCapturados
    {
        public List<Elementos> Detalles { get; set; }
    }

    public class Elementos
    {
        public string OrdenTrabajoSpoolID { get; set; }
        public string JuntaID { get; set; }
    }

    public class CapturaJuntas
    {
        public List<DetalleDatosJson> Detalles { get; set; }
    }
    public class DetalleGuardarJson
    {
        public int Accion { get; set; }
        public int TipoJuntaID { get; set; }
        public string JuntaID { get; set; }
        public string TallerID { get; set; }
        public string TuberoID { get; set; }
        public string FechaArmado { get; set; }
        public List<DetalleGuardarTrabajoAdicional> ListaDetalleTrabajoAdicional { get; set; }
        public List<NumerosUnicos> ListaNumeroUnicoAsignado { get; set; }
    }
    public class NumerosUnicos
    {
        public int Accion { get; set; }
        public int JuntaID { get; set; }
        public int NumeroUnico1ID { get; set; }
        public int NumeroUnico2ID { get; set; }
    }
    public class DetalleGuardarTrabajoAdicional
    {
        public int Accion { get; set; }
        public string JuntaID { get; set; }
        public int TrabajoAdicionalID { get; set; }
        public int ArmadoTrabajoAdicionalID { get; set; }
        public string Observacion { get; set; }
    }

    public class ValidarCaptura
    {
        public List<NuCapturado> Detalles { get; set; }
    }

    public class NuCapturado
    {
        public int JuntaSpoolID { get; set; }
        public int NumeroUnico1ID { get; set; }
        public int NumeroUnico2ID { get; set; }
        public int Localizacion1 { get; set; }
        public int Localizacion2 { get; set; }

    }
}