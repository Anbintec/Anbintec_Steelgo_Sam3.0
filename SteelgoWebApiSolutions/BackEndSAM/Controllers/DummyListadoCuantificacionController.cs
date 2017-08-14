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
    public class DummyListadoCuantificacionController : ApiController
    {
        // GET api/dummylistadocuantificacion
        public IEnumerable<ListadoCuantificacion> Get(string prueba, string filtros, string username, string token)
        {
            List<ListadoCuantificacion> lstCuantificacion = new List<ListadoCuantificacion>();
            ListadoCuantificacion cuantificacion = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion1 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion2 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion3 = new ListadoCuantificacion();

            cuantificacion.ItemCode = "AAC957927OOY";
            cuantificacion.BultoID = "A1";
            cuantificacion.Descripcion = "Brida";
            cuantificacion.Familia = "Brida";
            cuantificacion.Cedula = "STD";
            cuantificacion.TipoAcero = "CS";
            cuantificacion.D1 = 10;
            cuantificacion.D2 = 2;
            cuantificacion.Cantidad =3;
            cuantificacion.TieneNU = true;
            cuantificacion.TieneError = false;
            lstCuantificacion.Add(cuantificacion);

            cuantificacion1.ItemCode = "AAC957927OOZ";
            cuantificacion1.BultoID = "A2";
            cuantificacion1.Descripcion = "Sockolet";
            cuantificacion1.Familia = "";
            cuantificacion1.Cedula = "";
            cuantificacion1.TipoAcero = "";
            cuantificacion1.D1 = 8;
            cuantificacion1.D2 = 0;
            cuantificacion1.Cantidad = 150;
            cuantificacion1.TieneNU = false;
            cuantificacion1.TieneError = false;
            lstCuantificacion.Add(cuantificacion1);

            cuantificacion2.ItemCode = "AAC957927OOX";
            cuantificacion2.BultoID = "A3";
            cuantificacion2.Descripcion = "Codo45";
            cuantificacion2.Familia = "Codo";
            cuantificacion2.Cedula = "STD";
            cuantificacion2.TipoAcero = "SS";
            cuantificacion2.D1 = 10;
            cuantificacion2.D2 = 0;
            cuantificacion2.Cantidad = 1;
            cuantificacion2.TieneNU = true;
            cuantificacion2.TieneError = false;
            lstCuantificacion.Add(cuantificacion2);


            cuantificacion3.ItemCode = "AAC957927OOT";
            cuantificacion3.BultoID = "A4";
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

            return lstCuantificacion.AsEnumerable();
        }

        // GET api/dummylistadocuantificacion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dummylistadocuantificacion
        public IEnumerable<ListadoCuantificacion> Post(string cerrar, string incompletos, string FolioAvisollegadaId, string FolioCuantificacionID, string cuantificacion, string token)
        {
            List<ListadoCuantificacion> lstCuantificacion = new List<ListadoCuantificacion>();
            ListadoCuantificacion cuantificacion0 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion1 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion2 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion3 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion4 = new ListadoCuantificacion();

            cuantificacion0.ItemCode = "AAC957927OOY";
            cuantificacion0.Descripcion = "Brida";
            cuantificacion0.Familia = "Brida";
            cuantificacion0.Cedula = "STD";
            cuantificacion0.TipoAcero = "CS";
            cuantificacion0.D1 = 10;
            cuantificacion0.D2 = 2;
            cuantificacion0.Cantidad = 3;
            cuantificacion0.TieneNU = false;
            cuantificacion0.TieneError = false;
            lstCuantificacion.Add(cuantificacion0);

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
            cuantificacion2.TieneError = true;
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

            cuantificacion4.ItemCode = "Bulto";
            cuantificacion4.BultoID = "A1";
            cuantificacion4.Descripcion = "Brida";
            cuantificacion4.Familia = "Brida";
            cuantificacion4.Cedula = "STD";
            cuantificacion4.TipoAcero = "CS";
            cuantificacion4.D1 = 10;
            cuantificacion4.D2 = 2;
            cuantificacion4.Cantidad = 3;
            cuantificacion4.TieneNU = false;
            cuantificacion4.TieneError = false;
            lstCuantificacion.Add(cuantificacion4);

            return lstCuantificacion.AsEnumerable();
            //return true;
        }

        // PUT api/dummylistadocuantificacion/5
        public IEnumerable<ListadoCuantificacion> Put(string cerrar, string incompletos, string FolioAvisollegadaId, string FolioCuantificacionID, string cuantificacion, string token)
        {
            List<ListadoCuantificacion> lstCuantificacion = new List<ListadoCuantificacion>();
            ListadoCuantificacion cuantificacion0 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion1 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion2 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion3 = new ListadoCuantificacion();
            ListadoCuantificacion cuantificacion4 = new ListadoCuantificacion();

            cuantificacion0.ItemCode = "AAC957927OOY";
            cuantificacion0.Descripcion = "Brida";
            cuantificacion0.Familia = "Brida";
            cuantificacion0.Cedula = "STD";
            cuantificacion0.TipoAcero = "CS";
            cuantificacion0.D1 = 10;
            cuantificacion0.D2 = 2;
            cuantificacion0.Cantidad = 3;
            cuantificacion0.TieneNU = false;
            cuantificacion0.TieneError = false;
            lstCuantificacion.Add(cuantificacion0);

            cuantificacion1.ItemCode = "AAC957927OOZ";
            cuantificacion1.Descripcion = "Sockolet";
            cuantificacion1.Familia = "";
            cuantificacion1.Cedula = "";
            cuantificacion1.TipoAcero = "";
            cuantificacion1.D1 = 8;
            cuantificacion1.D2 = 0;
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
            cuantificacion2.D2 = 0;
            cuantificacion2.Cantidad = 1;
            cuantificacion2.TieneNU = true;
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
            cuantificacion3.TieneNU = true;
            cuantificacion3.TieneError = false;
            lstCuantificacion.Add(cuantificacion3);

            cuantificacion4.ItemCode = "Bulto";
            cuantificacion4.BultoID = "A1";
            cuantificacion4.Descripcion = "Brida";
            cuantificacion4.Familia = "Brida";
            cuantificacion4.Cedula = "STD";
            cuantificacion4.TipoAcero = "CS";
            cuantificacion4.D1 = 10;
            cuantificacion4.D2 = 2;
            cuantificacion4.Cantidad = 3;
            cuantificacion4.TieneNU = false;
            cuantificacion4.TieneError = false;
            lstCuantificacion.Add(cuantificacion4);

            return lstCuantificacion.AsEnumerable();
        }

        // DELETE api/dummylistadocuantificacion/5
        public void Delete(int id)
        {
        }
    }
}
