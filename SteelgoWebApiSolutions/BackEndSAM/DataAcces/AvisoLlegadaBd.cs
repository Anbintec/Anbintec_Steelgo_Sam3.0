using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using DatabaseManager.EntidadesPersonalizadas;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Configuration;
using System.Globalization;

namespace BackEndSAM.DataAcces
{
    /// <summary>
    /// Clase que contiene funciones relacionadas con los avisos de Llegada 
    /// </summary>
    public class AvisoLlegadaBd
    {
        private static readonly object _mutex = new object();
        private static AvisoLlegadaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private AvisoLlegadaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static AvisoLlegadaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AvisoLlegadaBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera un nuevo aviso de llegada.
        /// </summary>
        /// <param name="avisoJson">Propiedades del nuevo aviso de llegada</param>
        /// <param name="usuario">Informacion del usuario, que realiza la operación</param>
        /// <returns>Mensaje de Exito o error, que incluye el ID del nuevo aviso de llegada</returns>
        public object GenerarAvisoLlegada(AvisoLlegadaJson avisoJson, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ?
                        (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    string folioConfiguracion = string.Empty;
                    //Buscamos el folio maximo en los avisos de Legada
                    int? nuevoFolio;

                    if (ctx.Sam3_FolioAvisoLlegada.Any())
                    {
                        nuevoFolio = ctx.Sam3_FolioAvisoLlegada.Select(x => x.Consecutivo).Max();
                    }
                    else
                    {
                        nuevoFolio = 0;
                    }

                    if (nuevoFolio > 0)
                    {
                        nuevoFolio = nuevoFolio + 1;
                    }
                    else
                    {
                        nuevoFolio = 1;
                    }

                    DateTime temp = new DateTime();
                    DateTime.TryParse(avisoJson.FechaRecepcion, out temp);

                    //asignamos campos al nueva aviso de llegada
                    Sam3_FolioAvisoLlegada nuevoAvisoLlegada = new Sam3_FolioAvisoLlegada();
                    nuevoAvisoLlegada.Activo = true;
                    nuevoAvisoLlegada.ChoferID = avisoJson.Chofer[0].ChoferID;
                    nuevoAvisoLlegada.Consecutivo = nuevoFolio;
                    nuevoAvisoLlegada.Estatus = "Generado";
                    nuevoAvisoLlegada.EsVirtual = false;
                    nuevoAvisoLlegada.PaseSalidaEnviado = false;
                    nuevoAvisoLlegada.PatioID = avisoJson.Patio[0].PatioID;
                    nuevoAvisoLlegada.TransportistaID = avisoJson.Transportista[0].TransportistaID;
                    nuevoAvisoLlegada.FechaRecepcion = Convert.ToDateTime(avisoJson.FechaRecepcion);
                    nuevoAvisoLlegada.UsuarioModificacion = usuario.UsuarioID;
                    nuevoAvisoLlegada.FechaModificacion = DateTime.Now;
                    nuevoAvisoLlegada.VehiculoID = avisoJson.Tracto.VehiculoID != null ? Convert.ToInt32(avisoJson.Tracto.VehiculoID) : 0;
                    nuevoAvisoLlegada.Entidad = 1;
                    nuevoAvisoLlegada.ProyectoNombrado = 1;
                    //los datos de entrada traen el id del cliente en sam2
                    int sam2ClienteID = avisoJson.Cliente.ClienteID != string.Empty || avisoJson.Cliente.ClienteID != "-1"
                        ? Convert.ToInt32(avisoJson.Cliente.ClienteID) : 0;

                    //insertamos en el id del cliente en sam3
                    if (avisoJson.Cliente.Nombre == "Cliente Default")
                    {
                        nuevoAvisoLlegada.ClienteID = (from c in ctx.Sam3_Cliente
                                                       where c.Activo && c.Nombre == "Cliente Default"
                                                       select c.ClienteID).AsParallel().SingleOrDefault();
                    }
                    else
                    {
                        nuevoAvisoLlegada.ClienteID = (from c in ctx.Sam3_Cliente
                                                       where c.Activo && c.Sam2ClienteID == sam2ClienteID
                                                       select c.ClienteID).AsParallel().SingleOrDefault();
                    }

                    nuevoAvisoLlegada.TipoAvisoID = avisoJson.TipoAviso.TipoAvisoID != null ? Convert.ToInt32(avisoJson.TipoAviso.TipoAvisoID) : 0;
                    //Guardamos los cambios
                    ctx.Sam3_FolioAvisoLlegada.Add(nuevoAvisoLlegada);
                    ctx.SaveChanges();

                    int nuevoID = nuevoAvisoLlegada.FolioAvisoLlegadaID;

                    //guardamos en la relacion entre folios y proyectos
                    List<int> lstProyectos = avisoJson.Proyectos.Select(x => x.ProyectoID).Distinct().ToList();
                    foreach (int proyectoId in lstProyectos)
                    {
                        Sam3_Rel_FolioAvisoLlegada_Proyecto avisoProyecto = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                        avisoProyecto.Activo = true;
                        avisoProyecto.FolioAvisoLlegadaID = nuevoID;
                        avisoProyecto.ProyectoID = proyectoId;
                        avisoProyecto.UsuarioModificacion = usuario.UsuarioID;
                        avisoProyecto.FechaModificacion = DateTime.Now;

                        ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(avisoProyecto);
                    }

                    //Guardamos en la relacion de Avisos y planas
                    foreach (PlanaAV plana in avisoJson.Plana)
                    {
                        if (plana.PlanaID > 0)
                        {
                            Sam3_Rel_FolioAvisoLlegada_Vehiculo nuevaPlana = new Sam3_Rel_FolioAvisoLlegada_Vehiculo();
                            nuevaPlana.Activo = true;
                            nuevaPlana.FolioAvisoLlegadaID = nuevoID;
                            nuevaPlana.VehiculoID = plana.PlanaID;
                            nuevaPlana.UsuarioModificacion = usuario.UsuarioID;
                            nuevaPlana.FechaModificacion = DateTime.Now;

                            ctx.Sam3_Rel_FolioAvisoLlegada_Vehiculo.Add(nuevaPlana);
                        }
                    }

                    //guardamos todos los cambios pendientes
                    ctx.SaveChanges();

                    if (activarFolioConfiguracion)
                    {
                        Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == nuevoAvisoLlegada.FolioAvisoLlegadaID).FirstOrDefault();

                        folioConfiguracion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                              where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                              select pc.PreFijoFolioAvisoLlegada + ","
                                               + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                               + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                               + pc.PostFijoFolioAvisoLlegada).FirstOrDefault();

                        string[] elementos = folioConfiguracion.Split(',').ToArray();
                        int digitos = Convert.ToInt32(elementos[1]);
                        int consecutivo = Convert.ToInt32(elementos[2]);
                        string formato = "D" + digitos.ToString();

                        folioConfiguracion = elementos[0].Trim() + consecutivo.ToString(formato).Trim() + elementos[3].Trim();
                    }
                    else
                    {
                        folioConfiguracion = nuevoAvisoLlegada.FolioAvisoLlegadaID.ToString();
                    };

