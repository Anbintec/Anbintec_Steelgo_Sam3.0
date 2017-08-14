using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesManager
{
    public class UsuarioNotificacion
    {
        public int UsuariosNotificacionesID { get; set; }
        public Nullable<int> TipoNotificacionID { get; set; }
        public Nullable<int> UsuarioID { get; set; }
        public string Email { get; set; }
        public string Plantilla { get; set; }
        public string NombreUsuario { get; set; }
    
    }
}