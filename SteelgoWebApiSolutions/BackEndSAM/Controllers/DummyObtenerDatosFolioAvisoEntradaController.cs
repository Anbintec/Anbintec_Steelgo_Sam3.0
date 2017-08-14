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
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyObtenerDatosFolioAvisoEntradaController : ApiController
    {
        // GET api/dummyobtenerdatosfolioavisoentrada
        public object Get(string FolioAvisoEntradaID, string token)
        {
            
            List<FolioLlegada1> foliollegada = new List<FolioLlegada1>();
            List<Proyecto> lstProyecto = new List<Proyecto>();
            Proyecto proyecto0 = new Proyecto();
            Proyecto proyecto1 = new Proyecto();

            FolioLlegada1 folio = new FolioLlegada1();
            folio.FolioAvisoEntradaID = 3056;
            folio.FolioCuantificacionID = 1;
            foliollegada.Add(folio);

            FolioLlegada1 folio1 = new FolioLlegada1();
            folio1.FolioAvisoEntradaID = 3056;
            folio1.FolioCuantificacionID = 2;
            foliollegada.Add(folio1);

            proyecto0.Nombre = "Proyecto 1";
            proyecto0.ProyectoID = "1";
            lstProyecto.Add(proyecto0);

            proyecto1.Nombre = "Proyecto 2";
            proyecto1.ProyectoID = "2";
            lstProyecto.Add(proyecto1);

            //TipoPackingList tipoPackinglist= new TipoPackingList();
            //tipoPackinglist.id="2";
            //tipoPackinglist.Nombre = "Accesorio";
            //TipoUso tipouso= new TipoUso();
            //tipouso.id = "2";
            //tipouso.Nombre = "Tipo Uso 2";


            InfoFolioAvisoEntrada info = new InfoFolioAvisoEntrada();
            info.Proyecto = lstProyecto;
            info.FolioLlegada = foliollegada;
            //info.FolioLlegadaHijo = 2;
            //info.PackingList = "123";
            //info.TipoPackingList = tipoPackinglist;
            //info.TipoUso = tipouso;

            return info;
        }

        // GET api/dummyobtenerdatosfolioavisoentrada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyobtenerdatosfolioavisoentrada
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyobtenerdatosfolioavisoentrada/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyobtenerdatosfolioavisoentrada/5
        public void Delete(int id)
        {
        }
    }
}
