using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Cors;
using BackEndSAM.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace BackEndSAM.Controllers
{
    //[EnableCors(origins: "http://localhost:61102", headers: "*", methods: "*", SupportsCredentials = true)]
    [EnableCors(origins: "http://localhost:61102", headers: "*", methods: "*")]
    public class DummyFileManagerController : ApiController
    {

        // GET api/dummyfilemanager/
        public List<ListaDocumentosLlegadaMaterial> Get(string folioLlegada, string token)
        {
            var resultFiles = new List<ListaDocumentosLlegadaMaterial>();
            ListaDocumentosLlegadaMaterial files = new ListaDocumentosLlegadaMaterial();
            files.DocumentoID = "1";
            files.Nombre = "Prueba1";
            files.Extencion = ".doc";
            files.Descripcion = "Prueba description 1";
            files.Url = "";
            resultFiles.Add(files);

            ListaDocumentosLlegadaMaterial files2 = new ListaDocumentosLlegadaMaterial();
            files2.DocumentoID = "2";
            files2.Nombre = "Prueba2";
            files2.Extencion = ".docx";
            files2.Descripcion = "Prueba description 2";
            files2.Url = "";
            resultFiles.Add(files2);
            return resultFiles;
        }

        // PUT api/dummyfilemanager/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyfilemanager/5
        public void Delete(string archivoID, string username, string token)
        {
           
        }


        public Files Post(string username, string token)
        {
            // Check if the request contains multipart/form-data.
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            Files files = new Files();
            files.id = 1;
            files.Archivo = "Prueba1";
            files.Extension = ".doc";
            files.status = "OK";
            return files;
        }
    }
}
