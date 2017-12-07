using BackEndSAM.Models;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces.IncidenciasBD
{
    public class ListadoIncidenciasBD
    {
        private static readonly object _mutex = new Object();
        private static ListadoIncidenciasBD _instance;

        public static ListadoIncidenciasBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ListadoIncidenciasBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtenerListadoProyectos(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<int> patios;
                    List<int> proyectos;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<Proyecto> lstProyectos = (from r in ctx.Sam3_Proyecto
                                                   join c in ctx.Sam3_Cliente on r.ClienteID equals c.ClienteID
                                                   join pc in ctx.Sam3_ProyectoConfiguracion on r.ProyectoID equals pc.ProyectoID
                                                   where r.Activo && proyectos.Contains(r.ProyectoID) && pc.RequiereIncidenciaBilingue == true
                                                   select new Proyecto
                                                   {
                                                       Nombre = r.Nombre,
                                                       ProyectoID = r.ProyectoID.ToString(),
                                                       ClienteID = c.Sam2ClienteID.ToString()
                                                   }).AsParallel().ToList();

                    lstProyectos = lstProyectos.GroupBy(x => x.ProyectoID).Select(x => x.First()).ToList();
                    return lstProyectos;
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

        public object InsertarCaptura(DataTable dtDetalleCaptura, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }};
                    _SQL.Ejecuta(Stords.GuardarListadoIncidenciasBilingues, dtDetalleCaptura, "@ListadoIncidencias", parametro);
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
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }



        public object ObtieneListadoIncidenciasBilingues(Sam3_Usuario usuario, string lenguaje, int ProyectoID, string FechaInicial, string FechaFinal, string EstatusID, int Mostrar)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DateTime fechaInicial = new DateTime();
                    DateTime fechaFinal = new DateTime();
                    DateTime.TryParse(FechaInicial, out fechaInicial);
                    DateTime.TryParse(FechaFinal, out fechaFinal);

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

                    int proyectoID = ProyectoID != 0 ? Convert.ToInt32(ProyectoID) : 0;
                    int clienteID = 0;

                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    //Primero obtengo todas las incidencias activas dentro del rango de tiempo
                    List<Sam3_Incidencia> registrosIncidencias = (from incidencia in ctx.Sam3_Incidencia
                                                                  where incidencia.Activo
                                                                  && (incidencia.FechaCreacion >= fechaInicial && incidencia.FechaCreacion <= fechaFinal)
                                                                  && incidencia.Estatus == EstatusID
                                                                  select incidencia).Distinct().AsParallel().ToList();

                    List<int> incidenciasIDs = registrosIncidencias.Select(x => x.IncidenciaID).Distinct().ToList();

                    List<ListadoIncidencias> listado = new List<ListadoIncidencias>();
                    List<ListadoIncidencias> listaTemporal = new List<Models.ListadoIncidencias>();
                    List<int> temp = new List<int>();

                    //folios aviso de llegada -- OK
                    temp = (from r in ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.FolioAvisoLlegadaID).AsParallel().Distinct().ToList();

                    listaTemporal = ListadoIncidenciasAvisoLlegada(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Entrada de material
                    temp.Clear();
                    listaTemporal.Clear();
                    temp = (from r in ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.FolioAvisoEntradaID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasEntradaMaterial(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Pase salida, no se si existe la incidencia a nivel pase de salida o es de tipo aviso de entrada
                    //listado.AddRange(PaseSalidaBd.Instance.ListadoIncidencias(clienteID, proyectoID, proyectos, patios, incidenciasIDs, fechaInicial, fechaFinal));

                    //Packing list (Folio Cuantificacion)
                    temp.Clear();
                    listaTemporal.Clear();
                    temp = (from r in ctx.Sam3_Rel_Incidencia_FolioCuantificacion
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.FolioCuantificacionID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasPackingList(clienteID, proyectoID, proyectos, patios, temp,
                        fechaInicial, fechaFinal);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Orden recepcion
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.OrdenRecepcionID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasOrdenRecepcion(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Complemento recepcion
                    // N/A

                    //ItemCode
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_ItemCode
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.ItemCodeID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasItemCode(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Orden Almacenaje
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.OrdenalmacenajeID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasOrdenAlmacenaje(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Numero Unico
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_NumeroUnico
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.NumeroUnicoID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasNumeroUnico(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Despacho
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_Despacho
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.DespachoID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasDespacho(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    //Corte
                    temp.Clear();
                    listaTemporal.Clear();

                    temp = (from r in ctx.Sam3_Rel_Incidencia_Corte
                            where r.Activo && incidenciasIDs.Contains(r.IncidenciaID)
                            select r.CorteID).AsParallel().ToList();

                    listaTemporal = ListadoIncidenciasCorte(clienteID, proyectoID, proyectos, patios, temp);

                    if (listaTemporal.Count > 0) { listado.AddRange(listaTemporal); }

                    foreach (ListadoIncidencias l in listado)
                    {
                        DateTime fechaCreacion = new DateTime();
                        DateTime.TryParse(l.FechaRegistro, out fechaCreacion);

                        l.FechaRegistro = fechaCreacion.ToString("yyyy-MM-dd");
                    }
                    listado = (from ls in listado where ls.Estatus == EstatusID select ls).ToList<ListadoIncidencias>();
#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(listado);
#endif
                    foreach (var i in listado)
                    {
                        i.FolioOriginalID = (i.FolioOriginalID == null || i.FolioOriginalID == "") ? i.FolioIncidenciaID : i.FolioOriginalID;
                    }
                    return listado.OrderBy(x => x.FolioOriginalID).ToList();
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
        //folios aviso de llegada -- OK
        public List<ListadoIncidencias> ListadoIncidenciasAvisoLlegada(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_FolioAvisoEntrada> registros = new List<Sam3_FolioAvisoEntrada>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && patios.Contains(fe.PatioID)
                                     && IDs.Contains(fe.FolioAvisoEntradaID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select fe).Distinct().AsParallel().ToList();
                    
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
                                   Titulo = ind.Titulo,
                                   TituloIngles = ind.TituloIngles,
                                   Descripcion = ind.Descripcion,
                                   DescripcionIngles = ind.DescripcionIngles,
                                   DetalleResolucion = ind.DetalleResolucion,
                                   DetalleResolucionIngles = ind.DetalleResolucionIngles,
                                   MotivoCancelacion = ind.MotivoCancelacion,
                                   MotivoCancelacionIngles = ind.MotivoCancelacionIngles,
                                   Respuesta = ind.Respuesta,
                                   RespuestaIngles = ind.RespuestaIngles,
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
                    foreach (var it in listado)
                    {
                        it.FolioOriginalID = (it.FolioOriginalID == "" || it.FolioOriginalID == null) ? it.FolioIncidenciaID : it.FolioOriginalID;
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


        //Entrada de material
        public List<ListadoIncidencias> ListadoIncidenciasEntradaMaterial(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_FolioAvisoEntrada> registros = new List<Sam3_FolioAvisoEntrada>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && patios.Contains(fe.PatioID)
                                     && IDs.Contains(fe.FolioAvisoEntradaID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select fe).Distinct().AsParallel().ToList();
                   

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
                                   Titulo = ind.Titulo,
                                   TituloIngles = ind.TituloIngles,
                                   Descripcion = ind.Descripcion,
                                   DescripcionIngles = ind.DescripcionIngles,
                                   DetalleResolucion = ind.DetalleResolucion,
                                   DetalleResolucionIngles = ind.DetalleResolucionIngles,
                                   MotivoCancelacion = ind.MotivoCancelacion,
                                   MotivoCancelacionIngles = ind.MotivoCancelacionIngles,
                                   Respuesta = ind.Respuesta,
                                   RespuestaIngles = ind.RespuestaIngles,
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
                    foreach (var it in listado)
                    {
                        it.FolioOriginalID = (it.FolioOriginalID == "" || it.FolioOriginalID == null) ? it.FolioIncidenciaID : it.FolioOriginalID;
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

        //Packing list (Folio Cuantificacion)
        public List<ListadoIncidencias> ListadoIncidenciasPackingList(int clienteID, int proyectoID, List<int> proyectos, List<int> patios,
    List<int> foliosCuantificacionIDs, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                List<ListadoIncidencias> listado;
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ?
                    (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_FolioCuantificacion> registros = new List<Sam3_FolioCuantificacion>();
                    
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fc.Activo && fc.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && patios.Contains(fe.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && foliosCuantificacionIDs.Contains(fc.FolioCuantificacionID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select fc).AsParallel().Distinct().ToList();
                 

                    listado = (from r in registros
                               join rfi in ctx.Sam3_Rel_Incidencia_FolioCuantificacion on r.FolioCuantificacionID equals rfi.FolioCuantificacionID
                               join inc in ctx.Sam3_Incidencia on rfi.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rfi.Activo && inc.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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

        //Orden recepcion
        public List<ListadoIncidencias> ListadoIncidenciasOrdenRecepcion(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> Ids)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_OrdenRecepcion> registros = new List<Sam3_OrdenRecepcion>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    
                        registros = (from o in ctx.Sam3_OrdenRecepcion
                                     join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                     join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where o.Activo && rfo.Activo && fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && Ids.Contains(o.OrdenRecepcionID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select o).AsParallel().Distinct().ToList();
                    

                    listado = (from r in registros
                               join rio in ctx.Sam3_Rel_Incidencia_OrdenRecepcion on r.OrdenRecepcionID equals rio.OrdenRecepcionID
                               join inc in ctx.Sam3_Incidencia on rio.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rio.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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


        //ItemCode
        public List<ListadoIncidencias> ListadoIncidenciasItemCode(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> Ids)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ItemCode> registros = new List<Sam3_ItemCode>();

                    
                        registros = (from it in ctx.Sam3_ItemCode
                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                     join p in ctx.Sam3_Proyecto on it.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where it.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && Ids.Contains(rid.Rel_ItemCode_Diametro_ID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select it).AsParallel().Distinct().ToList();
                    

                    listado = (from r in registros
                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                               join riit in ctx.Sam3_Rel_Incidencia_ItemCode on rid.Rel_ItemCode_Diametro_ID equals riit.ItemCodeID
                               join inc in ctx.Sam3_Incidencia on riit.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where riit.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   TipoIncidencia = tpi.Nombre,
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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


        //Orden Almacenaje
        public List<ListadoIncidencias> ListadoIncidenciasOrdenAlmacenaje(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_OrdenAlmacenaje> registros = new List<Sam3_OrdenAlmacenaje>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    
                        registros = (from oa in ctx.Sam3_OrdenAlmacenaje
                                     join ron in ctx.Sam3_Rel_OrdenAlmacenaje_NumeroUnico on oa.OrdenAlmacenajeID equals ron.OrdenAlmacenajeID
                                     join nu in ctx.Sam3_NumeroUnico on ron.NumeroUnicoID equals nu.NumeroUnicoID
                                     join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where oa.Activo && ron.Activo && nu.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(oa.OrdenAlmacenajeID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select oa).AsParallel().Distinct().ToList();

                


                    listado = (from r in registros
                               join rio in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje on r.OrdenAlmacenajeID equals rio.OrdenalmacenajeID
                               join inc in ctx.Sam3_Incidencia on rio.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rio.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   Estatus = inc.Estatus,
                                   Clasificacion = c.Nombre,
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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


        //Numero Unico
        public List<ListadoIncidencias> ListadoIncidenciasNumeroUnico(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_NumeroUnico> registros = new List<Sam3_NumeroUnico>();

                    
                        registros = (from nu in ctx.Sam3_NumeroUnico
                                     join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where nu.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(nu.NumeroUnicoID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select nu).AsParallel().Distinct().ToList();
                    

                    listado = (from r in registros
                               join rin in ctx.Sam3_Rel_Incidencia_NumeroUnico on r.NumeroUnicoID equals rin.NumeroUnicoID
                               join inc in ctx.Sam3_Incidencia on rin.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rin.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   TipoIncidencia = tpi.Nombre,
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();


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


        //Despacho
        public List<ListadoIncidencias> ListadoIncidenciasDespacho(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Despacho> registros = new List<Sam3_Despacho>();

                    
                        registros = (from d in ctx.Sam3_Despacho
                                     join p in ctx.Sam3_Proyecto on d.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where d.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(d.DespachoID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select d).AsParallel().Distinct().ToList();
                

                   

                    listado = (from r in registros
                               join rid in ctx.Sam3_Rel_Incidencia_Despacho on r.DespachoID equals rid.DespachoID
                               join inc in ctx.Sam3_Incidencia on rid.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               join d in ctx.Sam3_Despacho on rid.DespachoID equals d.DespachoID
                               where rid.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   Estatus = inc.Estatus,
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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


        //Corte
        public List<ListadoIncidencias> ListadoIncidenciasCorte(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Corte> registros = new List<Sam3_Corte>();

                    
                        registros = (from c in ctx.Sam3_Corte
                                     join p in ctx.Sam3_Proyecto on c.ProyectoID equals p.ProyectoID
                                     join pc in ctx.Sam3_ProyectoConfiguracion on p.ProyectoID equals pc.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(c.CorteID)
                                     && pc.RequiereIncidenciaBilingue == true
                                     select c).AsParallel().Distinct().ToList();
                 

                    listado = (from r in registros
                               join ric in ctx.Sam3_Rel_Incidencia_Corte on r.CorteID equals ric.CorteID
                               join inc in ctx.Sam3_Incidencia on ric.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where ric.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Titulo = inc.Titulo,
                                   TituloIngles = inc.TituloIngles,
                                   Descripcion = inc.Descripcion,
                                   DescripcionIngles = inc.DescripcionIngles,
                                   DetalleResolucion = inc.DetalleResolucion,
                                   DetalleResolucionIngles = inc.DetalleResolucionIngles,
                                   MotivoCancelacion = inc.MotivoCancelacion,
                                   MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                   Respuesta = inc.Respuesta,
                                   RespuestaIngles = inc.RespuestaIngles,
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   TipoIncidencia = tpi.Nombre,
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

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


    }

}