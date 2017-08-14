using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BackEndSAM.Models;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyFormatoIncidenciasController : ApiController
    {
        // GET api/dummyformatoincidencias
        public IEnumerable<FormatoIncidencias> Get(string incidencias)
        {
            List<FormatoIncidencias> lstFormatoIncidencias = new List<FormatoIncidencias>();
            FormatoIncidencias formatoIncidencias1 = new FormatoIncidencias();
            FormatoIncidencias formatoIncidencias2 = new FormatoIncidencias();
            FormatoIncidencias formatoIncidencias3 = new FormatoIncidencias();

            formatoIncidencias1.numRFI = 1;
            formatoIncidencias1.numRFIRevNo = 0;
            formatoIncidencias1.numNoOfAttachmen = 1;
            formatoIncidencias1.datDate = DateTime.Today;
            formatoIncidencias1.txtAskedBy = "michael.mainvielle";
            formatoIncidencias1.ResponseBy = "Mitchel Richardson";
            formatoIncidencias1.ResponseDate = DateTime.Today;
            formatoIncidencias1.TransNo = 0;
            formatoIncidencias1.ActionBy = "michael.mainvielle";
            formatoIncidencias1.ActionDate = DateTime.Today;
            formatoIncidencias1.ynClosed = true;
            formatoIncidencias1.Reference = "Confirm Nomimal Wall Thickness for 26'' ´Pipe";
            formatoIncidencias1.mmQuestion = "Please review the attached Material Pre-Buy for line item 24 under pipe and confirm that nominal wall thickness applies for the 26'' 0.688 WT-Pipe EFW.";
            formatoIncidencias1.mmResponse = "Confirmed, the wall thickness shown for the 26'' NPS is nominal wall thickness.";
            lstFormatoIncidencias.Add(formatoIncidencias1);

            formatoIncidencias2.numRFI = 2;
            formatoIncidencias2.numRFIRevNo = 0;
            formatoIncidencias2.numNoOfAttachmen = 1;
            formatoIncidencias2.datDate = DateTime.Today;
            formatoIncidencias2.txtAskedBy = "michael.mainvielle";
            formatoIncidencias2.ResponseBy = "Suresh Narayanan";
            formatoIncidencias2.ResponseDate = DateTime.Today;
            formatoIncidencias2.TransNo = 0;
            formatoIncidencias2.ActionBy = "michael.mainvielle";
            formatoIncidencias2.ActionDate = DateTime.Today;
            formatoIncidencias2.ynClosed = true;
            formatoIncidencias2.Reference = "Color Coding of Unassembled Materials";
            formatoIncidencias2.mmQuestion = "Per Document No.: 000-SP-PI02-0225-Technical Specification for Color Coding of Piping Material, Shaw F&M must adhere to the color coding system as indicated.";
            formatoIncidencias2.mmResponse = "Since the fabricator is buying the pipe & transformed to Pipe Spools for shipping to the Field, Shaw can follow their internal color coding requirement for tracking.";
            lstFormatoIncidencias.Add(formatoIncidencias2);

            return lstFormatoIncidencias.AsEnumerable();
        }

        // GET api/dummyformatoincidencias/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyformatoincidencias
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyformatoincidencias/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyformatoincidencias/5
        public void Delete(int id)
        {
        }
    }
}
