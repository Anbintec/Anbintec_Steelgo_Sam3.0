using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class DatosFolioLlegadaCuantificacion
    {
        public int FolioCuantificacionID { get; set; }
        public int FolioAvisollegadaId { get; set; }
        public int ProyectoID { get; set; }
        public string PackingList { get; set; }
        public int TipoUso { get; set; }
        public string Estatus { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public bool Activo { get; set; }
        public int? BultoID { get; set; }
        public int? TipoPackingList { get; set; }
        public string OrdenDeCompra { get; set; }
        public string Factura { get; set; }
    }
}