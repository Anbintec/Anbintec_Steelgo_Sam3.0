using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.AdminComponentes
{
    public class DetalleGrid
    {
        public int? ComponenteID { get; set; }
        public string Componente { get; set; }
        public string Lote { get; set; }
        public int? Cantidad { get; set; }
        public int UnidadID { get; set; }
        public string Unidad { get; set; }
        public List<UnidadesMedida> ListaUnidadesMedida { get; set; }
        public bool RowOk { get; set; }
        public int Accion { get; set; }
        public int AdminComponentesID { get; set; }
    }

    public class UnidadesMedida
    {
        public int UnidadID { get; set; }
        public string Unidad { get; set; }

        public UnidadesMedida()
        {
            this.Unidad = "";
            this.UnidadID = 0;
        }
    }

    public class Componentes
    {
        public int ComponenteID { get; set; }
        public string Componente { get; set; }
        public string Unidad { get; set; }
        public Componentes()
        {
            ComponenteID = 0;
            Componente = "";
            Unidad = "";
        }
    }

    public class ComponenteAgregado
    {
        public int? ComponenteAgregadoID { get; set; }
        public int? ComponenteID { get; set; }
        public string Nombre { get; set; }
        public int Accion { get; set; }
        public List<Componentes> ListadoComponentes { get; set; }
        public int? ProcesoPinturaID { get; set; }

        public int ProyectoProcesoComponenteID { get; set; }
    }
    public class GuardarGrid
    {
        public int ComponenteID { get; set; }
        public string Lote { get; set; }
        public int Cantidad { get; set; }
        public int Accion { get; set; }
        public int AdminComponentesID { get; set; }
        public int UnidadID { get; set; }
    }

    public class Captura
    {
        public List<GuardarGrid> Detalles { get; set; }
    }
}