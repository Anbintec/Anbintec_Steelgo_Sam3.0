using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FrontEndSAM.Controllers.Proxy
{
    public class ProxyController : Controller
    {
        //
        // GET: /Proxy/
        public ActionResult Index()
        {
            return View();
        }
    }
}
