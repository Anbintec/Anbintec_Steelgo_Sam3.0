using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FrontEndSAM.Controllers.Busqueda
{
    public class BusquedaController : Controller
    {
        public ActionResult Busqueda()
        {
            return View();
        }

        //public async Task<ActionResult> ProxyBusqueda(string url,string token) 
        //{ 
        //    HttpContent content = new StringContent("");
        //    HttpResponseMessage response= new HttpResponseMessage();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        content.Headers.ContentType =
        //            new MediaTypeHeaderValue("application/json");

        //        try
        //        {
        //            using (response = await client.GetAsync(url))
        //            {
        //                response.EnsureSuccessStatusCode();
        //                var products = response.Content.ReadAsStringAsync().Result;
        //                return Content(products);
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        // an error occurred => here you could log the content returned by the remote server
        //        return Content("An error occurred: " + content);
        //    }
        //}
    }

}