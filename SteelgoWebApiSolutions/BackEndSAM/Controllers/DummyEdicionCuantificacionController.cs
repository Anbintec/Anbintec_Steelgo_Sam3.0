using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEndSAM.Models;
using System.Web.Http.Cors;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DummyEdicionCuantificacionController : ApiController
    {
        // GET api/dummyedicioncuantificacion
        public EdicionCuantificacion Get(string folioLlegadaID, string folioEntradaID, string packingListID, string username, string token)
        {
            EdicionCuantificacion EdicionCuantificacion = new EdicionCuantificacion();
            Proyecto proyecto= new Proyecto();
            proyecto.ProyectoID = "156";
            proyecto.Nombre = "Proyecto 4";

            FolioLlegada FolioLlegada= new FolioLlegada();
            FolioLlegada.FolioLlegadaID = "13";
            FolioLlegada.Consecutivo = "12333";


            EdicionCuantificacion.Proyecto = proyecto;
            EdicionCuantificacion.FolioLlegada = FolioLlegada;
            EdicionCuantificacion.TerminadoCuantificacion = false;



            List<ListadoCuantificacion> lstCuantificacion = new List<ListadoCuantificacion>();
            ListadoCuantificacion cuantificacion = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion1 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion2 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion3 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion4 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion5 = new ListadoCuantificacion();

            cuantificacion.ItemCode = "AAC957927OOY";
            cuantificacion.Descripcion = "Brida";
            cuantificacion.Familia = "";
            cuantificacion.Cedula = "";
            cuantificacion.TipoAcero = "";
            cuantificacion.D1 = 10;
            cuantificacion.D2 = 2;
            cuantificacion.Cantidad =3;
            cuantificacion.TieneNU = false;
            cuantificacion.TieneError = false;
            lstCuantificacion.Add(cuantificacion);

            cuantificacion1.ItemCode = "AAC957927OOZ";
            cuantificacion1.Descripcion = "Sockolet";
            cuantificacion1.Familia = "";
            cuantificacion1.Cedula = "";
            cuantificacion1.TipoAcero = "";
            cuantificacion1.D1 = 8;
            cuantificacion1.D2 = 1;
            cuantificacion1.Cantidad = 150;
            cuantificacion1.TieneNU = false;
            cuantificacion1.TieneError = false;
            lstCuantificacion.Add(cuantificacion1);

            cuantificacion2.ItemCode = "AAC957927OOX";
            cuantificacion2.Descripcion = "Codo45";
            cuantificacion2.Familia = "Codo";
            cuantificacion2.Cedula = "STD";
            cuantificacion2.TipoAcero = "SS";
            cuantificacion2.D1 = 10;
            cuantificacion2.D2 = 1;
            cuantificacion2.Cantidad = 1;
            cuantificacion2.TieneNU = false;
            cuantificacion2.TieneError = false;
            lstCuantificacion.Add(cuantificacion2);


            cuantificacion3.ItemCode = "AAC957927OOT";
            cuantificacion3.Descripcion = "ARNIKA";
            cuantificacion3.Familia = "ALTA";
            cuantificacion3.Cedula = "ASDA";
            cuantificacion3.TipoAcero = "ABRICA";
            cuantificacion3.D1 = 9;
            cuantificacion3.D2 = 1;
            cuantificacion3.Cantidad = 160;
            cuantificacion3.TieneNU = false;
            cuantificacion3.TieneError = false;
            lstCuantificacion.Add(cuantificacion3);

            cuantificacion4.ItemCode = "AAC957927OOB";
            cuantificacion4.Descripcion = "ARNIKA";
            cuantificacion4.Familia = "ALTA";
            cuantificacion4.Cedula = "ASDA";
            cuantificacion4.TipoAcero = "ABRICA";
            cuantificacion4.D1 = 9;
            cuantificacion4.D2 = 1;
            cuantificacion4.Cantidad = 160;
            cuantificacion4.TieneNU = true;
            cuantificacion4.TieneError = false;
            lstCuantificacion.Add(cuantificacion4);


            cuantificacion5.ItemCode = "AAC957927OOC";
            cuantificacion5.Descripcion = "ARNIKA";
            cuantificacion5.Familia = "ALTA";
            cuantificacion5.Cedula = "ASDA";
            cuantificacion5.TipoAcero = "ABRICA";
            cuantificacion5.D1 = 9;
            cuantificacion5.D2 = 1;
            cuantificacion5.Cantidad = 160;
            cuantificacion5.TieneNU = true;
            cuantificacion5.TieneError = false;
            lstCuantificacion.Add(cuantificacion5);

            EdicionCuantificacion.ListadoCuantificacion = lstCuantificacion;
           

            return EdicionCuantificacion;
        }

        // GET api/dummyedicioncuantificacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummyedicioncuantificacion
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dummyedicioncuantificacion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dummyedicioncuantificacion/5
        public void Delete(int id)
        {
        }
    }
}
