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
    
    public partial class Sam3_Rel_Proyecto_Entidad_Configuracion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sam3_Rel_Proyecto_Entidad_Configuracion()
        {
            this.Sam3_FolioCuantificacion = new HashSet<Sam3_FolioCuantificacion>();
            this.Sam3_Incidencia = new HashSet<Sam3_Incidencia>();
            this.Sam3_OrdenAlmacenaje = new HashSet<Sam3_OrdenAlmacenaje>();
            this.Sam3_OrdenRecepcion = new HashSet<Sam3_OrdenRecepcion>();
        }
    
        public int Rel_Proyecto_Entidad_Configuracion_ID { get; set; }
        public int Proyecto { get; set; }
        public int Entidad { get; set; }
        public string PreFijoFolioAvisoLlegada { get; set; }
        public string PostFijoFolioAvisoLlegada { get; set; }
        public int CantidadCerosFolioAvisoLlegada { get; set; }
        public int ConsecutivoFolioAvisoLlegada { get; set; }
        public string PreFijoFolioPackingList { get; set; }
        public string PostFijoFolioPackingList { get; set; }
        public int CantidadCerosFolioPackingList { get; set; }
        public int ConsecutivoFolioPackingList { get; set; }
        public string PreFijoFolioOrdenRecepcion { get; set; }
        public string PostFijoFolioOrdenRecepcion { get; set; }
        public int CantidadCerosFolioOrdenRecepcion { get; set; }
        public int ConsecutivoFolioOrdenRecepcion { get; set; }
        public string PreFijoFolioOrdenAlmacenaje { get; set; }
        public string PostFijoFolioOrdenAlmacenaje { get; set; }
        public int CantidadCerosFolioOrdenAlmacenaje { get; set; }
        public int ConsecutivoFolioOrdenAlmacenaje { get; set; }
        public string PreFijoFolioIncidencias { get; set; }
        public string PostFijoFolioIncidencias { get; set; }
        public int CantidadCerosFolioIncidencias { get; set; }
        public int ConsecutivoIncidencias { get; set; }
        public string PreFijoFolioPickingTicket { get; set; }
        public string PostFijoFolioPickingTicker { get; set; }
        public int CantidadCerosFolioPickingTicket { get; set; }
        public int ConsecutivoPickingTicket { get; set; }
        public int Activo { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
    
        public virtual Sam3_Entidad Sam3_Entidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_FolioCuantificacion> Sam3_FolioCuantificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_Incidencia> Sam3_Incidencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_OrdenAlmacenaje> Sam3_OrdenAlmacenaje { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sam3_OrdenRecepcion> Sam3_OrdenRecepcion { get; set; }
        public virtual Sam3_Proyecto Sam3_Proyecto { get; set; }
    }
}