                    if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(2,
                        string.Format("Se generó un nuevo folio de aviso de llegada con el numero: {0} y con fecha {1}",
                        folioConfiguracion, nuevoAvisoLlegada.FechaModificacion), usuario))
                    {
                        //Agregar error a la bitacora  PENDIENTE
                    }

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(folioConfiguracion);
                    result.ReturnMessage.Add(nuevoAvisoLlegada.FolioAvisoLlegadaID.ToString());
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
        /// Genera un listado de los avisos de llegada de acuerdo a los filtros proporcionados
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="usuario"></param>
        /// <returns>Listado en formato JSON</returns>
        public object ObtenerListadoAvisoLlegada(FiltrosJson filtros, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    List<int> lstFoliosAvisoLlegada = new List<int>();
                    DateTime fechaInicial = new DateTime();
                    DateTime fechaFinal = new DateTime();
                    DateTime.TryParse(filtros.FechaInicial, out fechaInicial);
                    DateTime.TryParse(filtros.FechaFinal, out fechaFinal);
                    List<int> patiosUsuario;
                    List<int> proyectosUsuario;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectosUsuario, out patiosUsuario);

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

                    int folioLlegadaID = filtros.FolioAvisoEntradaID != null ? Convert.ToInt32(filtros.FolioAvisoEntradaID) : 0;
                    int folioAvisoLlegadaID = filtros.FolioAvisoLlegadaID != null ? Convert.ToInt32(filtros.FolioAvisoLlegadaID) : 0;
                    int patioID = filtros.PatioID != "" ? Convert.ToInt32(filtros.PatioID) : 0;
                    int clienteID = filtros.ClienteID != "" ? Convert.ToInt32(filtros.ClienteID) : 0;

                    List<Sam3_FolioAvisoLlegada> result = new List<Sam3_FolioAvisoLlegada>();
                    result = (from fa in ctx.Sam3_FolioAvisoLlegada
                              join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                              join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                              join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                              where fa.Activo && fp.Activo && p.Activo && pa.Activo
                              && proyectosUsuario.Contains(p.ProyectoID)
                              && patiosUsuario.Contains(pa.PatioID)
                              && patiosUsuario.Contains(fa.PatioID)
                              && (fa.FechaRecepcion >= fechaInicial && fa.FechaRecepcion <= fechaFinal)
                              select fa).AsParallel().Distinct().ToList();

                    if (patioID > 0)
                    {
                        result = result.Where(x => x.PatioID == patioID).ToList();
                    }

                    if (clienteID > 0)
                    {
                        var idReal = (from c in ctx.Sam3_Cliente
                                        where c.Activo && c.Sam2ClienteID == clienteID
                                        select c.ClienteID).AsParallel().SingleOrDefault();
                        result = result.Where(x => x.ClienteID == idReal).ToList();
                    }

                    //Filtros Rapidos

                    if (filtros.Completos)
                    {
                        if (result.Count > 0)
                        {
                            List<Sam3_FolioAvisoLlegada> cuentas = (from r in result
                                                                    join pa in ctx.Sam3_Patio on r.PatioID equals pa.PatioID
                                                                    join p in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                                                    where r.Activo && p.Activo
                                                                    && p.PermisoAutorizado == true
                                                                    && pa.RequierePermisoAduana
                                                                    select r).AsParallel().ToList();
                            cuentas.AddRange((from r in result
                                              join pa in ctx.Sam3_Patio on r.PatioID equals pa.PatioID
                                              where r.Activo && pa.Activo
                                              && !pa.RequierePermisoAduana
                                              select r).AsParallel().ToList());

                            cuentas = cuentas.GroupBy(x => x.FolioAvisoLlegadaID).Select(x => x.First()).ToList();

                            result = cuentas;
                        }
                    }

                    if (filtros.SinAutorizacion)
                    {
                        if (result.Count > 0)
                        {
                            result = (from r in result
                                      join pa in ctx.Sam3_Patio on r.PatioID equals pa.PatioID
                                      join p in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                      where r.Activo == true && p.PermisoAutorizado == false && pa.RequierePermisoAduana
                                      select r).AsParallel().ToList();

                        }
                    }

                    if (filtros.SinPermiso)
                    {
                        if (result.Count > 0)
                        {
                            result = (from r in result
                                      join pa in ctx.Sam3_Patio on r.PatioID equals pa.PatioID
                                      where r.Activo == true
                                      && !(from x in ctx.Sam3_PermisoAduana select x.FolioAvisoLlegadaID).Contains(r.FolioAvisoLlegadaID)
                                      && pa.RequierePermisoAduana
                                      select r).AsParallel().ToList();
                        }
                    }

                    result = result.GroupBy(x => x.FolioAvisoLlegadaID).Select(x => x.First()).ToList();

                    lstFoliosAvisoLlegada = result.Select(x => x.FolioAvisoLlegadaID).ToList();



                    List<ElementoListadoFolioAvisoLlegada> resultados = new List<ElementoListadoFolioAvisoLlegada>();
                    ElementoListadoFolioAvisoLlegada elemento = new ElementoListadoFolioAvisoLlegada();
                    string max = lstFoliosAvisoLlegada.Count() > 0 ? lstFoliosAvisoLlegada.Max().ToString() : "0";
                    int maxpadleft = max.Length;

