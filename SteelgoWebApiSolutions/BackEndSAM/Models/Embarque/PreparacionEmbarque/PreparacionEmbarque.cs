using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.PreparacionEmbarque
{
    public class PreparacionEmbarque
    {
    }
        
    public class DetalleAgregarPlana
    {
        public int Accion { get; set; }
        public int EmbarqueID { get; set; }
        public string NombreEmbarque { get; set; }
        public int EmbarqueDetalleID { get; set; }
        public int PlanaID { get; set; }
        public string Nombre { get; set; }
        public int CargaPlanaID { get; set; }
        public bool StatusCarga { get; set; }
        public int CantidadElementos { get; set; }
        public decimal M2 { get; set; }
        public decimal Peso { get; set; }
        public decimal Pdi { get; set; }
        public decimal Peq { get; set; }
        public bool ModificadoPorUsuario { get; set; }
    }

    public class EmbarqueDetalle
    {
        public EmbarqueDetalle()
        {
            EmbarqueID = 0;
            Nombre = "";
            NombreCliente = "";
            Estatus = 0;
            TractoID = 0;
            ChoferID = 0;
            TractoEnvioID = 0;
            ChoferEnvioID = 0;
        }

        public int EmbarqueID { get; set; }
        public string Nombre { get; set; }
        public string NombreCliente { get; set; }
        public int Estatus { get; set; }
        public int TractoID { get; set; }
        public int ChoferID { get; set; }
        public int TractoEnvioID { get; set; }
        public int ChoferEnvioID { get; set; }
        public string FechaCreacion { get; set; }
    }

    public class Captura
    {
        public List<GuardarPreparacionEmbarque> Detalles { get; set; }
    }
     
    public class GuardarPreparacionEmbarque
    {
        public int Accion { get; set; }
        public int EmbarqueDetalleID { get; set; }
        public int EmbarqueID { get; set; }
        public int CargaPlanaID { get; set; }
    }

}
