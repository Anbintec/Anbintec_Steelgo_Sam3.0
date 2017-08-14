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
    public class DummyAvisoLlegadaController : ApiController
    {
        // GET api/avisollegada
        public IEnumerable<AvisoLlegada> Get(string folio, string username, string token)
        {
            List<AvisoLlegada> lstAviso = new List<AvisoLlegada>();
            List<Proyecto> lstProyecto= new List<Proyecto>();
            List<Files> listFiles = new List<Files>();
            List<Transportista> lstTransportista= new List<Transportista>();
            List<Proveedor> lstProveedor = new List<Proveedor>();
            List<Patio> lstPatio = new List<Patio>();
            List<Chofer> lstChofer = new List<Chofer>();
            List<Plana> lstPlana = new List<Plana>();
            List<Files> lstfilespasesalida = new List<Files>();

            Proveedor proveedor = new Proveedor();
            Transportista transportista = new Transportista();
            AvisoLlegada aviso = new AvisoLlegada();
            Proyecto proyecto= new Proyecto();
            Patio patio = new Patio();
            Files files = new Files();
            Files files2 = new Files();
            Chofer chofer = new Chofer();
            PlanaEntradaMat plana = new PlanaEntradaMat();
            PermisoAduana permisoaduana1 = new PermisoAduana();
            ArchivoAutorizadoAV archivoautorizado1 = new ArchivoAutorizadoAV();
            PaseSalida paseSalida = new PaseSalida();
            Files archivosPaseSalida= new Files();
            Files archivosPaseSalida1= new Files();

            paseSalida.PaseEnviado = 1;
            archivosPaseSalida.id = 12;
            archivosPaseSalida.Extension = ".xlsx";
            archivosPaseSalida.Archivo = "facebook.com";
            lstfilespasesalida.Add(archivosPaseSalida);

            archivosPaseSalida1.id = 15;
            archivosPaseSalida1.Extension = ".txt";
            archivosPaseSalida1.Archivo = "google.com";
            lstfilespasesalida.Add(archivosPaseSalida1);
            paseSalida.Archivos = lstfilespasesalida;

            files.id = 1;
            files.Archivo = "Prueba1";
            files.Extension = ".doc";
            files.TipoArchivo = "Tipo Archivo 1";
            listFiles.Add(files);

            files2.id = 2;
            files2.Archivo = "Prueba2";
            files2.Extension = ".docx";
            files2.TipoArchivo = "Tipo Archivo 2";
            listFiles.Add(files2);

            transportista.TransportistaID = "1";
            transportista.Nombre = "Francisco Martinez";
            

            proveedor.ProveedorID = "1";
            proveedor.Nombre = "Femsa";


            patio.PatioID = "1";
            patio.Nombre = "value1";
           

            chofer.ChoferID = "1";
            chofer.Nombre = "value1";

            List<PlanaEntradaMat> lstPlana1 = new List<PlanaEntradaMat>();
            PlanaEntradaMat plana1 = new PlanaEntradaMat();

            plana.PlanaID = "1";
            lstPlana1.Add(plana);

            plana1.PlanaID = "2";
            lstPlana1.Add(plana1);

            aviso.FolioAvisoLlegadaID = 1;

            proyecto.ProyectoID = "1";
            lstProyecto.Add(proyecto);
            aviso.Proyectos = lstProyecto;

            aviso.Factura = "RSE-Factfolio1";
            aviso.Transportistas = transportista;
            aviso.FechaRecepcion = Convert.ToDateTime("2015/05/25");
            aviso.Proveedores = proveedor;
            aviso.Planas = lstPlana1;
            aviso.OrdenCompra = "RSE-Factfolio1";
            aviso.Patios = patio;
            aviso.Choferes = chofer;
            aviso.Archivos = new List<Files>();
            aviso.Archivos = listFiles;
            permisoaduana1.PermisoAutorizado = 0;
            permisoaduana1.PermisoTramite = 1;
            permisoaduana1.ArchivoAutorizado = archivoautorizado1;
            aviso.PermisoAduana = permisoaduana1;
            aviso.PaseSalida = paseSalida;
            lstAviso.Add(aviso);

            List<Proveedor> lstProveedor2 = new List<Proveedor>();
            List<Transportista> lstTransportista2 = new List<Transportista>();
            List<Proyecto> lstProyecto2 = new List<Proyecto>();
            List<Files> listFiles2 = new List<Files>();
            List<Patio> lstPatio2 = new List<Patio>();
            List<Chofer> lstChofer2 = new List<Chofer>();
            List<Plana> lstPlana2 = new List<Plana>();
            List<Files> lstfilespasesalida1 = new List<Files>();

            Transportista transportista2 = new Transportista();
            AvisoLlegada aviso1 = new AvisoLlegada();
            Proyecto proyecto1 = new Proyecto();
            Proveedor proveedor2 = new Proveedor();
            Patio patio2 = new Patio();
            Files files1 = new Files();
            Files files3 = new Files();
            Chofer chofer2 = new Chofer();
            PlanaEntradaMat plana2 = new PlanaEntradaMat();
            PermisoAduana permisoaduana = new PermisoAduana();
            ArchivoAutorizadoAV archivoautorizado = new ArchivoAutorizadoAV();

            PaseSalida paseSalida1 = new PaseSalida();
            Files archivosPaseSalidaaa1 = new Files();
            Files archivosPaseSalidaaa2 = new Files();

            paseSalida1.PaseEnviado = 1;
            archivosPaseSalidaaa1.id = 12;
            archivosPaseSalidaaa1.Extension = ".xlsx";
            archivosPaseSalidaaa1.Archivo = "facebook.com";
            lstfilespasesalida1.Add(archivosPaseSalidaaa1);

            archivosPaseSalidaaa2.id = 15;
            archivosPaseSalidaaa2.Extension = ".txt";
            archivosPaseSalidaaa2.Archivo = "google.com";
            lstfilespasesalida1.Add(archivosPaseSalidaaa2);
            paseSalida1.Archivos = lstfilespasesalida1;

            files1.id = 1;
            files1.Archivo = "Prueba1";
            files1.Extension = ".doc";
            files1.TipoArchivo = "TipoArchivo 1";
            listFiles2.Add(files1);
            files3.id = 2;
            files3.Archivo = "Prueba2";
            files3.Extension = ".docx";
            files3.TipoArchivo = "TipoArchivo 2";
            listFiles2.Add(files3);
            transportista2.TransportistaID = "1";
            transportista2.Nombre = "Sara Martinez";
            proveedor2.ProveedorID = "2";
            proveedor2.Nombre = "Kentucky";
            patio2.PatioID = "2";
            patio2.Nombre = "value2";
            chofer2.ChoferID = "2";
            chofer2.Nombre = "value2";

            List<PlanaEntradaMat> lstPlana3 = new List<PlanaEntradaMat>();
            PlanaEntradaMat plana3 = new PlanaEntradaMat();

            plana2.PlanaID = "1";
            lstPlana3.Add(plana2);

            plana3.PlanaID = "2";
            lstPlana3.Add(plana3);


            aviso1.FolioAvisoLlegadaID = 2;
            proyecto1.ProyectoID = "1";
            lstProyecto2.Add(proyecto1);

            archivoautorizado.ArchivoID = 99;
            archivoautorizado.Extension = ".doc";
            archivoautorizado.Nombre = "c://loca..coma";

            aviso1.Proyectos = lstProyecto2;
            aviso1.Factura = "RSE-Factfolio2";
            aviso1.Transportistas = transportista2;
            aviso1.FechaRecepcion = Convert.ToDateTime("2015/05/25");
            aviso1.Proveedores = proveedor2;
            aviso1.Planas = lstPlana3;
            aviso1.OrdenCompra = "RSE-Factfolio2";
            aviso1.Patios = patio2;
            aviso1.Choferes = chofer2;
            aviso1.Archivos = new List<Files>();
            aviso1.Archivos = listFiles2;
            permisoaduana.PermisoAutorizado = 1;
            permisoaduana.PermisoTramite = 1;
            permisoaduana.NumeroPermiso = 123;
            permisoaduana.ArchivoAutorizado = archivoautorizado;
            aviso1.PermisoAduana = permisoaduana;
            aviso1.PaseSalida = paseSalida1;
            lstAviso.Add(aviso1);

            return lstAviso.Where(x => x.FolioAvisoLlegadaID == int.Parse(folio)).AsEnumerable();
        }

        // GET api/avisollegada/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/avisollegada
        public AvisoLlegada Post(string avisollegada, string username, string token)
        {
            AvisoLlegada aviso1 = new AvisoLlegada();
            aviso1.FolioAvisoLlegadaID = 12;
            return aviso1;//
        }

        // PUT api/avisollegada/5
        public void Put(string avisollegada, string username, string token)
        {
        }

        // DELETE api/avisollegada/5
        public void Delete(int id)
        {
        }
    }
}
