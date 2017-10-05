using BackEndSAM.Models.Pintura.PinturaGeneral;
using BackEndSAM.Models.Sam3General.Cuadrante;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BackEndSAM.Models.Pintura.CapturaAvance
{

    
    public class DetalleCapturaAvanceCarroDimanico
    {
        public Dictionary<string, object> ComponenteDinamico { get; set; }
    }
    public class DetalleCapturaAvanceCarro
    {
        public int? AvanceCarroID { get; set; }
        public int Accion { get; set; }
        public int? CarroID { get; set; }
        public int? CargaCarroID { get; set; }
        public int? SpoolID { get; set; }
        public string Spool { get; set; }
        public int? SistemaPinturaID { get; set; }
        public int? SistemaPinturaProyectoID { get; set; }
        public string SistemaPintura { get; set; }
        public int? ColorPinturaID { get; set; }
        public string Color { get; set; }
        public decimal Area { get; set; }
        public int? LoteID { get; set; }
        public string FechaProceso { get; set; }
        public string plantillaObrero { get; set; }
        public List<PintorSpool> ListaObrerosGuargados { get; set; }
        public List<PintorSpool> ListaObreros { get; set; }
        public List<PintorSpool> ListaObrerosSeleccionados { get; set; }


        public int? PatioID { get; set; }
        public int? CuadranteAnteriorID { get; set; }
        public int? ZonaAnteriorID { get; set; }
        public int? CuadranteID { get; set; }
        public string Cuadrante { get; set; }

        public Dictionary<string, object> ComponenteDinamico { get; set; }

        public string Banderastatus { get; set; }

        public int? SistemaPinturaColorID { get; set; }

    }

    public class CamposPredeterminados
    {
        public string FechaShotblast { get; set; }
        public string FechaPrimario { get; set; }
        public string Llenado { get; set; }
    }
    public class Pintor
    {
        public int ObreroID { get; set; }
        public string Codigo { get; set; }
        public string TipoObrero { get; set; }
    }

    public class Componente
    {
        public string NombreComponente { get; set; }

        public Componente()
        {
            this.NombreComponente = "";
        }
    }

    public class Reductor
    {
        public string NombreReductor { get; set; }

        public Reductor()
        {
            this.NombreReductor = "";
        }
    }

    public class RetornaDetalles
    {
        public List<DetalleCapturaAvanceCarro> listaCapturaAvance { get; set; }
    }

    public class Captura
    {
        public List<DetalleSpool> Detalles { get; set; }
    }

    public class DetalleSpool
    {
        public int? ID { get; set; }
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int ProcesoPinturaID { get; set; }
        public string FechaProceso { get; set; }
        public int SistemaPinturaID { get; set; }
        public string Reductor { get; set; }
        public string ReductorLote { get; set; }
        public int CuadranteID { get; set; }
        public List<ObrerosGuardar> ListaObrerosSeleccionados { get; set; }
        public List<ComponenteDinamico> ListaComponentesDinamicos { get; set; }
    }

    public class CapturaLote
    {
        public List<LoteBaja> Detalles { get; set; }

    }


    public class LoteBaja
    {
        public int ProcesoPintura { get; set; }
        public int SistemPinturaColorID { get; set; }
        public double Area { get; set; }
        public int LoteID { get; set; }
    }
    public class ComponenteDinamico
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public string Componente { get; set; }
        public string Lote { get; set; }
        public int ProcesoPinturaID { get; set; }
    }


    public class PintorSpool
    {
        public int Accion { get; set; }
        public int AvanceCarroObreroId { get; set; }
        public int ObreroID { get; set; }
        public string Codigo { get; set; }
    }


    public class ObrerosGuardar
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int ObreroID { get; set; }
        public int ProcesoPinturaID { get; set; }
    }
    public class CarroCerrado
    {
        public int MedioTransporteID { get; set; }
        public int MedioTransporteCargaID { get; set; }
        public string Nombre { get; set; }
        public int SistemaPinturaProyectoID { get; set; }

        public int CuadranteID { get; set; }

        public CarroCerrado()
        {
            MedioTransporteID = 0;
            MedioTransporteCargaID = 0;
            Nombre = "";
            SistemaPinturaProyectoID = 0;
            CuadranteID = 0;
        }
    }

    public class Lote
    {
        public string NombreLote { get; set; }
        public int? Cantidad { get; set; }
        public Lote()
        {
            NombreLote = "";
            Cantidad = 0;
        }
    }

    public class ListadoLotesPorColumna
    {
        public string NombreColumna { get; set; }
        public List<Lote> ListadoLotes { get; set; }
    }


}