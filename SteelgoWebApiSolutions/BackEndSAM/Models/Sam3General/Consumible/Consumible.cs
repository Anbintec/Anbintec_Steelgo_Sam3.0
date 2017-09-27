using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General.Consumible
{
    public class Consumible
    {
        public int ConsumibleID { get; set; }
        public string Codigo { get; set; }

        public Consumible()
        {
            ConsumibleID = 0;
            Codigo = "";
        }
    }
}