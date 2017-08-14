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
    public class DummyProveedorController : ApiController
    {
        // GET api/dummyproveedor
        public IEnumerable<Proveedor> Get()
        {
            List<Proveedor> lstProveedor = new List<Proveedor>();
            Proveedor proveedor = new Proveedor();
            proveedor.ProveedorID = "1999";
            proveedor.Nombre = "Kentucky";
            lstProveedor.Add(proveedor);

            Proveedor proveedor1 = new Proveedor();
            proveedor1.ProveedorID = "2000";
            proveedor1.Nombre = "Femsa";
            lstProveedor.Add(proveedor1);
            return lstProveedor.AsEnumerable();
        }

        // GET api/dummyproveedor/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyproveedor
        public void Post(string proveedor, string username, string token)
        {
        }

        // PUT api/dummyproveedor/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyproveedor/5
        public void Delete(int id)
        {
        }
    }
}
