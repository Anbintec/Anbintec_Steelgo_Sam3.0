using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Deficit
    {
        public string ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public string Diametro1 { get; set; }
        public string Diametro2 { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }
        public string DeficitTotal { get; set; }
    }

    public class DiametrosItemCode
    {
        public string Diametro1 { get; set; }
        public string Diametro2 { get; set; }
    }

    public class SpoolsDeficit
    {
        public string SpoolID { get; set; }
        public string Spool { get; set; }
        public string Prioridad { get; set; }
        public string Peqs { get; set; }
        public string Peso { get; set; }
        public string ItemCodeID { get; set; }
        public List<Deficit> ItemCodes { get; set; }
        public bool Seleccionado { get; set; }

        public SpoolsDeficit()
        {
            ItemCodes = new List<Deficit>();
        }
    }

    public class DatosDeficitItemCodes
    {
        public int ItemCodeID { get; set; }
        public int DeficitTotal { get; set; }
    }

    public class RevisionDeficitDatos
    {
        public string SpoolID { get; set; }
        public string ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public string Diametro1 { get; set; }
        public string Diametro2 { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }
        public string DeficitID { get; set; }
        public string Deficit { get; set; }
    }

    public class SpoolsSeleccionados
    {
        public int Spool { get; set; }
        public List<int> ItemCodeID { get; set; }

        public SpoolsSeleccionados()
        {
            ItemCodeID = new List<int>();
        }
    }
    public class SolucionarRevision
    {
        public int DeficitID { get; set; }
        public int Deficit { get; set; }
    }

    public class DeficitTrabajoMaterial
    {
        public int OrdenTrabajoMaterialID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public int? NumeroUnicoID { get; set; }
        public string Segmento { get; set; }
        public int? CantidadCongelada { get; set; }
    }
}