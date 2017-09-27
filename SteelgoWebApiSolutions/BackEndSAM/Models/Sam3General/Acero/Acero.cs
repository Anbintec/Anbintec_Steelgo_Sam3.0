using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.FamiliaAcero
{
    public class DetalleFamiliaAcero
    {
        public DetalleFamiliaAcero()
        {
            FamiliaAceroID = 0;
            Nombre = "";
        }

        public int FamiliaAceroID { get; set; }
        public string Nombre { get; set; }
    }
}