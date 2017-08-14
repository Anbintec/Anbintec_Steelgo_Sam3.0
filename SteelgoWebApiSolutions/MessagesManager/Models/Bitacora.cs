using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesManager.Models
{
    public partial class Bitacora
    {
        public int BitacoraId { get; set; }
        public int UsuarioId { get; set; }
        public int TipoActividadID { get; set; }
        public string Mensaje { get; set; }
        public System.DateTime Fecha { get; set; }
        public int EntidadId { get; set; }
    }    
}
