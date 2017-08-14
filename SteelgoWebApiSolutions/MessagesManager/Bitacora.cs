using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesManager
{
    public class Bitacora
    {
        public int BitacoraId { get; set; }
        public int UsuarioId { get; set; }
        public int TipoActividadID { get; set; }
        public string Mensaje { get; set; }
        public System.DateTime Fecha { get; set; }
        public int EntidadId { get; set; }
    }
}