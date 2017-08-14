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
    public class DummyOperadorController : ApiController
    {
        // GET api/dummyoperador
        public IEnumerable<Operador> Get(string token)
        {
            List<Operador> lstOperador = new List<Operador>();
            Operador operador1 = new Operador();
            Operador operador2 = new Operador();
            Operador operador3 = new Operador();

            operador1.OperadorID = "1";
            operador1.Nombre = "Operador 1";
            lstOperador.Add(operador1);

            operador2.OperadorID = "2";
            operador2.Nombre = "Operador 2";
            lstOperador.Add(operador2);

            operador3.OperadorID = "3";
            operador3.Nombre = "Operador 3";
            lstOperador.Add(operador3);
            return lstOperador.AsEnumerable();
        }
    }
}
