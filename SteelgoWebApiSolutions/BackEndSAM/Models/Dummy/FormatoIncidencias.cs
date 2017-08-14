using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FormatoIncidencias
    {
        public int numRFI { get; set; }
        public int numRFIRevNo { get; set; }
        public int numNoOfAttachmen { get; set; }
        public DateTime datDate { get; set; }
        public string txtAskedBy { get; set; }
        public string ResponseBy { get; set; }
        public DateTime ResponseDate { get; set; }
        public int TransNo { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
        public bool ynClosed { get; set; }
        public string Reference { get; set; }
        public string mmQuestion { get; set; }
        public string mmResponse { get; set; }
    }
}