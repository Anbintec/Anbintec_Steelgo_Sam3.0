using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Configuration;
using System.Globalization;

namespace BackEndSAM.DataAcces
{
    public class FolioAvisoEntradaBd
    {
        private static readonly object _mutex = new object();
        private static FolioAvisoEntradaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private FolioAvisoEntradaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static FolioAvisoEntradaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FolioAvisoEntradaBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera un set de datos para el grid de Avisos de entrada de material
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerListadoAvisosEntrada(FiltrosJson filtros, Sam3_Usuario usuario)
        {
            try
            {
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        List<int> lstFoliosAvisoEntrada = new List<int>();
                        DateTime fechaInicial = new DateTime();
                        DateTime fechaFinal = new DateTime();
                        DateTime.TryParse(filtros.FechaInicial, out fechaInicial);
                        DateTime.TryParse(filtros.FechaFinal, out fechaFinal);

                        List<int> patiosUsuario;
                        List<int> proyectosUsuario;
                        UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectosUsuario, out patiosUsuario);
                        int patioID = filtros.PatioID != string.Empty ? Convert.ToInt32(filtros.PatioID) > 0 ? Convert.ToInt32(filtros.PatioID) : 0 : 0;
                        int clienteID = filtros.ClienteID != string.Empty ? Convert.ToInt32(filtros.ClienteID) > 0 ? Convert.ToInt32(filtros.ClienteID) : 0 : 0;
                        int folioAvisoLlegadaID = filtros.FolioAvisoLlegadaID != string.Empty ? Convert.ToInt32(filtros.FolioAvisoLlegadaID) > 0 ?
                            Convert.ToInt32(filtros.FolioAvisoLlegadaID) : 0 : 0;
                        int folioEntradaID = filtros.FolioAvisoEntradaID != string.Empty ? Convert.ToInt32(filtros.FolioAvisoEntradaID) > 0 ?
                            Convert.ToInt32(filtros.FolioAvisoEntradaID) : 0 : 0;

                        Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"])
                            ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;

                        if (fechaFinal.ToShortDateString() == "1/1/0001")
                        {
                            fechaFinal = DateTime.Now;
                        }

                        if (fechaInicial.ToShortDateString() == "1/1/0001")
                        {
                            //int mes = DateTime.Now.Month != 1 ? DateTime.Now.Month - 1 : 12;
                            //int year = DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                            //fechaInicial = new DateTime(year, mes, DateTime.Now.Day);
                            fechaInicial = new DateTime(2000, 01, 01);
                        }

                        List<Sam3_FolioAvisoEntrada> result = (from fe in ctx.Sam3_FolioAvisoEntrada
                                                               join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                                               join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                               join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                               join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                               where fe.Activo && fa.Activo && fp.Activo && p.Activo && pa.Activo
                                                               && proyectosUsuario.Contains(p.ProyectoID)
                                                               && patiosUsuario.Contains(pa.PatioID)
                                                               && patiosUsuario.Contains(fe.PatioID)
                                                               && patiosUsuario.Contains(fa.PatioID)
                                                               && (fe.FechaCreacion >= fechaInicial && fe.FechaCreacion <= fechaFinal)
                                                               select fe).Distinct().AsParallel().ToList();

                        if (folioEntradaID > 0)
                        {
                            result = (from r in result
                                      where r.Activo && r.FolioAvisoEntradaID == folioEntradaID
                                      select r).AsParallel().ToList();
                        }
                        if (folioAvisoLlegadaID > 0)
                        {
                            result = (from r in result
                                      where r.FolioAvisoLlegadaID == folioAvisoLlegadaID
                                      select r).AsParallel().ToList();
                        }
                        if (clienteID > 0)
                        {
                            result = (from r in result
                                      join c1 in ctx.Sam3_Cliente on r.ClienteID equals c1.ClienteID
                                      join c2 in ctx2.Cliente on c1.Sam2ClienteID equals c2.ClienteID
                                      where c2.ClienteID == clienteID
                                      select r).AsParallel().ToList();
                        }
                        if (patioID > 0)
                        {
                            result = (from r in result
                                      where r.PatioID == patioID
                                      select r).AsParallel().ToList();
                        }


                        List<ElementoListadoFolioEntradaMaterial> elementos = new List<ElementoListadoFolioEntradaMaterial>();

                        //if (result.Count > 0)
                        //{
                        if (filtros.PorLlegar)
                        {
                            List<Sam3_FolioAvisoLlegada> foliosAvisoLlegada = (from fa in ctx.Sam3_FolioAvisoLlegada.ToList()
                                                                               join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                                               join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                                               join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                                               where fa.Activo && fp.Activo && p.Activo && pa.Activo
                                                                               && proyectosUsuario.Contains(p.ProyectoID)
                                                                               && patiosUsuario.Contains(pa.PatioID)
                                                                               && patiosUsuario.Contains(fa.PatioID)
                                                                               && !(from fe in result
                                                                                    where fe.Activo
                                                                                    select fe.FolioAvisoLlegadaID).Contains(fa.FolioAvisoLlegadaID)
                                                                               && (fa.FechaModificacion >= fechaInicial && fa.FechaModificacion <= fechaFinal)
                                                                               select fa).AsParallel().ToList();

                            if (folioAvisoLlegadaID > 0)
                            {
                                foliosAvisoLlegada = foliosAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegadaID).AsParallel().ToList();
                            }

                            if (clienteID > 0)
                            {
                                foliosAvisoLlegada = (from r in foliosAvisoLlegada
                                                      join c1 in ctx.Sam3_Cliente on r.ClienteID equals c1.ClienteID
                                                      join c2 in ctx2.Cliente on c1.Sam2ClienteID equals c2.ClienteID
                                                      where c2.ClienteID == clienteID
                                                      select r).AsParallel().ToList();

                               // foliosAvisoLlegada = foliosAvisoLlegada.Where(x => x.ClienteID == clienteID).AsParallel().ToList();
                            }

                            if (patioID > 0)
                            {
                                foliosAvisoLlegada = foliosAvisoLlegada.Where(x => x.PatioID == patioID).AsParallel().ToList();
                            }
                           
                            elementos = (from fa in foliosAvisoLlegada
                                         select new ElementoListadoFolioEntradaMaterial
                                         {
                                             EstatusFolio = fa.Estatus,
                                             FechaCreación = fa.FechaModificacion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")),
                                             FolioAvisoEntradaID = fa.FolioAvisoLlegadaID.ToString(),
                                             FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID.ToString(),
                                             Patio = fa.Sam3_Patio.Nombre,
                                             FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                               where pc.Entidad == fa.Entidad && pc.Proyecto == fa.ProyectoNombrado
                                                                                               select pc.PreFijoFolioAvisoLlegada + ","
                                                                                                + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                                + fa.Consecutivo.ToString() + ","
                                                                                                + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : fa.FolioAvisoLlegadaID.ToString()
                                         }).AsParallel().ToList();

                            string max = elementos.Count() > 0 ? elementos.Max(x => x.FolioConfiguracion).ToString() : "0";
                            int maxpadleft = max.Length;

                            //elementos = (from fa in ctx.Sam3_FolioAvisoLlegada.ToList()
                            //             join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                            //             join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                            //             join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                            //             where fa.Activo && fp.Activo && p.Activo && pa.Activo
                            //             && proyectosUsuario.Contains(p.ProyectoID)
                            //             && patiosUsuario.Contains(pa.PatioID)
                            //             && patiosUsuario.Contains(fa.PatioID)
                            //             //&& (fa.ClienteID == clienteID && clienteID > 0)
                            //             //&& (fa.FolioAvisoLlegadaID == folioAvisoLlegadaID && folioAvisoLlegadaID > 0)
                            //             //&& (fa.PatioID == patioID && patioID > 0)
                            //             && !(from fe in result
                            //                  where fe.Activo
                            //                  select fe.FolioAvisoLlegadaID).Contains(fa.FolioAvisoLlegadaID)
                            //             && (fa.FechaModificacion >= fechaInicial && fa.FechaModificacion <= fechaFinal)
                            //             select new ElementoListadoFolioEntradaMaterial
                            //             {
                            //                 EstatusFolio = fa.Estatus,
                            //                 FechaCreación = fa.FechaModificacion.Value.ToString("dd/MM/yyyy"),
                            //                 FolioAvisoEntradaID = fa.FolioAvisoLlegadaID.ToString(),
                            //                 FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID.ToString(),
                            //                 Patio = fa.Sam3_Patio.Nombre,
                            //                 FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                            //                                                                   where pc.Entidad == fa.Entidad && pc.Proyecto == fa.ProyectoNombrado
                            //                                                                   select pc.PreFijoFolioAvisoLlegada + ","
                            //                                                                    + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                            //                                                                    + fa.Consecutivo.ToString() + ","
                            //                                                                    + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : fa.FolioAvisoLlegadaID.ToString()
                            //             }).AsParallel().ToList();


                            foreach (ElementoListadoFolioEntradaMaterial lst in elementos)
                            {
                                if (lst.FolioConfiguracion != null)
                                {
                                    if (activarFolioConfiguracion)
                                    {
                                        string[] elemntos = lst.FolioConfiguracion.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        lst.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    else
                                    {
                                        lst.FolioConfiguracion = lst.FolioConfiguracion.PadLeft(maxpadleft, '0');
                                    }
                                }
                            }
                           
                            return elementos;
                        }

                        if (filtros.PorDescargar)
                        {
                            result = result.Where(x => x.FolioDescarga <= 0 && x.Estatus == "En Patio").ToList();
                        }

                        if (filtros.PorSalir)
                        {
                            result = (from r in result
                                      join f in ctx.Sam3_FolioAvisoLlegada on r.FolioAvisoLlegadaID equals f.FolioAvisoLlegadaID
                                      where r.Activo && f.Activo
                                      && (f.PaseSalidaEnviado == false || r.Estatus == "Cierre de Folio Por Devolución")
                                      && r.Estatus != "En Patio"
                                      select r).AsParallel().ToList();

                        }
                        //}

                        result = result.GroupBy(x => x.FolioAvisoEntradaID).Select(x => x.First()).ToList();

                        string maxresult = result.Count() > 0 ? result.Max(x => x.FolioAvisoLlegadaID).ToString() : "0";
                        int maxresultpadleft = maxresult.Length;
                        foreach (Sam3_FolioAvisoEntrada folio in result)
                        {
                            ElementoListadoFolioEntradaMaterial nuevoElemento = new ElementoListadoFolioEntradaMaterial();

                            Sam3_FolioAvisoLlegada folioavisollegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folio.FolioAvisoLlegadaID).AsParallel().FirstOrDefault();

                            nuevoElemento.FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                            where pc.Entidad == folioavisollegada.Entidad && pc.Proyecto == folioavisollegada.ProyectoNombrado
                                                                                            select pc.PreFijoFolioAvisoLlegada + ","
                                                                                             + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                             + folioavisollegada.Consecutivo.ToString() + ","
                                                                                             + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : folio.FolioAvisoLlegadaID.ToString();
                            if (activarFolioConfiguracion)
                            {
                                string[] elemntos = nuevoElemento.FolioConfiguracion.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                nuevoElemento.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                            else {
                                nuevoElemento.FolioConfiguracion = nuevoElemento.FolioConfiguracion.PadLeft(maxresultpadleft, '0');
                            }
                            nuevoElemento.FolioAvisoEntradaID = folio.FolioAvisoEntradaID.ToString();
                            nuevoElemento.FolioAvisoLlegadaID = folio.FolioAvisoLlegadaID.ToString();
                            nuevoElemento.EstatusFolio = folio.Estatus;
                            nuevoElemento.Patio = folio.Sam3_Patio.Nombre;
                            nuevoElemento.FechaCreación = folio.FechaModificacion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));

