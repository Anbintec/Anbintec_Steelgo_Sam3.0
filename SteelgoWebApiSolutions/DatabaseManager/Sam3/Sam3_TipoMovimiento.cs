//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManager.Sam3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sam3_TipoMovimiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sam3_TipoMovimiento()
        {
            this.Sam3_NumeroUnicoMovimiento = new HashSet<Sam3_NumeroUnicoMovimiento>();
        }
    
        public int TipoMovimientoID { get; set; }
        public bool EsEntrada { get; set; }
        public bool EsTransferenciaProcesos { get; set; }
        public bool ApareceEnSaldos { get; set; }
        public bool DisponibleMovimientosUI { get; set; }
        public string Nombre { get; set; }
        public string NombreIngles { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_NumeroUnicoMovimiento> Sam3_NumeroUnicoMovimiento { get; set; }
    }
}
