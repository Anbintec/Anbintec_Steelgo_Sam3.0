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
    
    public partial class TipoPrueba
    {
        public TipoPrueba()
        {
            this.Defecto = new HashSet<Defecto>();
            this.ReportePnd = new HashSet<ReportePnd>();
            this.ReporteTt = new HashSet<ReporteTt>();
            this.Requisicion = new HashSet<Requisicion>();
        }
    
        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public string NombreIngles { get; set; }
        public string Categoria { get; set; }
        public Nullable<System.Guid> UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public byte[] VersionRegistro { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<Defecto> Defecto { get; set; }
        public virtual ICollection<ReportePnd> ReportePnd { get; set; }
        public virtual ICollection<ReporteTt> ReporteTt { get; set; }
        public virtual ICollection<Requisicion> Requisicion { get; set; }
    }
}