                    foreach (int folio in lstFoliosAvisoLlegada)
                    {
                        int proyectoId = ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == folio)
                            .Select(x => x.ProyectoID).FirstOrDefault();

                        if (ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == folio).Any())
                        {
                            Sam3_FolioAvisoLlegada temp = (from r in ctx.Sam3_FolioAvisoLlegada
                                                           join p in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                                           where r.Activo == true
                                                           && r.FolioAvisoLlegadaID == folio
                                                           select r).AsParallel().SingleOrDefault();

                            //string fechaR = temp.FechaRecepcion.HasValue ? temp.FechaRecepcion.Value.ToString("dd/MM/yyyy") : string.Empty;
                            //string fechag = temp.FechaModificacion.HasValue ? temp.FechaModificacion.Value.ToString("dd/MM/yyyy") : string.Empty;
                            string fechaR = temp.FechaRecepcion.HasValue ? temp.FechaRecepcion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty;
                            string fechag = temp.FechaModificacion.HasValue ? temp.FechaModificacion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty;
                            elemento = new ElementoListadoFolioAvisoLlegada
                            {
                                FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                  where pc.Entidad == temp.Entidad && pc.Proyecto == temp.ProyectoNombrado
                                                                                  select pc.PreFijoFolioAvisoLlegada + ","
                                                                                  + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                  + temp.Consecutivo.ToString() + ","
                                                                                  + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : temp.FolioAvisoLlegadaID.ToString(),
                                FolioAvisoLlegadaID = temp.FolioAvisoLlegadaID.ToString(),
                                FechaGeneracion = fechag,
                                FechaRecepcion = fechaR
                            };
                            if (activarFolioConfiguracion)
                            {
                                string[] elementos = elemento.FolioConfiguracion.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elementos[1]);
                                int consecutivo = Convert.ToInt32(elementos[2]);
                                string formato = "D" + digitos.ToString();
                                elemento.FolioConfiguracion = elementos[0].Trim() + consecutivo.ToString(formato) + elementos[3];
                            }
                            else 
                            {
                                elemento.FolioConfiguracion = elemento.FolioConfiguracion.PadLeft(maxpadleft, '0');
                            }
                        }
                        else
                        {
                            Sam3_FolioAvisoLlegada temp = (from r in ctx.Sam3_FolioAvisoLlegada
                                                           where r.Activo == true
                                                           && r.FolioAvisoLlegadaID == folio
                                                           select r).AsParallel().SingleOrDefault();

                            string fechaR = temp.FechaRecepcion.HasValue ? temp.FechaRecepcion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty;
                            string fechag = temp.FechaModificacion.HasValue ? temp.FechaModificacion.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty;

                            elemento = new ElementoListadoFolioAvisoLlegada
                            {
                                FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                  where pc.Entidad == temp.Entidad && pc.Proyecto == temp.ProyectoNombrado
                                                                                  select pc.PreFijoFolioAvisoLlegada + ","
                                                                                  + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                  + temp.Consecutivo.ToString() + ","
                                                                                  + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : temp.FolioAvisoLlegadaID.ToString(),
                                FolioAvisoLlegadaID = temp.FolioAvisoLlegadaID.ToString(),
                                FechaGeneracion = fechag,
                                FechaRecepcion = fechaR
                            };
                            if (activarFolioConfiguracion)
                            {
                                string[] elementos = elemento.FolioConfiguracion.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elementos[1]);
                                int consecutivo = Convert.ToInt32(elementos[2]);
                                string formato = "D" + digitos.ToString();
                                elemento.FolioConfiguracion = elementos[0].Trim() + consecutivo.ToString(formato) + elementos[3];
                            }
                            else
                            {
                                elemento.FolioConfiguracion = elemento.FolioConfiguracion.PadLeft(maxpadleft, '0');
                            }
                        }


                        if (elemento != null)
                        {
                            resultados.Add(elemento);
                        }

                    }

                    return resultados;
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

        public List<ListaCombos> ObtenerListadoFolioConfiguracion()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaCombos> lstFolios = (from r in ctx.Sam3_FolioAvisoLlegada
                                                   where r.Activo
                                                   select new ListaCombos
                                                  {
                                                      id = r.FolioAvisoLlegadaID.ToString(),
                                                      value = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                               where pc.Entidad == r.Entidad && pc.Proyecto == r.ProyectoNombrado
                                                               select pc.PreFijoFolioAvisoLlegada + ","
                                                                + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                + r.Consecutivo.ToString() + ","
                                                                + pc.PostFijoFolioAvisoLlegada).FirstOrDefault()
                                                  }).AsParallel().ToList();

                    foreach (ListaCombos lst in lstFolios)
                    {
                        if (lst.value != null)
                        {
                            string[] elemntos = lst.value.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int consecutivo = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                        }

                    }

                    return lstFolios;
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
                return null;
            }
        }
        /// <summary>
        /// Obtiene la cuadrilla de descarga de un aviso de llegada
        /// </summary>
        /// <param name="avisoLlegadaID"></param>
        /// <returns>Cuadrilla de descarga</returns>
        public object ObtenerCuadrillaDescarga(string avisoLlegadaID)
        {
            try
            {
                int avisoID = avisoLlegadaID != "" ? Convert.ToInt32(avisoLlegadaID) : 0;
                using (SamContext ctx = new SamContext())
                {
                    AvisoLlegadaJson aviso = new AvisoLlegadaJson();

                    Sam3_FolioAvisoLlegada registroBd = ctx.Sam3_FolioAvisoLlegada.Where(x =>
                        x.FolioAvisoLlegadaID == avisoID && x.Activo)
                        .AsParallel().SingleOrDefault();

                    aviso.CuadrillaDescarga = registroBd.CuadrillaDescarga;
                    return aviso;
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
        /// Obtiene el detalle de un aviso de Llegada
        /// </summary>
        /// <param name="avisoLlegadaID"></param>
        /// <returns>JSON con las propiedades para el detalle de aviso de llegada</returns>
        public object ObtenerAvisoLlegadaPorID(int avisoLlegadaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ?
                        (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    AvisoLlegadaJson aviso = new AvisoLlegadaJson();

                    Sam3_FolioAvisoLlegada registroBd = ctx.Sam3_FolioAvisoLlegada.Where(x =>
                        x.FolioAvisoLlegadaID == avisoLlegadaID && x.Activo)
                        .AsParallel().SingleOrDefault();

                    aviso.FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                            where pc.Entidad == registroBd.Entidad && pc.Proyecto == registroBd.ProyectoNombrado
                                                                            select pc.PreFijoFolioAvisoLlegada + ","
                                                                             + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                             + registroBd.Consecutivo.ToString() + ","
                                                                             + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : registroBd.FolioAvisoLlegadaID.ToString();
                    if (activarFolioConfiguracion)
                    {
                        string[] elemntos = aviso.FolioConfiguracion.Split(',').ToArray();
                        int digitos = Convert.ToInt32(elemntos[1]);
                        int consecutivo = Convert.ToInt32(elemntos[2]);
                        string formato = "D" + digitos.ToString();

                        aviso.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                    }

                    //agregamos el listado de archivos del aviso de llegada
                    aviso.Archivos = (from r in ctx.Sam3_Rel_FolioAvisoLlegada_Documento
                                      join t in ctx.Sam3_TipoArchivo on r.TipoArchivoID equals t.TipoArchivoID
                                      where r.Activo && r.FolioAvisoLlegadaID == registroBd.FolioAvisoLlegadaID
                                      select new ArchivosAV
                                      {
                                          ArchivoID = r.DocumentoID,
                                          Extension = r.Extencion,
                                          Nombre = r.Nombre,
                                          Url = r.Url,
                                          TipoArchivo = t.Nombre
                                      }).ToList();

                    //agregamog los choferes
                    aviso.Chofer = (from r in ctx.Sam3_Chofer
                                    where r.ChoferID == registroBd.ChoferID && r.Activo
                                    select new ChoferAV { ChoferID = r.ChoferID, Nombre = r.Nombre }).AsParallel().ToList();

                    aviso.FechaRecepcion = registroBd.FechaRecepcion.HasValue ? registroBd.FechaRecepcion.Value.ToString("yyyy-MM-dd") : string.Empty;
                    aviso.FolioAvisoLlegadaID = registroBd.FolioAvisoLlegadaID;
                    aviso.Estatus = registroBd.Estatus;

                    TractoAV tractoBd = (from r in ctx.Sam3_Vehiculo
                                         where r.Activo
                                         && r.VehiculoID == registroBd.VehiculoID
                                         select new TractoAV
                                         {
                                             VehiculoID = r.VehiculoID.ToString(),
                                             Placas = r.Placas
                                         }).AsParallel().SingleOrDefault();

                    aviso.Tracto = tractoBd != null ? tractoBd : new TractoAV { VehiculoID = "0", Placas = string.Empty };



                    TipoAvisoAV tipoAvisoBD = (from r in ctx.Sam3_TipoAviso
                                               where r.TipoAvisoID == registroBd.TipoAvisoID
                                               select new TipoAvisoAV
                                               {
                                                   Nombre = r.Nombre,
                                                   TipoAvisoID = r.TipoAvisoID.ToString()
                                               }).AsParallel().SingleOrDefault();

                    aviso.TipoAviso = tipoAvisoBD != null ? tipoAvisoBD : new TipoAvisoAV { Nombre = string.Empty, TipoAvisoID = "0" };

                    //en el registro se encuentra el id del cliente en sam3, pero el detalle requiere del id del cliente en sam2
                    int clienteId = 0;
                    string nombre = (from c in ctx.Sam3_Cliente
                                     where c.ClienteID == registroBd.ClienteID
                                     select c.Nombre).SingleOrDefault();

                    //if (nombre == "Cliente Default")
                    //{
                    //    clienteId = (from c in ctx.Sam3_Cliente
                    //                 where c.ClienteID == registroBd.ClienteID
                    //                 select c.ClienteID).SingleOrDefault();
                    //}
                    //else
                    //{
                    //clienteId = (from c in ctx.Sam3_Cliente
                    //             where c.Activo && c.ClienteID == registroBd.ClienteID.Value
                    //             select c.Sam2ClienteID.Value).AsParallel().SingleOrDefault(); //registroBd.ClienteID.Value;
                    //}
                    aviso.Cliente = (from c in ctx.Sam3_Cliente
                                     where c.Activo && c.ClienteID == registroBd.ClienteID.Value
                                     select new Models.Cliente
                                     {
                                         ClienteID = c.Sam2ClienteID.ToString(),
                                         Nombre = c.Nombre
                                     }).AsParallel().SingleOrDefault();

                    //aviso.Cliente = (Models.Cliente)ClienteBd.Instance.ObtnerElementoClientePorID(clienteId);

                    //agregamos los patios
                    aviso.Patio = (from r in ctx.Sam3_Patio
                                   where r.PatioID == registroBd.PatioID && r.Activo
                                   select new PatioAV
                                   {
                                       Nombre = r.Nombre,
                                       PatioID = r.PatioID,
                                       RequierePermisoAduana = r.RequierePermisoAduana
                                   }).AsParallel().ToList();

                    //agregar permisos de aduana
                    //primero obtenemos los archivos de permisos de aduana
                    List<Sam3_PermisoAduana> lstpermisosAduana = ctx.Sam3_PermisoAduana
                        .Where(x => x.FolioAvisoLlegadaID == registroBd.FolioAvisoLlegadaID && x.Activo)
                        .AsParallel().ToList();

                    foreach (Sam3_PermisoAduana p in lstpermisosAduana)
                    {
                        //List<ArchivoAutorizadoAV> lstarchivosPermisoAduana = (from r in ctx.Sam3_Rel_PermisoAduana_Documento
                        //                                                      where r.PermisoAduanaID == p.PermisoAduanaID && r.Activo
                        //                                                      select new ArchivoAutorizadoAV
                        //                                                      {
                        //                                                          ArchivoID = r.DocumentoID,
                        //                                                          Extension = r.Extencion,
                        //                                                          Nombre = r.Nombre
                        //                                                      }).AsParallel().ToList();

                        aviso.PermisoAduana.Add(new PermisoAduanaAV
                        {
                            NumeroPermiso = p.NumeroPermiso.ToString(),
                            PermisoAutorizado = p.PermisoAutorizado,
                            PermisoTramite = p.PermisoTramite,
                            FechaAutorizacion = p.FechaAutorización.ToString(),
                            FechaGeneracion = p.FechaGeneracion.ToString()
                        });
                    }

                    if (aviso.PermisoAduana.Count <= 0)
                    {
                        aviso.PermisoAduana.Add(new PermisoAduanaAV
                        {
                            NumeroPermiso = string.Empty,
                            PermisoAutorizado = false,
                            PermisoTramite = false,
                            FechaAutorizacion = string.Empty,
                            FechaGeneracion = string.Empty
                        });
                    }

                    aviso.Plana = (from r in ctx.Sam3_Vehiculo
                                   join p in ctx.Sam3_Rel_FolioAvisoLlegada_Vehiculo on r.VehiculoID equals p.VehiculoID
                                   where p.FolioAvisoLlegadaID == registroBd.FolioAvisoLlegadaID && r.Activo && p.Activo
                                   select new PlanaAV
                                   {
                                       PlanaID = r.VehiculoID
                                   }).AsParallel().ToList();

                    aviso.Proyectos = (from r in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                       where r.FolioAvisoLlegadaID == registroBd.FolioAvisoLlegadaID && r.Activo
                                       select new ProyectosAV
                                       {
                                           ProyectoID = r.ProyectoID
                                       }).AsParallel().ToList();

                    aviso.Transportista = (from r in ctx.Sam3_Transportista
                                           where r.TransportistaID == registroBd.TransportistaID && r.Activo
                                           select new TransportistaAV
                                           {
                                               Nombre = r.Nombre,
                                               TransportistaID = r.TransportistaID
                                           }).AsParallel().ToList();

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(aviso);

                    return aviso;
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
        /// Elimina logicamente un aviso de llegada
        /// </summary>
        /// <param name="avisoLlegadaID"></param>
        /// <param name="usuario"></param>
        /// <returns>Aviso de exito o error</returns>
        public object EliminarAvisoLlegada(int avisoLlegadaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_FolioAvisoLlegada aviso = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == avisoLlegadaID)
                        .AsParallel().SingleOrDefault();


                    aviso.Activo = false;
                    aviso.UsuarioModificacion = usuario.UsuarioID;
                    aviso.FechaModificacion = DateTime.Now;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Actualiza la informacion de un aviso de llegada en especifico
        /// </summary>
        /// <param name="cambios"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ActualizarAvisoLlegada(AvisoLlegadaJson cambios, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        bool tienePermisoAduana = false;

                        Sam3_FolioAvisoLlegada avisoBd = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == cambios.FolioAvisoLlegadaID)
                            .AsParallel().SingleOrDefault();
                        if (avisoBd != null)
                        {
                            //modificamos los parametros generales
                            avisoBd.Activo = true;
                            avisoBd.ChoferID = cambios.Chofer.Count > 0 ? cambios.Chofer[0].ChoferID : 1;
                            avisoBd.FechaModificacion = DateTime.Now;
                            avisoBd.FechaRecepcion = Convert.ToDateTime(cambios.FechaRecepcion);
                            avisoBd.PatioID = cambios.Patio.Count > 0 ? cambios.Patio[0].PatioID : 1;
                            avisoBd.TransportistaID = cambios.Transportista.Count > 0 ? cambios.Transportista[0].TransportistaID : 1;
                            avisoBd.UsuarioModificacion = usuario.UsuarioID;
                            avisoBd.VehiculoID = cambios.Tracto.VehiculoID != null ? Convert.ToInt32(cambios.Tracto.VehiculoID) : 1;
                            //los datos de entrada traen el id del cliente en sam2
                            int sam2ClienteID = cambios.Cliente.ClienteID != string.Empty || cambios.Cliente.ClienteID != "-1"
                                ? Convert.ToInt32(cambios.Cliente.ClienteID) : 0;
                            //insertamos en el id del cliente en sam3
                            avisoBd.ClienteID = (from c in ctx.Sam3_Cliente
                                                 where c.Activo && c.Sam2ClienteID == sam2ClienteID
                                                 select c.ClienteID).AsParallel().SingleOrDefault();
                            avisoBd.TipoAvisoID = cambios.TipoAviso.TipoAvisoID != null ? Convert.ToInt32(cambios.TipoAviso.TipoAvisoID) : 1;
                            avisoBd.PaseSalidaEnviado = cambios.PaseSalidaEnviado;

                            List<int> iPlanas = cambios.Plana.Select(x => x.PlanaID).ToList();

                            List<Sam3_Rel_FolioAvisoLlegada_Vehiculo> aviso = (from av in ctx.Sam3_Rel_FolioAvisoLlegada_Vehiculo
                                                                               where av.FolioAvisoLlegadaID == cambios.FolioAvisoLlegadaID && !iPlanas.Contains(av.VehiculoID) && av.Activo
                                                                               select av).AsParallel().ToList();

                            foreach (Sam3_Rel_FolioAvisoLlegada_Vehiculo av in aviso)
                            {
                                av.Activo = false;
                                av.FechaModificacion = DateTime.Now;
                                av.UsuarioModificacion = usuario.UsuarioID;
                            }

                            //Actualizar informacion de las planas
                            foreach (PlanaAV plana in cambios.Plana)
                            {
                                if (!avisoBd.Sam3_Rel_FolioAvisoLlegada_Vehiculo
                                    .Where(x => x.VehiculoID == plana.PlanaID && x.FolioAvisoLlegadaID == cambios.FolioAvisoLlegadaID && x.Activo).Any()) // varificamos si existe la plana
                                {
                                    //agregamos una nuevo registro a la relacion de aviso y planas
                                    Sam3_Rel_FolioAvisoLlegada_Vehiculo nuevoRegistro = new Sam3_Rel_FolioAvisoLlegada_Vehiculo();
                                    nuevoRegistro.Activo = true;
                                    nuevoRegistro.FechaModificacion = DateTime.Now;
                                    nuevoRegistro.FolioAvisoLlegadaID = avisoBd.FolioAvisoLlegadaID;
                                    nuevoRegistro.VehiculoID = plana.PlanaID;
                                    nuevoRegistro.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_FolioAvisoLlegada_Vehiculo.Add(nuevoRegistro);
                                }
                            }

                            //actualizar la información de los documentos de Aviso de llegada
                            //foreach (ArchivosAV archivo in cambios.Archivos)
                            //{
                            //    //verificamos si ya existe el archivo actual
                            //    if (!ctx.Sam3_Rel_FolioAvisoLlegada_Documento
                            //        .Where(x => x.Rel_FolioAvisoLlegada_DocumentoID == archivo.ArchivoID
                            //            && x.FolioAvisoLlegadaID == avisoBd.FolioAvisoLlegadaID).Any())
                            //    {
                            //        //si el archivo no existe, agregamos uno nuevo
                            //        Sam3_Rel_FolioAvisoLlegada_Documento nuenoDoc = new Sam3_Rel_FolioAvisoLlegada_Documento();
                            //        nuenoDoc.Activo = true;
                            //        nuenoDoc.DocumentoID = archivo.ArchivoID;
                            //        nuenoDoc.Extencion = archivo.Extension;
                            //        nuenoDoc.FechaModificacion = DateTime.Now;
                            //        nuenoDoc.FolioAvisoLlegadaID = avisoBd.FolioAvisoLlegadaID;
                            //        nuenoDoc.Nombre = archivo.Nombre;
                            //        nuenoDoc.UsuarioModificacion = usuario.UsuarioID;

                            //        ctx.Sam3_Rel_FolioAvisoLlegada_Documento.Add(nuenoDoc);
                            //    }
                            //}

                            List<Sam3_Rel_FolioAvisoLlegada_Proyecto> lstProyectos = ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                    .Where(x => x.FolioAvisoLlegadaID == cambios.FolioAvisoLlegadaID).ToList();

                            if (cambios.Proyectos.Count > 0)
                            {
                                List<Sam3_Rel_FolioAvisoLlegada_Proyecto> eliminados = lstProyectos.Where(x => !cambios.Proyectos.Select(y => y.ProyectoID).Contains(x.ProyectoID)).ToList();

                                foreach (Sam3_Rel_FolioAvisoLlegada_Proyecto item in eliminados)
                                {
                                    item.Activo = false;
                                    ctx.SaveChanges();
                                }
                                //ctx.SaveChanges();

                                foreach (ProyectosAV proyecto in cambios.Proyectos)
                                {
                                    //verificamos si existe el registro
                                    if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.ProyectoID == proyecto.ProyectoID
                                        && x.FolioAvisoLlegadaID == cambios.FolioAvisoLlegadaID
                                        && x.Activo).Any())
                                    {
                                        Sam3_Rel_FolioAvisoLlegada_Proyecto nuevoProyecto = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                        nuevoProyecto.Activo = true;
                                        nuevoProyecto.FechaModificacion = DateTime.Now;
                                        nuevoProyecto.FolioAvisoLlegadaID = cambios.FolioAvisoLlegadaID;
                                        nuevoProyecto.ProyectoID = proyecto.ProyectoID;
                                        nuevoProyecto.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevoProyecto);
                                    }
                                }
                            }

                            avisoBd.UsuarioModificacion = usuario.UsuarioID;
                            avisoBd.FechaModificacion = DateTime.Now;

                            //si no tiene llegada de material:
                            // se actualiza el estatus del folio aviso llegada.
                            //permisos tramite o autorizados en inactivo para poder reenviar permiso Issue #235
                            if (!ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoLlegadaID == avisoBd.FolioAvisoLlegadaID).Any())
                            {

                                if (ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == avisoBd.FolioAvisoLlegadaID && x.Activo).Any())
                                {
                                    tienePermisoAduana = true;
                                    Sam3_PermisoAduana permisoBd = ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == avisoBd.FolioAvisoLlegadaID && x.Activo)
                                    .AsParallel().SingleOrDefault();
                                    permisoBd.FechaAutorización = DateTime.Now;
                                    permisoBd.FechaModificacion = DateTime.Now;
                                    permisoBd.UsuarioModificacion = usuario.UsuarioID;
                                    permisoBd.Activo = false;
                                    ctx.SaveChanges();

                                    int permisoID = (permisoBd.PermisoAduanaID);

                                    if (ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID == permisoID && x.Activo).Any())
                                    {
                                        tienePermisoAduana = true;
                                        foreach (Sam3_Rel_PermisoAduana_Documento documentos in ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID == permisoID && x.Activo))
                                        {
                                            documentos.Activo = false;
                                            documentos.FechaModificacion = DateTime.Now;
                                            documentos.FechaModificacion = DateTime.Now;
                                            documentos.UsuarioModificacion = usuario.UsuarioID;
                                        }
                                        ctx.SaveChanges();
                                    }

                                }



                            }

                            if (tienePermisoAduana)
                            {
                                avisoBd.Estatus = "Generado";
                            }

                            ctx.SaveChanges();


                        }
                        sam3_tran.Commit();
                    }
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Obtinene un listado de tipo ID, Valor. Para mostarse en un combobox. Muestra todos los folios activos
        /// </summary>
        /// <returns></returns>
        public object ObtenerListadoFoliosParaFiltro(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    //Folios que aun no tienen relacionado una orden de recepcion
                    //List<ListaCombos> lstFolios = new List<ListaCombos>();
                    List<ListaCombos> lstFolios = (from r in ctx.Sam3_FolioAvisoLlegada
                                                   join fe in ctx.Sam3_FolioAvisoEntrada on r.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                                   join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                   join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                   where r.Activo && fp.Activo && p.Activo && pa.Activo
                                                   && proyectos.Contains(p.ProyectoID)
                                                   && patios.Contains(pa.PatioID)
                                                   && patios.Contains(r.PatioID)
                                                   && patios.Contains(fe.PatioID)
                                                   select new ListaCombos
                                                  {
                                                      id = r.FolioAvisoLlegadaID.ToString(),
                                                      value = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                           where pc.Entidad == r.Entidad && pc.Proyecto == r.ProyectoNombrado
                                                                                           select pc.PreFijoFolioAvisoLlegada + ","
                                                                                            + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                            + r.Consecutivo.ToString() + ","
                                                                                            + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : r.FolioAvisoLlegadaID.ToString()
                                                  }).AsParallel().ToList();

                    lstFolios = lstFolios.GroupBy(x => x.id).Select(x => x.First()).ToList();

                    if (activarFolioConfiguracion)
                    {
                        foreach (ListaCombos lst in lstFolios)
                        {
                            if (lst.value != null)
                            {
                                string[] elemntos = lst.value.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }

                        }
                    }

#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(lstFolios);
#endif

                    return lstFolios;
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
        /// Obtener los folios de Aviso llegada que no tienen entrada de material
        /// </summary>
        /// <returns></returns>
        public object ObtenerFoliosAvisoLlegadaSinEntrada(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"])
                        ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;

                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    //List<int> idsAvisosEntrada = (from fe in ctx.Sam3_FolioAvisoEntrada
                    //                              where fe.Activo
                    //                              && patios.Contains(fe.PatioID)
                    //                              select fe.FolioAvisoLlegadaID.Value).Distinct().AsParallel().ToList();

                    List<ListaCombos> lstFolios = (from r in ctx.Sam3_FolioAvisoLlegada
                                                   join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                   join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                   where r.Activo && fp.Activo && p.Activo && pa.Activo
                                                       //&& !(from av in ctx.Sam3_FolioAvisoEntrada
                                                       //     where av.Activo
                                                       //     select av.FolioAvisoLlegadaID.Value).Contains(r.FolioAvisoLlegadaID)
                                                   && proyectos.Contains(p.ProyectoID)
                                                   && patios.Contains(pa.PatioID)
                                                   && patios.Contains(r.PatioID)
                                                   select new ListaCombos
                                                   {
                                                       id = r.FolioAvisoLlegadaID.ToString(),
                                                       value = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                            where pc.Entidad == r.Entidad && pc.Proyecto == r.ProyectoNombrado
                                                                                            select pc.PreFijoFolioAvisoLlegada + ","
                                                                                             + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                             + r.Consecutivo.ToString() + ","
                                                                                             + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : r.FolioAvisoLlegadaID.ToString()
                                                   }).AsParallel().GroupBy(g => g.id).Select(grp => grp.First()).OrderBy(x => x.id).ToList();

                    if (activarFolioConfiguracion)
                    {
                        foreach (ListaCombos lst in lstFolios)
                        {
                            if (lst.value != null)
                            {
                                string[] elemntos = lst.value.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }

                        }
                    }
                    return lstFolios;
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
        /// Devuelve un listado de tipo ID, Valor. Con los Folios de llegada que ya tienen folio de descarga.
        /// </summary>
        /// <returns></returns>
        public object ObtenerListadoSinPaseSalida(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;

                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<ListaCombos> lstFolios = (from r in ctx.Sam3_FolioAvisoLlegada
                                                   join m in ctx.Sam3_FolioAvisoEntrada on r.FolioAvisoLlegadaID equals m.FolioAvisoLlegadaID
                                                   join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                   join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                   where r.Activo && m.Activo && fp.Activo && p.Activo && pa.Activo
                                                   && proyectos.Contains(p.ProyectoID)
                                                   && patios.Contains(pa.PatioID)
                                                   && patios.Contains(r.PatioID)
                                                   && patios.Contains(m.PatioID)
                                                   && m.FolioDescarga > 0
                                                   select new ListaCombos
                                                   {
                                                       id = r.FolioAvisoLlegadaID.ToString(),
                                                       value = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                            where pc.Entidad == r.Entidad && pc.Proyecto == r.ProyectoNombrado
                                                                                            select pc.PreFijoFolioAvisoLlegada + ","
                                                                                             + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                             + r.Consecutivo.ToString() + ","
                                                                                             + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : r.FolioAvisoLlegadaID.ToString()
                                                   }).AsParallel().ToList();

                    lstFolios = lstFolios.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.value).ToList();
                    
                    if (activarFolioConfiguracion)
                    {
                        foreach (ListaCombos lst in lstFolios)
                        {
                            if (lst.value != null)
                            {
                                string[] elemntos = lst.value.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }

                        }
                    }
                    return lstFolios;
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
        /// Devuelve un listado de tipo ID, Valor. Con los folios de llegada que requieren de permiso de aduana
        /// </summary>
        /// <returns></returns>
        public object ObtenerListadoFoliosRequierePermiso(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;

                    List<int> proyectos;
                    List<int> patios;

                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<ListaCombos> lstFolios = (from r in ctx.Sam3_FolioAvisoLlegada
                                                   join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                                                   join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                   join pe in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals pe.FolioAvisoLlegadaID
                                                   where r.Activo && pe.Activo && fp.Activo && p.Activo && pa.Activo
                                                   && pa.RequierePermisoAduana
                                                   && proyectos.Contains(p.ProyectoID)
                                                   && patios.Contains(pa.PatioID)
                                                   && patios.Contains(r.PatioID)
                                                   select new ListaCombos
                                                   {
                                                       id = r.FolioAvisoLlegadaID.ToString(),
                                                       value = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                            where pc.Entidad == r.Entidad && pc.Proyecto == r.ProyectoNombrado
                                                                                            select pc.PreFijoFolioAvisoLlegada + ","
                                                                                             + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                             + r.Consecutivo.ToString() + ","
                                                                                             + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : r.FolioAvisoLlegadaID.ToString()
                                                   }).AsParallel().ToList();

                    if (activarFolioConfiguracion)
                    {
                        foreach (ListaCombos lst in lstFolios)
                        {
                            if (lst.value != null)
                            {
                                string[] elemntos = lst.value.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                        }
                    }

                    return lstFolios;
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
        /// Verifica si un folio de llegada requiere de un permiso de aduana.
        /// </summary>
        /// <param name="folioAvisoLlegadaID"></param>
        /// <param name="usuario"></param>
        /// <returns>true  / false</returns>
        public object VerificarPermisoAduana(int folioAvisoLlegadaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    bool permisoAutorizado = (from r in ctx.Sam3_FolioAvisoLlegada
                                              join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                              join py in ctx.Sam3_Proyecto on rfp.ProyectoID equals py.ProyectoID
                                              join pa in ctx.Sam3_Patio on py.PatioID equals pa.PatioID
                                              join p in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                              where r.FolioAvisoLlegadaID == folioAvisoLlegadaID && p.PermisoAutorizado == true
                                              && patios.Contains(r.PatioID)
                                              && proyectos.Contains(py.ProyectoID)
                                              && patios.Contains(pa.PatioID)
                                              && patios.Contains(r.PatioID)
                                              && r.Activo && rfp.Activo && py.Activo && pa.Activo && p.Activo
                                              select p.PermisoAduanaID).AsParallel().Any();

                    return permisoAutorizado;
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
        /// Verifica si un Folio tiene documento de permiso de aduana, si lo tiene, devuelve la información del documento, 
        /// si no existe devuelve un null.
        /// </summary>
        /// <param name="folioAvisoLlegadaID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object VerificarDocumentoAduana(int folioAvisoLlegadaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    DocumentoPermisoAduana permiso = (from r in ctx.Sam3_PermisoAduana
                                                      //join d in ctx.Sam3_Rel_PermisoAduana_Documento on r.PermisoAduanaID equals d.PermisoAduanaID
                                                      //join p in ctx.Sam3_PermisoAduana on d.PermisoAduanaID equals p.PermisoAduanaID
                                                      //join t in ctx.Sam3_TipoArchivo on d.TipoArchivoID equals t.TipoArchivoID
                                                      where r.FolioAvisoLlegadaID == folioAvisoLlegadaID && r.Activo
                                                      select new DocumentoPermisoAduana
                                                      {
                                                          PermisoID = r.PermisoAduanaID.ToString(),
                                                          //DocumentoID = d.Rel_Permiso_Documento_ID.ToString(),
                                                          NumeroPermiso = r.NumeroPermiso.ToString(),
                                                          PermisoAutorizado = r.PermisoAutorizado,
                                                          //Url = d.Url,
                                                          //Nombre = d.Nombre,
                                                          //TipoArchivo = t.Nombre,
                                                          //Extencion = d.Extencion,
                                                      }).AsParallel().SingleOrDefault();

                    //if (ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID.ToString() == permiso.PermisoID).Count() == 1)
                    //{
                    permiso.DocumentoID = (from d in ctx.Sam3_Rel_PermisoAduana_Documento
                                           where d.PermisoAduanaID.ToString() == permiso.PermisoID
                                           && d.Activo
                                           select d.Rel_Permiso_Documento_ID.ToString()).AsParallel().FirstOrDefault();

                    permiso.Url = (from d in ctx.Sam3_Rel_PermisoAduana_Documento
                                   where d.PermisoAduanaID.ToString() == permiso.PermisoID
                                           && d.Activo
                                   select d.Url).AsParallel().FirstOrDefault();

                    permiso.Nombre = (from d in ctx.Sam3_Rel_PermisoAduana_Documento
                                      where d.PermisoAduanaID.ToString() == permiso.PermisoID
                                           && d.Activo
                                      select d.Nombre).AsParallel().FirstOrDefault();

                    permiso.TipoArchivo = (from d in ctx.Sam3_Rel_PermisoAduana_Documento
                                           join t in ctx.Sam3_TipoArchivo on d.TipoArchivoID equals t.TipoArchivoID
                                           where d.PermisoAduanaID.ToString() == permiso.PermisoID
                                           && d.Activo && t.Activo
                                           select t.Nombre).AsParallel().FirstOrDefault();

                    permiso.Extencion = (from d in ctx.Sam3_Rel_PermisoAduana_Documento
                                         where d.PermisoAduanaID.ToString() == permiso.PermisoID
                                           && d.Activo
                                         select d.Extencion).AsParallel().FirstOrDefault();
                    //}
                    //else
                    //{

                    //}

                    return permiso;
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

        public List<ListadoIncidencias> ListadoInciendias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    //obtenemos todos los avisos de llegada segun los datos propuestos
                    List<Sam3_FolioAvisoLlegada> registros = (from fa in ctx.Sam3_FolioAvisoLlegada
                                                              join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                                              join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                                              join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                              where fa.Activo && rfp.Activo
                                                              && proyectos.Contains(rfp.ProyectoID)
                                                              && patios.Contains(pa.PatioID)
                                                              && patios.Contains(fa.PatioID)
                                                              && IDs.Contains(fa.FolioAvisoLlegadaID)
                                                              select fa).Distinct().AsParallel().ToList();

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = registros.Where(x => x.ClienteID == sam3Cliente).ToList();
                    }

                    if (proyectoID > 0)
                    {
                        registros = (from r in registros
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     where r.Activo && rfp.Activo
                                     && rfp.ProyectoID == proyectoID
                                     select r).Distinct().AsParallel().ToList();
                    }



                    listado = (from r in registros
                               join rif in ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada on r.FolioAvisoLlegadaID equals rif.FolioAvisoLlegadaID
                               join incd in ctx.Sam3_Incidencia on rif.IncidenciaID equals incd.IncidenciaID
                               join cls in ctx.Sam3_ClasificacionIncidencia on incd.ClasificacionID equals cls.ClasificacionIncidenciaID
                               join ti in ctx.Sam3_TipoIncidencia on incd.TipoIncidenciaID equals ti.TipoIncidenciaID
                               join us in ctx.Sam3_Usuario on incd.UsuarioID equals us.UsuarioID
                               where r.Activo && rif.Activo && incd.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = cls.Nombre,
                                   Estatus = incd.Estatus,
                                   FechaRegistro = incd.FechaCreacion.ToString(),
                                   FolioIncidenciaID = incd.IncidenciaID.ToString(),
                                   RegistradoPor = us.Nombre + " " + us.ApellidoPaterno,
                                   TipoIncidencia = ti.Nombre,
                                   FolioOriginalID = incd.IncidenciaOriginalID.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == incd.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + incd.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : incd.IncidenciaID.ToString()
                               }).Distinct().AsParallel().ToList();

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
                            else
                            {
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

    }//Fin Clase
}