                            elementos.Add(nuevoElemento);
                        }

                        return elementos;
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Genera el set de datos para la pantalla de detalle de entrada de material
        /// </summary>
        /// <param name="folio"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object DetalleAvisoEntrada(int folio, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    DetalleAvisoEntradaJson detalle = new DetalleAvisoEntradaJson();
                    Sam3_FolioAvisoEntrada registro =  ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoLlegadaID == folio).AsParallel().FirstOrDefault();
                    Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folio).AsParallel().FirstOrDefault();

                    //el registro de folio de llegada tiene registrado el id del cliente en sam3
                    int ClienteFolioAvisoLlegada = FolioAvisoLlegada.ClienteID.GetValueOrDefault();
                    //obtenemos el id del cliente en sam2
                    int sam2Cliente = (from c in ctx.Sam3_Cliente
                                       where c.Activo && c.ClienteID == ClienteFolioAvisoLlegada
                                       select c.Sam2ClienteID.Value).AsParallel().SingleOrDefault();

                    //Ahora si traemos la informacion del cliente
                    detalle.Cliente = (Models.Cliente)ClienteBd.Instance.ObtnerElementoClientePorID(sam2Cliente);

                    //devuelvo la lista de proyectos registrados en la relacion de aviso de llegada
                    detalle.Proyectos = (from r in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                         join f in ctx.Sam3_FolioAvisoLlegada on r.FolioAvisoLlegadaID equals f.FolioAvisoLlegadaID
                                         join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                         where r.Activo && f.Activo && p.Activo
                                         && r.FolioAvisoLlegadaID == folio
                                         select r.ProyectoID).AsParallel().ToList();

                    detalle.FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                              where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                                                              select pc.PreFijoFolioAvisoLlegada + ","
                                                                               + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                               + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                                                               + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : FolioAvisoLlegada.FolioAvisoLlegadaID.ToString();
                    if (activarFolioConfiguracion)
                    {
                        string[] elemntos = detalle.FolioConfiguracion.Split(',').ToArray();
                        int digitos = Convert.ToInt32(elemntos[1]);
                        int consecutivo = Convert.ToInt32(elemntos[2]);
                        string formato = "D" + digitos.ToString();

                        detalle.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                    };
                    detalle.EstatusAv = FolioAvisoLlegada.Estatus;

                    if (registro != null)
                    {


                        detalle.Documentos = (from d in ctx.Sam3_Rel_FolioAvisoEntrada_Documento
                                              where d.FolioAvisoEntradaID == registro.FolioAvisoEntradaID && d.Activo
                                              select new ListaDocumentos
                                              {
                                                  DocumentoID = d.Rel_FolioAvisoEntrada_DocumentoID.ToString(),
                                                  Nombre = d.Nombre,
                                                  Extencion = d.Extencion,
                                                  TipoArchivo = d.TipoArchivoID.ToString(),
                                                  Url = d.Url,
                                                  Descripcion = d.Descripcion
                                              }).AsParallel().ToList();
                        detalle.FolioAvisoEntradaID = registro.FolioAvisoEntradaID;
                        detalle.Estatus = registro.Estatus;
                        detalle.Factura = registro.Factura;
                        detalle.FolioAvisollegadaId = registro.FolioAvisoLlegadaID.HasValue ? registro.FolioAvisoLlegadaID.Value : 0;
                        detalle.OrdenCompra = registro.OrdenCompra;
                        detalle.FechaFinDescarga = registro.FechaFinDescarga;
                        detalle.FechaGeneracionDescarga = registro.FechaFolioDescarga;
                        detalle.FechaInicioDescarga = registro.FechainicioDescarga;
                        detalle.FolioDescarga = registro.FolioDescarga;
                        detalle.ComboEstatus = registro.ComboEstatus;
                        detalle.IdentificadorCliente = registro.IdentificadorCliente;

                        detalle.Patio = (from p in ctx.Sam3_Patio
                                         where p.Activo && p.PatioID == registro.Sam3_FolioAvisoLlegada.PatioID
                                         select new Models.Patio
                                         {
                                             PatioID = p.PatioID.ToString(),
                                             Nombre = p.Nombre
                                         }).AsParallel().SingleOrDefault();

                        detalle.Proveedor = (from p in ctx.Sam3_Proveedor
                                             where p.ProveedorID == registro.ProveedorID
                                             select new Models.Proveedor
                                             {
                                                 ProveedorID = p.ProveedorID.ToString(),
                                                 Nombre = p.Nombre
                                             }).AsParallel().SingleOrDefault();

                    }
                    return detalle;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Inserta un nuevo registro de folio de aviso de entrada de material
        /// </summary>
        /// <param name="json"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object InsertarFolioAvisoEntrada(FolioAvisoEntradaJson json, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int consecutivo = 0;
                    if (ctx.Sam3_FolioAvisoEntrada.Select(x => x.Consecutivo).Any())
                    {
                        consecutivo = (from r in ctx.Sam3_FolioAvisoEntrada
                                       select r.Consecutivo).Max().Value;
                        consecutivo = consecutivo + 1;
                    }
                    else
                    {
                        consecutivo = 1;
                    }
                     

                    Sam3_Cliente cliente = ctx.Sam3_Cliente.Where(x => x.Sam2ClienteID == json.ClienteId).AsParallel().SingleOrDefault();

                    if (cliente == null)
                    {
                        cliente = new Sam3_Cliente();
                        DatabaseManager.Sam2.Cliente clienteSam2;
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            clienteSam2 = ctx2.Cliente.Where(x => x.ClienteID == json.ClienteId).AsParallel().SingleOrDefault();
                        }
                        cliente.Activo = true;
                        cliente.Ciudad = clienteSam2.Ciudad;
                        cliente.Direccion = clienteSam2.Direccion;
                        cliente.Estado = clienteSam2.Estado;
                        cliente.FechaModificacion = DateTime.Now;
                        cliente.Nombre = clienteSam2.Nombre;
                        cliente.Pais = clienteSam2.Pais;
                        cliente.Sam2ClienteID = clienteSam2.ClienteID;
                        cliente.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_Cliente.Add(cliente);
                        ctx.SaveChanges();
                    }

                    Sam3_FolioAvisoEntrada nuevo = new Sam3_FolioAvisoEntrada();
                    nuevo.Activo = true;
                    nuevo.ClienteID = cliente.ClienteID;
                    nuevo.Consecutivo = consecutivo > 0 ? consecutivo : 1;
                    nuevo.Estatus = json.Estatus;
                    nuevo.Factura = json.Factura;
                    nuevo.FechaModificacion = DateTime.Now;
                    nuevo.FolioAvisoLlegadaID = json.FolioAvisollegadaId;
                    nuevo.OrdenCompra = json.OrdenCompra;
                    nuevo.IdentificadorCliente = json.IdentificadorCliente;
                    nuevo.PatioID = (from fa in ctx.Sam3_FolioAvisoLlegada
                                     where fa.Activo && fa.FolioAvisoLlegadaID == json.FolioAvisollegadaId
                                     select fa.PatioID).AsParallel().SingleOrDefault();
                    nuevo.ProveedorID = json.ProveedorID;
                    nuevo.UsuarioModificacion = usuario.UsuarioID;
                    nuevo.ComboEstatus = json.ComboEstatus;
                    nuevo.FechaCreacion = DateTime.Now;

                    ctx.Sam3_FolioAvisoEntrada.Add(nuevo);
                    ctx.SaveChanges();


                    List<Sam3_Rel_FolioAvisoLlegada_Proyecto> lstProyectos = ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                    .Where(x => x.FolioAvisoLlegadaID == json.FolioAvisollegadaId).ToList();

                    if (json.Proyectos.Count > 0)
                    {
                        List<Sam3_Rel_FolioAvisoLlegada_Proyecto> eliminados = lstProyectos.Where(x => !json.Proyectos.Select(y => y).Contains(x.ProyectoID)).ToList();

                        foreach (Sam3_Rel_FolioAvisoLlegada_Proyecto item in eliminados)
                        {
                            item.Activo = false;
                            ctx.SaveChanges();
                        }

                        foreach (int i in json.Proyectos)
                        {
                            if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == json.FolioAvisollegadaId
                                && x.ProyectoID == i).AsParallel().Any())
                            {
                                Sam3_Rel_FolioAvisoLlegada_Proyecto nFolioProyecto = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                nFolioProyecto.Activo = true;
                                nFolioProyecto.ProyectoID = i;
                                nFolioProyecto.FolioAvisoLlegadaID = json.FolioAvisollegadaId;
                                nFolioProyecto.FechaModificacion = DateTime.Now;
                                nFolioProyecto.UsuarioModificacion = usuario.UsuarioID;
                                ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nFolioProyecto);
                            }
                        }
                    }

                    ctx.SaveChanges();

                    Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == json.FolioAvisollegadaId).AsParallel().SingleOrDefault();
                    FolioAvisoLlegada.ClienteID = cliente.ClienteID;
                    FolioAvisoLlegada.FechaModificacion = DateTime.Now;
                    FolioAvisoLlegada.UsuarioModificacion = usuario.UsuarioID;
                    ctx.SaveChanges();

                    if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(1,
                        string.Format("Se generó un nuevo aviso de Entrada para el folio {0} con fecha {1}",
                        nuevo.FolioAvisoLlegadaID, nuevo.FechaModificacion), usuario))
                    {
                        //Agregar error a la bitacora  PENDIENTE
                    }

                    

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(nuevo.FolioAvisoEntradaID.ToString());
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Actualiza las propiedades de un registro de folio de entrada de material
        /// </summary>
        /// <param name="json"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ActualizarFolioEntrada(FolioAvisoEntradaJson json, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        Sam3_FolioAvisoEntrada registroBd = ctx.Sam3_FolioAvisoEntrada
                            .Where(x => x.FolioAvisoEntradaID == json.FolioAvisoEntradaID).AsParallel().SingleOrDefault();
                        registroBd.ClienteID = (from c in ctx.Sam3_Cliente
                                                where c.Activo && c.Sam2ClienteID == json.ClienteId
                                                select c.ClienteID).AsParallel().SingleOrDefault();
                        registroBd.Estatus = json.Estatus;
                        registroBd.Factura = json.Factura;
                        registroBd.FechaModificacion = DateTime.Now;
                        registroBd.FolioAvisoLlegadaID = json.FolioAvisollegadaId;
                        registroBd.OrdenCompra = json.OrdenCompra;
                        registroBd.IdentificadorCliente = json.IdentificadorCliente;
                        registroBd.PatioID = json.PatioID;
                        registroBd.ProveedorID = json.ProveedorID;
                        registroBd.UsuarioModificacion = usuario.UsuarioID;
                        registroBd.ComboEstatus = json.ComboEstatus;

                        //verificar y registrar los proyectos
                        List<Sam3_Rel_FolioAvisoLlegada_Proyecto> lstProyectos = ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                    .Where(x => x.FolioAvisoLlegadaID == json.FolioAvisollegadaId).ToList();
                        if (json.Proyectos.Count > 0)
                        {
                            List<Sam3_Rel_FolioAvisoLlegada_Proyecto> eliminados = lstProyectos.Where(x => !json.Proyectos.Select(y => y).Contains(x.ProyectoID)).ToList();

                            foreach (Sam3_Rel_FolioAvisoLlegada_Proyecto item in eliminados)
                            {
                                item.Activo = false;
                                ctx.SaveChanges();
                            }

                            foreach (int i in json.Proyectos)
                            {
                                //si no existe registro del proyecto, se crea uno nuevo, de existir el proyecto se deja tal cual esta
                                if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == registroBd.FolioAvisoLlegadaID
                                    && x.ProyectoID == i).AsParallel().Any())
                                {
                                    Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRelProyecto = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                    nuevaRelProyecto.Activo = true;
                                    nuevaRelProyecto.FechaModificacion = DateTime.Now;
                                    nuevaRelProyecto.FolioAvisoLlegadaID = registroBd.FolioAvisoLlegadaID.Value;
                                    nuevaRelProyecto.ProyectoID = i;
                                    nuevaRelProyecto.UsuarioModificacion = usuario.UsuarioID;
                                    ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRelProyecto);
                                }
                            }
                        }

                        Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada
                            .Where(x => x.FolioAvisoLlegadaID == json.FolioAvisollegadaId).AsParallel().SingleOrDefault();
                        FolioAvisoLlegada.ClienteID = (from c in ctx.Sam3_Cliente
                                                       where c.Activo && c.Sam2ClienteID == json.ClienteId
                                                       select c.ClienteID).AsParallel().SingleOrDefault();
                        FolioAvisoLlegada.FechaModificacion = DateTime.Now;
                        FolioAvisoLlegada.UsuarioModificacion = usuario.UsuarioID;


                        ctx.SaveChanges();
                        ctx_tran.Commit();

                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Elimina logicamente un aviso de entrada de material
        /// </summary>
        /// <param name="folio"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarFolioEntrada(int folio, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_FolioAvisoEntrada avisoBd = ctx.Sam3_FolioAvisoEntrada
                        .Where(x => x.FolioAvisoEntradaID == folio).AsParallel().SingleOrDefault();

                    avisoBd.Activo = false;
                    avisoBd.FechaModificacion = DateTime.Now;
                    avisoBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Verifica si un folio de aviso de entrada de material tiene orden de descarga
        /// </summary>
        /// <param name="folioAvisoEntradaID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object TieneOrdenDescarga(int folioAvisoEntradaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int folio = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoEntradaID == folioAvisoEntradaID)
                        .Select(x => x.FolioDescarga).SingleOrDefault();

                    return folio > 0 ? true : false; 
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Genera un nuevo folio de descarga para un aviso de entrada de material
        /// </summary>
        /// <param name="folioAvisoEntradaID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object GenerarFolioDescarga(int folioAvisoEntradaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int consecutivo = 0;
                    if (ctx.Sam3_FolioAvisoEntrada.Select(x => x.FolioDescarga).Any())
                    {
                        consecutivo = ctx.Sam3_FolioAvisoEntrada.Select(x => x.FolioDescarga).Max();
                        consecutivo = consecutivo + 1;
                    }
                    else
                    {
                        consecutivo = 1;
                    }

                    Sam3_FolioAvisoEntrada registroBd = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoEntradaID == folioAvisoEntradaID)
                        .AsParallel().SingleOrDefault();
                    registroBd.Estatus = "Orden de Descarga Generada";
                    registroBd.FolioDescarga = consecutivo;
                    registroBd.FechaFolioDescarga = DateTime.Now;
                    registroBd.FechaModificacion = DateTime.Now;
                    registroBd.FechainicioDescarga = DateTime.Now;
                    registroBd.UsuarioModificacion = usuario.UsuarioID;
                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_FolioAvisoEntrada> registros = new List<Sam3_FolioAvisoEntrada>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    if (proyectoID > 0)
                    {
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && patios.Contains(fe.PatioID)
                                     && IDs.Contains(fe.FolioAvisoEntradaID)
                                     select fe).Distinct().AsParallel().ToList();
                    }
                    else
                    {
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && IDs.Contains(fe.FolioAvisoEntradaID)
                                     && patios.Contains(fe.PatioID)
                                     select fe).Distinct().AsParallel().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3ClienteID = (from c in ctx.Sam3_Cliente
                                             where c.Activo && c.Sam2ClienteID == clienteID
                                             select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = registros.Where(x => x.ClienteID == sam3ClienteID).ToList();
                    }

                    listado = (from r in registros
                               join rif in ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada on r.FolioAvisoEntradaID equals rif.FolioAvisoEntradaID
                               join ind in ctx.Sam3_Incidencia on rif.IncidenciaID equals ind.IncidenciaID
                               join clas in ctx.Sam3_ClasificacionIncidencia on ind.ClasificacionID equals clas.ClasificacionIncidenciaID
                               join ti in ctx.Sam3_TipoIncidencia on ind.TipoIncidenciaID equals ti.TipoIncidenciaID
                               join us in ctx.Sam3_Usuario on ind.UsuarioID equals us.UsuarioID
                               join fe in ctx.Sam3_FolioAvisoEntrada on rif.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                               join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                               where r.Activo && rif.Activo && ind.Activo && clas.Activo && ti.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = clas.Nombre,
                                   Estatus = ind.Estatus,
                                   FechaRegistro = ind.FechaCreacion.ToString(),
                                   FolioIncidenciaID = ind.IncidenciaID.ToString(),
                                   RegistradoPor = us.Nombre + " " + us.ApellidoPaterno,
                                   TipoIncidencia = ti.Nombre,
                                   FolioOriginalID = ind.IncidenciaOriginalID.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == ind.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + ind.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : ind.IncidenciaID.ToString()
                               }).Distinct().AsParallel().ToList();
                    foreach (var it in listado) { 
                        it.FolioOriginalID = (it.FolioOriginalID=="" || it.FolioOriginalID==null) ? it.FolioIncidenciaID : it.FolioOriginalID;
                    }
                    listado.OrderBy(x => x.FolioOriginalID);
                    if (ActivarFolioConfiguracionIncidencias)
                    {
                        foreach (ListadoIncidencias item in listado)
                        {
                            if (!string.IsNullOrEmpty(item.FolioConfiguracionIncidencia))
                            {
                                string[] elemntos = item.FolioConfiguracionIncidencia.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                item.FolioConfiguracionIncidencia = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                            else {
                                item.FolioConfiguracionIncidencia = item.FolioIncidenciaID.ToString();
                            }
                        }
                    }

                }
                return listado;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }
    }
}