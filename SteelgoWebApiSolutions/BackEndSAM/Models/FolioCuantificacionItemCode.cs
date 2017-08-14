using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FolioCuantificacionItemCode
    {
        public int Rel_FolioCuantificacion_ItemCode_ID { get; set; }
        public int FolioCuantificacionID { get; set; }
        public string ItemCodeID { get; set; }
        public string TieneNumerosUnicos { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public bool Activo { get; set; }
    }
}