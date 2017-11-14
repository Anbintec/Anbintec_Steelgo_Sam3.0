using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.RevisionPintura
{
    public class SistemaPintura
    {
        public int SistemaPinturaID { get; set; }
        public string Nombre { get; set; }
        public int SistemaPinturaProyectoID { get; set; }

        public bool? NoPintable { get; set; }

        public SistemaPintura()
        {
            SistemaPinturaID = 0;
            Nombre = "";
            SistemaPinturaProyectoID = 0;
            NoPintable= false;
        }
    }
}