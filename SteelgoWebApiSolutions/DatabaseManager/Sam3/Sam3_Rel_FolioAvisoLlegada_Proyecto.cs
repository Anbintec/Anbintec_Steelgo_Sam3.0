//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManager.Sam3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sam3_Rel_FolioAvisoLlegada_Proyecto
    {
        public int Rel_FolioAviso_ProyectoID { get; set; }
        public int FolioAvisoLlegadaID { get; set; }
        public int ProyectoID { get; set; }
        public bool Activo { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        public virtual Sam3_FolioAvisoLlegada Sam3_FolioAvisoLlegada { get; set; }
        public virtual Sam3_Proyecto Sam3_Proyecto { get; set; }
    }
}
