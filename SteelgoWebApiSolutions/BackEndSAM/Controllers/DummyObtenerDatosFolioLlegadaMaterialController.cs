using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using BackEndSAM.Models;
using BackEndSAM.DataAcces;
namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyObtenerDatosFolioLlegadaMaterialController : ApiController
    {
        // GET api/dummyobtenerdatosfoliollegadamaterial
        public InfoFolioLlegadaMaterial Get(string FolioLlegadaMaterialID, string FolioCuantificacion,string token)
        {
            InfoFolioLlegadaMaterial info = new InfoFolioLlegadaMaterial();
            TipoPackingList tipoPackinglist = new TipoPackingList();
            tipoPackinglist.id = "2";
            tipoPackinglist.Nombre = "Accesorio";
            TipoUso tipouso = new TipoUso();
            tipouso.id = "1";
            tipouso.Nombre = "Tipo Uso 2";

            info.ProyectoID = 1;
            info.FolioLlegadaHijo = 2;
            info.PackingList = "123";
            info.TipoPackingList = tipoPackinglist;
            info.TipoUso = tipouso;
            info.Estatus = "En Proceso Recepcion";
            return info;
        }

        // GET api/dummyobtenerdatosfoliollegadamaterial/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyobtenerdatosfoliollegadamaterial
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyobtenerdatosfoliollegadamaterial/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyobtenerdatosfoliollegadamaterial/5
        public void Delete(int id)
        {
        }
    }
}
