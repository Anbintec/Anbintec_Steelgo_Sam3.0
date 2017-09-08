using System;
using System.Collections;
using System.Collections.Generic;

namespace SecurityManager.Api.Models
{
    public class TransactionalInformation
    {
        public bool ReturnStatus { get; set; }
        public int ReturnCode { get; set; }
        public List<String> ReturnMessage { get; set; }
        public Hashtable ValidationErrors;
        public int TotalPages;
        public int TotalRows;
        public int PageSize;
        public Boolean IsAuthenicated;
        public int PerfilID { get; set; }

        public TransactionalInformation()
        {
            ReturnMessage = new List<String>();
            //ReturnStatus = true;
            //ReturnCode = 200;
            ValidationErrors = new Hashtable();
            //TotalPages = 0;
            //TotalPages = 0;
            //PageSize = 0;
            IsAuthenicated = false;
        }
    }
}
