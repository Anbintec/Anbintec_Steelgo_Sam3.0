using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FoliosGenerados
    {
        public int FolioLlegadaID { get; set; }
        public int FolioAvisoLlegadaID { get; set; }
        public int PackingListID { get; set; }
        public string Proyecto { get; set; }
    }
}