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
    
    public partial class Sam3_Chofer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sam3_Chofer()
        {
            this.Sam3_FolioAvisoLlegada = new HashSet<Sam3_FolioAvisoLlegada>();
            this.Sam3_Rel_Vehiculo_Chofer = new HashSet<Sam3_Rel_Vehiculo_Chofer>();
        }
    
        public int ChoferID { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public int TransportistaID { get; set; }
    
        public virtual Sam3_Transportista Sam3_Transportista { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_FolioAvisoLlegada> Sam3_FolioAvisoLlegada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_Rel_Vehiculo_Chofer> Sam3_Rel_Vehiculo_Chofer { get; set; }
    }
}
