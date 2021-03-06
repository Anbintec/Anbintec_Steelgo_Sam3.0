//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManager.Sam2
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProyectoPrograma
    {
        public ProyectoPrograma()
        {
            this.PeriodoPrograma = new HashSet<PeriodoPrograma>();
        }
    
        public int ProyectoProgramaID { get; set; }
        public int ProyectoID { get; set; }
        public string Rango { get; set; }
        public string Unidades { get; set; }
        public Nullable<int> IsosPlaneados { get; set; }
        public Nullable<int> IsosReprogramados { get; set; }
        public Nullable<int> SpoolsPlaneados { get; set; }
        public Nullable<int> SpoolsReprogramados { get; set; }
        public Nullable<System.Guid> UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public byte[] VersionRegistro { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<PeriodoPrograma> PeriodoPrograma { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
