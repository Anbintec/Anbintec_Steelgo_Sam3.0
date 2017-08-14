using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;


namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyMaquinaController : ApiController
    {
        // GET api/dummymaquina
        public IEnumerable<Maquina> Get(string token)
        {
            List<Maquina> lstMaquina = new List<Maquina>();
            Maquina maquina1 = new Maquina();
            Maquina maquina2 = new Maquina();
            Maquina maquina3 = new Maquina();

            maquina1.MaquinaID = "1";
            maquina1.Nombre = "Maquina 1";
            lstMaquina.Add(maquina1);

            maquina2.MaquinaID = "2";
            maquina2.Nombre = "Maquina 2";
            lstMaquina.Add(maquina2);

            maquina3.MaquinaID = "3";
            maquina3.Nombre = "Maquina 3";
            lstMaquina.Add(maquina3);

            return lstMaquina.AsEnumerable();
        }

        public int Get(string MaquinaID,string token)
        {

            return 25;
        }
    }
}
