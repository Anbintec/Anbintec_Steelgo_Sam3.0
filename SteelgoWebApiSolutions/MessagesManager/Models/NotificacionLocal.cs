using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesManager.Models
{ 
    public partial class NotificacionLocal
    {
        public int NotificacionID { get; set; }
        public int UsuarioIDReceptor { get; set; }
        public Nullable<int> UsuarioIDEmisor { get; set; }
        public Nullable<int> TipoNotificacionID { get; set; }
        public string Mensaje { get; set; }
        public System.DateTime FechaEnvio { get; set; }
        public Nullable<System.DateTime> FechaRecepcion { get; set; }
        public Nullable<bool> EstatusLectura { get; set; }
        public Nullable<int> EntidadID { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}
