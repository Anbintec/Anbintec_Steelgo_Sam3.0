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
    
    public partial class InspeccionVisual
    {
        public InspeccionVisual()
        {
            this.JuntaInspeccionVisual = new HashSet<JuntaInspeccionVisual>();
        }
    
        public int InspeccionVisualID { get; set; }
        public int ProyectoID { get; set; }
        public string NumeroReporte { get; set; }
        public System.DateTime FechaReporte { get; set; }
        public Nullable<System.Guid> UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public byte[] VersionRegistro { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<JuntaInspeccionVisual> JuntaInspeccionVisual { get; set; }
    }
}
