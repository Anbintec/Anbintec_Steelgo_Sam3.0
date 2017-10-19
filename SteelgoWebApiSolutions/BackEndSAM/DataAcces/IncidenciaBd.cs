using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Configuration;
using BackEndSAM.Utilities;

namespace BackEndSAM.DataAcces
{
    public class IncidenciaBd
    {
        private static readonly object _mutex = new object();
        private static IncidenciaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private IncidenciaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static IncidenciaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new IncidenciaBd();
                    }
                }
                return _instance;
            }
        }

        public object GenerarIncidencia(Incidencia datos, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Incidencia nuevaIncidencia = new Sam3_Incidencia();
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"])
                    ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tan = ctx.Database.BeginTransaction())
                    {
                        nuevaIncidencia.Activo = true;
                        nuevaIncidencia.ClasificacionID = datos.ClasificacionID;
                        nuevaIncidencia.Descripcion = datos.Descripcion;
                        nuevaIncidencia.DetalleResolucion = datos.DetalleResolucion;
                        if (datos.FechaRegistro != null)
                        { nuevaIncidencia.FechaCreacion = Convert.ToDateTime(datos.FechaRegistro); }
                        else { nuevaIncidencia.FechaCreacion = DateTime.Now; }

                        nuevaIncidencia.FechaModificacion = DateTime.Now;
                        nuevaIncidencia.MotivoCancelacion = datos.MotivoCancelacion;
                        nuevaIncidencia.NoRFI = null;
                        nuevaIncidencia.Respuesta = datos.Respuesta;
                        nuevaIncidencia.TipoIncidenciaID = datos.TipoIncidenciaID;
                        nuevaIncidencia.Titulo = datos.Titulo;
                        nuevaIncidencia.UsuarioID = datos.RegistradoPor != null && datos.RegistradoPor != "" ? Convert.ToInt32(datos.RegistradoPor) : 1;
                        nuevaIncidencia.Version = 1;
                        nuevaIncidencia.UsuarioModificacion = usuario.UsuarioID;
                        nuevaIncidencia.IncidenciaInterna = datos.IncidenciaInterna;
                        nuevaIncidencia.TituloIngles = datos.TituloIngles;
                        nuevaIncidencia.DescripcionIngles = datos.DescripcionIngles;
                        nuevaIncidencia.RespuestaIngles = datos.RespuestaIngles;
                        nuevaIncidencia.MotivoCancelacionIngles = datos.MotivoCancelacionIngles;
                        nuevaIncidencia.DetalleResolucionIngles = datos.DetalleResolucionIngles;
                        //if (datos.FechaRespuesta != null) { nuevaIncidencia.FechaRespuesta = Convert.ToDateTime(datos.FechaRespuesta); }
                        //if (datos.FechaResolucion != null) { nuevaIncidencia.FechaSolucion = Convert.ToDateTime(datos.FechaResolucion); }
                        //nuevaIncidencia.UsuarioIDRespuesta = datos.RespondidoPor != null && datos.RespondidoPor != "" ? Convert.ToInt32(datos.RespondidoPor) : 1;
                        //nuevaIncidencia.UsuarioResuelveID = datos.ResueltoPor != null && datos.ResueltoPor != "" ? Convert.ToInt32(datos.ResueltoPor) : 1;
                        nuevaIncidencia.Estatus = datos.Estatus == null || datos.Estatus == "" ? "Abierta" : datos.Estatus;
                        //nuevaIncidencia.RespondidoPor = datos.RespondidoPor;
                        //nuevaIncidencia.ResueltoPor = datos.ResueltoPor;
                        nuevaIncidencia.RegistradoPor = datos.RegistradoPor;


                        ctx.Sam3_Incidencia.Add(nuevaIncidencia);

                        ctx.SaveChanges();
                        Sam3_Incidencia actualizarIncidencia;
                        Sam3_Rel_Proyecto_Entidad_Configuracion rel_proyecto_entidad_configuracion;
                        int consecutivoincidencia = 0;
                        if (datos.TipoIncidenciaID > 0)
                        {
                            switch (datos.TipoIncidenciaID)
                            {
                                case 1: //Folio Aviso Entrada
                                    Sam3_Rel_Incidencia_FolioAvisoLlegada nuevoRegistro = new Sam3_Rel_Incidencia_FolioAvisoLlegada();
                                    nuevoRegistro.Activo = true;
                                    nuevoRegistro.FechaModificacion = DateTime.Now;
                                    nuevoRegistro.FolioAvisoLlegadaID = datos.ReferenciaID;
                                    nuevoRegistro.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevoRegistro.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada.Add(nuevoRegistro);
                                    ctx.SaveChanges();

                                    datos.FolioConfiguracionIncidenciaID = nuevaIncidencia.IncidenciaID.ToString();

                                    Sam3_FolioAvisoLlegada folioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Entidad == folioAvisoLlegada.Entidad && x.Proyecto == folioAvisoLlegada.ProyectoNombrado).FirstOrDefault();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        datos.FolioConfiguracionIncidenciaID = (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                 + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                 + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                 + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias);

                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();


                                    }

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    break;
                                case 2: // Entrada de Material
                                    Sam3_Rel_Incidencia_FolioAvisoEntrada nuevaRelEntradaMaterial = new Sam3_Rel_Incidencia_FolioAvisoEntrada();
                                    nuevaRelEntradaMaterial.Activo = true;
                                    nuevaRelEntradaMaterial.FechaModificacion = DateTime.Now;
                                    nuevaRelEntradaMaterial.FolioAvisoEntradaID = datos.ReferenciaID;
                                    nuevaRelEntradaMaterial.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelEntradaMaterial.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada.Add(nuevaRelEntradaMaterial);
                                    ctx.SaveChanges();

                                    Sam3_FolioAvisoEntrada folioAvisoEntrada = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoEntradaID == datos.ReferenciaID).FirstOrDefault();
                                    Sam3_FolioAvisoLlegada folioAvisoLlegadaEntradaMaterial = ctx.Sam3_FolioAvisoLlegada
                                        .Where(x => x.FolioAvisoLlegadaID == folioAvisoEntrada.FolioAvisoLlegadaID).FirstOrDefault();


                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                        .Where(x => x.Entidad == folioAvisoLlegadaEntradaMaterial.Entidad && x.Proyecto == folioAvisoLlegadaEntradaMaterial.ProyectoNombrado)
                                        .FirstOrDefault();

                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ?
                                        (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                         + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                         + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                         + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }


                                    break;
                                case 3: // Pase Salida. Por el momento sin implementacion
                                    break;
                                case 4: // Packing List
                                    Sam3_Rel_Incidencia_FolioCuantificacion nuevaRelFolioC = new Sam3_Rel_Incidencia_FolioCuantificacion();
                                    nuevaRelFolioC.Activo = true;
                                    nuevaRelFolioC.FechaModificacion = DateTime.Now;
                                    nuevaRelFolioC.FolioCuantificacionID = datos.ReferenciaID;
                                    nuevaRelFolioC.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelFolioC.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioCuantificacion.Add(nuevaRelFolioC);
                                    ctx.SaveChanges();

                                    Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == folioCuantificacion.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                case 5: // Orden de recepcion

                                    Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.Folio == datos.ReferenciaID).AsParallel().SingleOrDefault();

                                    Sam3_Rel_Incidencia_OrdenRecepcion nuevaRelOrdenR = new Sam3_Rel_Incidencia_OrdenRecepcion();
                                    nuevaRelOrdenR.Activo = true;
                                    nuevaRelOrdenR.FechaModificacion = DateTime.Now;
                                    nuevaRelOrdenR.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelOrdenR.OrdenRecepcionID = orden.OrdenRecepcionID;
                                    nuevaRelOrdenR.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_OrdenRecepcion.Add(nuevaRelOrdenR);
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                        .Where(x => x.Activo == 1 && x.Rel_Proyecto_Entidad_Configuracion_ID == orden.Rel_Proyecto_Entidad_Configuracion_ID).AsParallel().FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? rel_proyecto_entidad_configuracion != null ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString() : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias && orden.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                case 6: // Complemento de recepcion. Por el momento sin implementacion
                                    break;
                                case 7: // ItemCode
                                    Sam3_Rel_Incidencia_ItemCode nuevaRelItemCode = new Sam3_Rel_Incidencia_ItemCode();
                                    nuevaRelItemCode.Activo = true;
                                    nuevaRelItemCode.FechaModificacion = DateTime.Now;
                                    nuevaRelItemCode.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelItemCode.ItemCodeID = datos.ReferenciaID;
                                    nuevaRelItemCode.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_ItemCode.Add(nuevaRelItemCode);
                                    ctx.SaveChanges();


                                    Sam3_ItemCode itemCode = (from rid in ctx.Sam3_Rel_ItemCode_Diametro
                                                              join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                              where rid.Activo && it.Activo
                                                              && rid.Rel_ItemCode_Diametro_ID == datos.ReferenciaID
                                                              select it).AsParallel().SingleOrDefault();

                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == itemCode.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                case 8: // Orden de almacenaje

                                    Sam3_OrdenAlmacenaje ordenAlmacenaje = ctx.Sam3_OrdenAlmacenaje.Where(x => x.Folio == datos.ReferenciaID)
                                        .AsParallel().SingleOrDefault();

                                    Sam3_Rel_Incidencia_OrdenAlmacenaje nuevaRelOrdenAlmacenaje = new Sam3_Rel_Incidencia_OrdenAlmacenaje();
                                    nuevaRelOrdenAlmacenaje.Activo = true;
                                    nuevaRelOrdenAlmacenaje.FechaModificacion = DateTime.Now;
                                    nuevaRelOrdenAlmacenaje.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelOrdenAlmacenaje.OrdenalmacenajeID = ordenAlmacenaje.OrdenAlmacenajeID;
                                    nuevaRelOrdenAlmacenaje.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje.Add(nuevaRelOrdenAlmacenaje);
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                       .Where(x => x.Activo == 1 && x.Rel_Proyecto_Entidad_Configuracion_ID == ordenAlmacenaje.Rel_Proyecto_Entidad_Configuracion_ID).AsParallel().FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? rel_proyecto_entidad_configuracion != null ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString() : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();


                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias && ordenAlmacenaje.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    break;
                                case 9: // Numero unico
                                    Sam3_Rel_Incidencia_NumeroUnico nuevaRelNumeroUnico = new Sam3_Rel_Incidencia_NumeroUnico();
                                    nuevaRelNumeroUnico.Activo = true;
                                    nuevaRelNumeroUnico.FechaModificacion = DateTime.Now;
                                    nuevaRelNumeroUnico.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelNumeroUnico.NumeroUnicoID = datos.ReferenciaID;
                                    nuevaRelNumeroUnico.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_NumeroUnico.Add(nuevaRelNumeroUnico);
                                    ctx.SaveChanges();


                                    Sam3_NumeroUnico numeroUnico = ctx.Sam3_NumeroUnico.Where(x => x.NumeroUnicoID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == numeroUnico.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                case 10: // Despacho
                                    Sam3_Rel_Incidencia_Despacho nuevaRelDespacho = new Sam3_Rel_Incidencia_Despacho();
                                    nuevaRelDespacho.Activo = true;
                                    nuevaRelDespacho.DespachoID = datos.ReferenciaID;
                                    nuevaRelDespacho.FechaModificacion = DateTime.Now;
                                    nuevaRelDespacho.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelDespacho.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_Despacho.Add(nuevaRelDespacho);
                                    ctx.SaveChanges();


                                    Sam3_Despacho despacho = ctx.Sam3_Despacho.Where(x => x.DespachoID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == despacho.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                case 11: // Corte
                                    Sam3_Rel_Incidencia_Corte nuevaRelCorte = new Sam3_Rel_Incidencia_Corte();
                                    nuevaRelCorte.Activo = true;
                                    nuevaRelCorte.CorteID = datos.ReferenciaID;
                                    nuevaRelCorte.FechaModificacion = DateTime.Now;
                                    nuevaRelCorte.IncidenciaID = nuevaIncidencia.IncidenciaID;
                                    nuevaRelCorte.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_Corte.Add(nuevaRelCorte);
                                    ctx.SaveChanges();

                                    Sam3_Corte corte = ctx.Sam3_Corte.Where(x => x.CorteID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == corte.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                   + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                   + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                   + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevaIncidencia.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevaIncidencia.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }
                                    break;
                                default:
                                    throw new Exception("No se encontro el tipo de incidencia");
                            }
                        }
                        else
                        {
                            throw new Exception("El tipo de incidencia es requerido");
                        }

                        ctx_tan.Commit();
                    }
                }

                datos.Version = nuevaIncidencia.Version.ToString();
                datos.FolioIncidenciaID = nuevaIncidencia.IncidenciaID;
                datos.FolioOriginalID = nuevaIncidencia.IncidenciaID.ToString();
                return datos;
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

        public object ActualizarIncidencia(Incidencia datos, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        DateTime fechaRespuesta = new DateTime();
                        DateTime fechaSolucion = new DateTime();
                        DateTime fechaRegistro = new DateTime();
                        Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                        if (datos.FechaRespuesta != null) { DateTime.TryParse(datos.FechaRespuesta, out fechaRespuesta); }
                        if (datos.FechaResolucion != null) { DateTime.TryParse(datos.FechaResolucion, out fechaSolucion); }
                        if (datos.FechaRegistro != null) { DateTime.TryParse(datos.FechaRegistro, out fechaRegistro); }


                        Sam3_Incidencia nuevoRegistro = new Sam3_Incidencia();
                        Sam3_Incidencia registro = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == datos.FolioIncidenciaID).AsParallel().SingleOrDefault();

                        if (datos.Estatus == "Cancelado" || datos.Estatus == "Resuelto" || datos.Estatus == "Respondido")
                        {
                            nuevoRegistro.IncidenciaID = registro.IncidenciaID;
                            registro.Activo = true;
                            registro.ClasificacionID = datos.ClasificacionID;
                            registro.Descripcion = datos.Descripcion;
                            registro.Estatus = datos.Estatus;
                            //if (fechaRegistro.ToShortTimeString() != "1/1/0001")
                            //{ registro.FechaCreacion = fechaRegistro; }
                            //else { registro.FechaCreacion = DateTime.Now; }
                            registro.NoRFI = null;
                            registro.TipoIncidenciaID = datos.TipoIncidenciaID;
                            registro.Titulo = datos.Titulo;
                            //if (datos.RegistradoPor != null && datos.RegistradoPor != "") { registro.UsuarioID = Convert.ToInt32(datos.RegistradoPor); }
                            registro.IncidenciaInterna = datos.IncidenciaInterna ;
                            registro.TituloIngles = datos.TituloIngles;
                            registro.DescripcionIngles = datos.DescripcionIngles;
                            
                            
                            
                            registro.FechaModificacion = DateTime.Now;
                            //registro.IncidenciaOriginalID = registro.IncidenciaID;
                            registro.IncidenciaOriginalID = Convert.ToInt32(datos.FolioOriginalID) == 0 ? datos.FolioIncidenciaID : Convert.ToInt32(datos.FolioOriginalID);
                            registro.UsuarioModificacion = usuario.UsuarioID;

                            if (datos.Estatus == "Cancelado")
                            {
                                registro.MotivoCancelacion = datos.MotivoCancelacion;
                                registro.MotivoCancelacionIngles = datos.MotivoCancelacionIngles;
                                registro.FechaCancelacion = DateTime.Now;
                                registro.UsuarioCancelaID = usuario.UsuarioID;
                            }
                            if (datos.Estatus == "Resuelto")
                            {
                                registro.DetalleResolucion = datos.DetalleResolucion;
                                registro.DetalleResolucionIngles = datos.DetalleResolucionIngles;
                                if (datos.ResueltoPor != null && datos.ResueltoPor != "") { registro.UsuarioResuelveID = Convert.ToInt32(datos.ResueltoPor); }
                                if (fechaSolucion.ToShortDateString() != "1/1/0001") { registro.FechaSolucion = fechaSolucion; }
                            }

                            if (datos.Estatus == "Respondido")
                            {
                                registro.Respuesta = datos.Respuesta;
                                registro.RespuestaIngles = datos.RespuestaIngles;
                                if (datos.RespondidoPor != null && datos.RespondidoPor != "") { registro.UsuarioIDRespuesta = Convert.ToInt32(datos.RespondidoPor); }
                                if (fechaRespuesta.ToShortDateString() != "1/1/0001") { registro.FechaRespuesta = fechaRespuesta; }
                            }

                            ctx.SaveChanges();
                        }
                        else
                        {
                            registro.Activo = false;
                            registro.FechaModificacion = DateTime.Now;
                            registro.UsuarioModificacion = usuario.UsuarioID;

                            ctx.SaveChanges();

                            nuevoRegistro.Activo = true;
                            nuevoRegistro.ClasificacionID = datos.ClasificacionID;
                            nuevoRegistro.Descripcion = datos.Descripcion;
                            //nuevoRegistro.DetalleResolucion = datos.DetalleResolucion;
                            nuevoRegistro.Estatus = datos.Estatus;
                            if (fechaRegistro.ToShortTimeString() != "1/1/0001")
                            { nuevoRegistro.FechaCreacion = fechaRegistro; }
                            else { nuevoRegistro.FechaCreacion = DateTime.Now; }
                            nuevoRegistro.IncidenciaInterna = datos.IncidenciaInterna;
                            nuevoRegistro.TituloIngles = datos.TituloIngles;
                            nuevoRegistro.DescripcionIngles = datos.DescripcionIngles;
                            nuevoRegistro.RespuestaIngles = datos.RespuestaIngles;
                            nuevoRegistro.MotivoCancelacionIngles = datos.MotivoCancelacionIngles;
                            nuevoRegistro.DetalleResolucionIngles = datos.DetalleResolucionIngles;
                            nuevoRegistro.FechaModificacion = DateTime.Now;
                            //if (fechaRespuesta.ToShortDateString() != "1/1/0001") { nuevoRegistro.FechaRespuesta = fechaRespuesta; }
                            //if (fechaSolucion.ToShortDateString() != "1/1/0001") { nuevoRegistro.FechaSolucion = fechaSolucion; }
                            nuevoRegistro.IncidenciaOriginalID = Convert.ToInt32(datos.FolioOriginalID) == 0 ? datos.FolioIncidenciaID : Convert.ToInt32(datos.FolioOriginalID);
                            //nuevoRegistro.MotivoCancelacion = datos.MotivoCancelacion;
                            nuevoRegistro.NoRFI = null;
                            //nuevoRegistro.Respuesta = datos.Respuesta;
                            nuevoRegistro.TipoIncidenciaID = datos.TipoIncidenciaID;
                            nuevoRegistro.Titulo = datos.Titulo;
                            nuevoRegistro.IncidenciaInterna = datos.IncidenciaInterna;
                            if (datos.RegistradoPor != null && datos.RegistradoPor != "") { nuevoRegistro.UsuarioID = Convert.ToInt32(datos.RegistradoPor); }
                            //if (datos.RespondidoPor != null && datos.RespondidoPor != "") { nuevoRegistro.UsuarioIDRespuesta = Convert.ToInt32(datos.RespondidoPor); }
                            //if (datos.ResueltoPor != null && datos.ResueltoPor != "") { nuevoRegistro.UsuarioResuelveID = Convert.ToInt32(datos.ResueltoPor); }
                            nuevoRegistro.UsuarioModificacion = usuario.UsuarioID;
                            nuevoRegistro.Version = registro.Version + 1;

                            ctx.Sam3_Incidencia.Add(nuevoRegistro);
                            ctx.SaveChanges();

                            Sam3_Incidencia actualizarIncidencia;
                            Sam3_Rel_Proyecto_Entidad_Configuracion rel_proyecto_entidad_configuracion;
                            int consecutivoincidencia = 0;

                            switch (datos.TipoIncidenciaID)
                            {
                                case 1: //Folio Aviso Entrada

                                    Sam3_Rel_Incidencia_FolioAvisoLlegada relFolioLlegqada = ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relFolioLlegqada.Activo = false;
                                    relFolioLlegqada.FechaModificacion = DateTime.Now;
                                    relFolioLlegqada.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_FolioAvisoLlegada nuevaRelFolio = new Sam3_Rel_Incidencia_FolioAvisoLlegada();
                                    nuevaRelFolio.Activo = true;
                                    nuevaRelFolio.FechaModificacion = DateTime.Now;
                                    nuevaRelFolio.FolioAvisoLlegadaID = datos.ReferenciaID;
                                    nuevaRelFolio.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelFolio.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada.Add(nuevaRelFolio);
                                    ctx.SaveChanges();

                                    Sam3_FolioAvisoLlegada folioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Entidad == folioAvisoLlegada.Entidad && x.Proyecto == folioAvisoLlegada.ProyectoNombrado).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                          + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                          + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                          + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();


                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Folio de Llegada : {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 2: // Entrada de Material
                                    Sam3_Rel_Incidencia_FolioAvisoEntrada relFolioEntrada = ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relFolioEntrada.Activo = false;
                                    relFolioEntrada.FechaModificacion = DateTime.Now;
                                    relFolioEntrada.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_FolioAvisoEntrada nuevaRelFolioEntrada = new Sam3_Rel_Incidencia_FolioAvisoEntrada();
                                    nuevaRelFolioEntrada.Activo = true;
                                    nuevaRelFolioEntrada.FechaModificacion = DateTime.Now;
                                    nuevaRelFolioEntrada.FolioAvisoEntradaID = datos.ReferenciaID;
                                    nuevaRelFolioEntrada.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelFolioEntrada.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada.Add(nuevaRelFolioEntrada);

                                    ctx.SaveChanges();


                                    Sam3_FolioAvisoEntrada folioAvisoEntrada = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoEntradaID == datos.ReferenciaID).FirstOrDefault();
                                    Sam3_FolioAvisoLlegada folioAvisoLlegadaEntradaMaterial = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioAvisoEntrada.FolioAvisoLlegadaID).FirstOrDefault();


                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Entidad == folioAvisoLlegadaEntradaMaterial.Entidad && x.Proyecto == folioAvisoLlegadaEntradaMaterial.ProyectoNombrado).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Folio de Entrada de Material : {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 3: // Pase Salida. Por el momento sin implementacion
                                    break;
                                case 4: // Packing List
                                    Sam3_Rel_Incidencia_FolioCuantificacion RelFolioC = ctx.Sam3_Rel_Incidencia_FolioCuantificacion
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    RelFolioC.Activo = false;
                                    RelFolioC.FechaModificacion = DateTime.Now;
                                    RelFolioC.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_FolioCuantificacion nuevaRelFolioC = new Sam3_Rel_Incidencia_FolioCuantificacion();
                                    nuevaRelFolioC.Activo = true;
                                    nuevaRelFolioC.FechaModificacion = DateTime.Now;
                                    nuevaRelFolioC.FolioCuantificacionID = datos.ReferenciaID;
                                    nuevaRelFolioC.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelFolioC.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_FolioCuantificacion.Add(nuevaRelFolioC);

                                    ctx.SaveChanges();


                                    Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == folioCuantificacion.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                   + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                   + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                   + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Packing List: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 5: // Orden de recepcion
                                    Sam3_Rel_Incidencia_OrdenRecepcion relOrdenR = ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relOrdenR.Activo = false;
                                    relOrdenR.FechaModificacion = DateTime.Now;
                                    relOrdenR.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_OrdenRecepcion nuevaRelOrdenR = new Sam3_Rel_Incidencia_OrdenRecepcion();
                                    nuevaRelOrdenR.Activo = true;
                                    nuevaRelOrdenR.FechaModificacion = DateTime.Now;
                                    nuevaRelOrdenR.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelOrdenR.OrdenRecepcionID = datos.ReferenciaID;
                                    nuevaRelOrdenR.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_OrdenRecepcion.Add(nuevaRelOrdenR);

                                    ctx.SaveChanges();

                                    Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.OrdenRecepcionID == datos.ReferenciaID).AsParallel().SingleOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                .Where(x => x.Activo == 1 && x.Rel_Proyecto_Entidad_Configuracion_ID == orden.Rel_Proyecto_Entidad_Configuracion_ID).AsParallel().FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? rel_proyecto_entidad_configuracion != null ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString() : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias && orden.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para la Orden de Recepción: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 6: // Complemento de recepcion. Por el momento sin implementacion
                                    break;
                                case 7: // ItemCode
                                    Sam3_Rel_Incidencia_ItemCode relItemCode = ctx.Sam3_Rel_Incidencia_ItemCode
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().AsParallel().SingleOrDefault();
                                    relItemCode.Activo = false;
                                    relItemCode.FechaModificacion = DateTime.Now;
                                    relItemCode.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_ItemCode nuevaRelItemCode = new Sam3_Rel_Incidencia_ItemCode();
                                    nuevaRelItemCode.Activo = true;
                                    nuevaRelItemCode.FechaModificacion = DateTime.Now;
                                    nuevaRelItemCode.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelItemCode.ItemCodeID = datos.ReferenciaID;
                                    nuevaRelItemCode.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_ItemCode.Add(nuevaRelItemCode);

                                    ctx.SaveChanges();

                                    Sam3_ItemCode itemCode = (from rid in ctx.Sam3_Rel_ItemCode_Diametro
                                                              join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                              where rid.Rel_ItemCode_Diametro_ID == datos.ReferenciaID
                                                              select it).AsParallel().Distinct().SingleOrDefault();

                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == itemCode.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el ItemCode: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 8: // Orden de almacenaje
                                    Sam3_Rel_Incidencia_OrdenAlmacenaje relOrdenAlmacenaje = ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relOrdenAlmacenaje.Activo = false;
                                    relOrdenAlmacenaje.FechaModificacion = DateTime.Now;
                                    relOrdenAlmacenaje.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_OrdenAlmacenaje nuevaRelOrdenAlmacenaje = new Sam3_Rel_Incidencia_OrdenAlmacenaje();
                                    nuevaRelOrdenAlmacenaje.Activo = true;
                                    nuevaRelOrdenAlmacenaje.FechaModificacion = DateTime.Now;
                                    nuevaRelOrdenAlmacenaje.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelOrdenAlmacenaje.OrdenalmacenajeID = datos.ReferenciaID;
                                    nuevaRelOrdenAlmacenaje.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje.Add(nuevaRelOrdenAlmacenaje);

                                    ctx.SaveChanges();

                                    Sam3_OrdenAlmacenaje ordenAlmacenaje = ctx.Sam3_OrdenAlmacenaje.Where(x => x.OrdenAlmacenajeID == datos.ReferenciaID)
                                             .AsParallel().SingleOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                       .Where(x => x.Activo == 1 && x.Rel_Proyecto_Entidad_Configuracion_ID == ordenAlmacenaje.Rel_Proyecto_Entidad_Configuracion_ID).AsParallel().FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? rel_proyecto_entidad_configuracion != null ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                  + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString() : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias && ordenAlmacenaje.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }


                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para la Orden de Almacenaje: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 9: // Numero unico
                                    Sam3_Rel_Incidencia_NumeroUnico relNumeroUnico = ctx.Sam3_Rel_Incidencia_NumeroUnico
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relNumeroUnico.Activo = false;
                                    relNumeroUnico.FechaModificacion = DateTime.Now;
                                    relNumeroUnico.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_NumeroUnico nuevaRelNumeroUnico = new Sam3_Rel_Incidencia_NumeroUnico();
                                    nuevaRelNumeroUnico.Activo = true;
                                    nuevaRelNumeroUnico.FechaModificacion = DateTime.Now;
                                    nuevaRelNumeroUnico.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelNumeroUnico.NumeroUnicoID = datos.ReferenciaID;
                                    nuevaRelNumeroUnico.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_NumeroUnico.Add(nuevaRelNumeroUnico);

                                    ctx.SaveChanges();


                                    Sam3_NumeroUnico numeroUnico = ctx.Sam3_NumeroUnico.Where(x => x.NumeroUnicoID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == numeroUnico.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Número Único: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 10: // Despacho
                                    Sam3_Rel_Incidencia_Despacho relDespacho = ctx.Sam3_Rel_Incidencia_Despacho
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relDespacho.Activo = false;
                                    relDespacho.FechaModificacion = DateTime.Now;
                                    relDespacho.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_Despacho nuevaRelDespacho = new Sam3_Rel_Incidencia_Despacho();
                                    nuevaRelDespacho.Activo = true;
                                    nuevaRelDespacho.DespachoID = datos.ReferenciaID;
                                    nuevaRelDespacho.FechaModificacion = DateTime.Now;
                                    nuevaRelDespacho.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelDespacho.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_Despacho.Add(nuevaRelDespacho);

                                    ctx.SaveChanges();

                                    Sam3_Despacho despacho = ctx.Sam3_Despacho.Where(x => x.DespachoID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == despacho.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Despacho: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                case 11: // Corte
                                    Sam3_Rel_Incidencia_Corte relCorte = ctx.Sam3_Rel_Incidencia_Corte
                                        .Where(x => x.Activo && x.IncidenciaID == registro.IncidenciaID).AsParallel().SingleOrDefault();
                                    relCorte.Activo = false;
                                    relCorte.FechaModificacion = DateTime.Now;
                                    relCorte.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_Rel_Incidencia_Corte nuevaRelCorte = new Sam3_Rel_Incidencia_Corte();
                                    nuevaRelCorte.Activo = true;
                                    nuevaRelCorte.CorteID = datos.ReferenciaID;
                                    nuevaRelCorte.FechaModificacion = DateTime.Now;
                                    nuevaRelCorte.IncidenciaID = nuevoRegistro.IncidenciaID;
                                    nuevaRelCorte.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_Corte.Add(nuevaRelCorte);

                                    ctx.SaveChanges();


                                    Sam3_Corte corte = ctx.Sam3_Corte.Where(x => x.CorteID == datos.ReferenciaID).FirstOrDefault();
                                    rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion.Where(x => x.Activo == 1 && x.Proyecto == corte.ProyectoID).FirstOrDefault();
                                    datos.FolioConfiguracionIncidenciaID = ActivarFolioConfiguracionIncidencias ? (rel_proyecto_entidad_configuracion.PreFijoFolioIncidencias + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.ConsecutivoIncidencias.ToString() + ","
                                                                                                                                    + rel_proyecto_entidad_configuracion.PostFijoFolioIncidencias) : nuevoRegistro.IncidenciaID.ToString();

                                    consecutivoincidencia = rel_proyecto_entidad_configuracion.ConsecutivoIncidencias;
                                    actualizarIncidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == nuevoRegistro.IncidenciaID).FirstOrDefault();
                                    actualizarIncidencia.Consecutivo = consecutivoincidencia;
                                    actualizarIncidencia.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                                    ctx.SaveChanges();

                                    rel_proyecto_entidad_configuracion.ConsecutivoIncidencias = consecutivoincidencia + 1;
                                    ctx.SaveChanges();

                                    if (ActivarFolioConfiguracionIncidencias)
                                    {
                                        string[] elemntos = datos.FolioConfiguracionIncidenciaID.Split(',').ToArray();
                                        int digitos = Convert.ToInt32(elemntos[1]);
                                        int consecutivo = Convert.ToInt32(elemntos[2]);
                                        string formato = "D" + digitos.ToString();

                                        datos.FolioConfiguracionIncidenciaID = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                                    }

                                    EnviarAvisosBd.Instance.EnviarNotificación(1,
                                        string.Format("Se generó una incidencia para el Corte: {0}", datos.ReferenciaID),
                                        usuario);

                                    break;
                                default:
                                    throw new Exception("No se encontro el tipo de incidencia");
                            }
                        }

                        List<Sam3_Rel_Incidencia_Documento> documentos = ctx.Sam3_Rel_Incidencia_Documento.Where(x => x.IncidenciaID == datos.FolioIncidenciaID && x.Activo).AsParallel().ToList();

                        foreach (Sam3_Rel_Incidencia_Documento doc in documentos)
                        {
                            if (doc.Activo)
                            {
                                doc.Activo = false;
                                doc.FechaModificacion = DateTime.Now;
                                doc.UsuarioModificacion = usuario.UsuarioID;
                                ctx.SaveChanges();

                                Sam3_Rel_Incidencia_Documento nuevaRelDocumento = new Sam3_Rel_Incidencia_Documento();
                                nuevaRelDocumento.Activo = true;
                                nuevaRelDocumento.ContentType = doc.ContentType;
                                nuevaRelDocumento.Descripcion = doc.Descripcion;
                                nuevaRelDocumento.DescripcionIngles = doc.DescripcionIngles;
                                nuevaRelDocumento.DocGuid = doc.DocGuid;
                                nuevaRelDocumento.DocumentoID = doc.DocumentoID;
                                nuevaRelDocumento.Extencion = doc.Extencion;
                                nuevaRelDocumento.FechaModificacion = DateTime.Now;
                                nuevaRelDocumento.IncidenciaID = nuevoRegistro.IncidenciaID;
                                nuevaRelDocumento.Nombre = doc.Nombre;
                                nuevaRelDocumento.TipoArchivoID = 0;
                                nuevaRelDocumento.Url = doc.Url;
                                nuevaRelDocumento.UsuarioModificacion = usuario.UsuarioID;
                                nuevaRelDocumento.EstatusIncidencia = doc.EstatusIncidencia;

                                ctx.Sam3_Rel_Incidencia_Documento.Add(nuevaRelDocumento);
                            }
                        }

                        ctx.SaveChanges();


                        ctx_tran.Commit();

                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add(nuevoRegistro.IncidenciaID.ToString() + ',' + nuevoRegistro.IncidenciaOriginalID.ToString());
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

        public object EliminarIncidencia(int incidenciaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        Sam3_Incidencia incidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == incidenciaID).AsParallel().SingleOrDefault();
                        incidencia.Activo = false;
                        incidencia.FechaModificacion = DateTime.Now;
                        incidencia.UsuarioModificacion = usuario.UsuarioID;


                        switch (incidencia.TipoIncidenciaID)
                        {
                            case 1: //Folio Aviso Entrada
                                List<Sam3_Rel_Incidencia_FolioAvisoLlegada> incidenciasFolioLlegada =
                                    (from rif in ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada
                                     where rif.Activo && rif.IncidenciaID == incidencia.IncidenciaID
                                     select rif).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_FolioAvisoLlegada inc in incidenciasFolioLlegada)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 2: // Entrada de Material
                                List<Sam3_Rel_Incidencia_FolioAvisoEntrada> incidenciasEntradaMaterial =
                                    (from rie in ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada
                                     where rie.Activo && rie.IncidenciaID == incidencia.IncidenciaID
                                     select rie).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_FolioAvisoEntrada inc in incidenciasEntradaMaterial)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 3: // Pase Salida. Por el momento sin implementacion
                                break;
                            case 4: // Packing List
                                List<Sam3_Rel_Incidencia_FolioCuantificacion> incidenciasRelFolioC =
                                    (from rifc in ctx.Sam3_Rel_Incidencia_FolioCuantificacion
                                     where rifc.Activo && rifc.IncidenciaID == incidencia.IncidenciaID
                                     select rifc).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_FolioCuantificacion inc in incidenciasRelFolioC)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 5: // Orden de recepcion
                                List<Sam3_Rel_Incidencia_OrdenRecepcion> incidenciasOrdenR =
                                    (from rior in ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                                     where rior.Activo && rior.IncidenciaID == incidencia.IncidenciaID
                                     select rior).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_OrdenRecepcion inc in incidenciasOrdenR)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 6: // Complemento de recepcion. Por el momento sin implementacion
                                break;
                            case 7: // ItemCode
                                List<Sam3_Rel_Incidencia_ItemCode> incidenciasItemCode =
                                    (from riit in ctx.Sam3_Rel_Incidencia_ItemCode
                                     where riit.Activo && riit.IncidenciaID == incidencia.IncidenciaID
                                     select riit).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_ItemCode inc in incidenciasItemCode)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 8: // Orden de almacenaje
                                List<Sam3_Rel_Incidencia_OrdenAlmacenaje> incidenciasOrdenAlmacenaje =
                                    (from rioa in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                                     where rioa.Activo && rioa.IncidenciaID == incidencia.IncidenciaID
                                     select rioa).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_OrdenAlmacenaje inc in incidenciasOrdenAlmacenaje)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 9: // Numero unico
                                List<Sam3_Rel_Incidencia_NumeroUnico> incidenciasNumeroUnico =
                                    (from rin in ctx.Sam3_Rel_Incidencia_NumeroUnico
                                     where rin.Activo && rin.IncidenciaID == incidencia.IncidenciaID
                                     select rin).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_NumeroUnico inc in incidenciasNumeroUnico)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 10: // Despacho
                                List<Sam3_Rel_Incidencia_Despacho> incidenciasDespacho =
                                    (from rid in ctx.Sam3_Rel_Incidencia_Despacho
                                     where rid.Activo && rid.IncidenciaID == incidencia.IncidenciaID
                                     select rid).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_Despacho inc in incidenciasDespacho)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }

                                break;
                            case 11: // Corte
                                List<Sam3_Rel_Incidencia_Corte> incidenciasCorte =
                                    (from ric in ctx.Sam3_Rel_Incidencia_Corte
                                     where ric.Activo && ric.IncidenciaID == incidencia.IncidenciaID
                                     select ric).Distinct().AsParallel().ToList();

                                foreach (Sam3_Rel_Incidencia_Corte inc in incidenciasCorte)
                                {
                                    inc.Activo = false;
                                    inc.FechaModificacion = DateTime.Now;
                                    inc.UsuarioModificacion = usuario.UsuarioID;
                                }
                                break;
                            default:
                                throw new Exception("No se encontro el tipo de incidencia");
                        }

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

        public object DetalleIncidencia(int incidenciaID, Sam3_Usuario usuario)
        {
            try
            {
                Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                Boolean activarFolioConfiguracionCuantificacion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;
                Boolean activarFolioConfiguracionOrdenRecepcion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"].Equals("1") ? true : false) : false;
                Boolean activarFolioConfiguracionOrdenAlmacenaje = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenAlmacenaje"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenAlmacenaje"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    Incidencia detalle = (from inc in ctx.Sam3_Incidencia
                                          where inc.Activo && inc.IncidenciaID == incidenciaID
                                          select new Incidencia
                                          {
                                              ArchivosIncidencia = (from rid in ctx.Sam3_Rel_Incidencia_Documento
                                                                    where rid.Activo && rid.IncidenciaID == inc.IncidenciaID
                                                                    && rid.EstatusIncidencia == 1
                                                                    select new ListaDocumentos
                                                                    {
                                                                        Descripcion = rid.Descripcion,
                                                                        DescripcionIngles = rid.DescripcionIngles == null ? "": rid.DescripcionIngles,
                                                                        DocumentoID = rid.DocGuid.ToString(),
                                                                        Extencion = rid.Extencion,
                                                                        Nombre = rid.Nombre,
                                                                        TipoArchivo = rid.TipoArchivoID.ToString(),
                                                                        Url = rid.Url
                                                                    }).ToList(),
                                              ArchivosResponder = (from rid in ctx.Sam3_Rel_Incidencia_Documento
                                                                   where rid.Activo && rid.IncidenciaID == inc.IncidenciaID
                                                                   && rid.EstatusIncidencia == 3
                                                                   select new ListaDocumentos
                                                                   {
                                                                       Descripcion = rid.Descripcion,
                                                                       DescripcionIngles = rid.DescripcionIngles == null ? "" : rid.DescripcionIngles,
                                                                       DocumentoID = rid.DocGuid.ToString(),
                                                                       Extencion = rid.Extencion,
                                                                       Nombre = rid.Nombre,
                                                                       TipoArchivo = rid.TipoArchivoID.ToString(),
                                                                       Url = rid.Url
                                                                   }).ToList(),
                                              ArchivosResolver = (from rid in ctx.Sam3_Rel_Incidencia_Documento
                                                                  where rid.Activo && rid.IncidenciaID == inc.IncidenciaID
                                                                  && rid.EstatusIncidencia == 2
                                                                  select new ListaDocumentos
                                                                  {
                                                                      Descripcion = rid.Descripcion,
                                                                      DescripcionIngles = rid.DescripcionIngles == null ? "" : rid.DescripcionIngles,
                                                                      DocumentoID = rid.DocGuid.ToString(),
                                                                      Extencion = rid.Extencion,
                                                                      Nombre = rid.Nombre,
                                                                      TipoArchivo = rid.TipoArchivoID.ToString(),
                                                                      Url = rid.Url
                                                                  }).ToList(),
                                              ArchivosCancelar = (from rid in ctx.Sam3_Rel_Incidencia_Documento
                                                                  where rid.Activo && rid.IncidenciaID == inc.IncidenciaID
                                                                  && rid.EstatusIncidencia == 4
                                                                  select new ListaDocumentos
                                                                  {
                                                                      Descripcion = rid.Descripcion,
                                                                      DescripcionIngles = rid.DescripcionIngles == null ? "" : rid.DescripcionIngles,
                                                                      DocumentoID = rid.DocGuid.ToString(),
                                                                      Extencion = rid.Extencion,
                                                                      Nombre = rid.Nombre,
                                                                      TipoArchivo = rid.TipoArchivoID.ToString(),
                                                                      Url = rid.Url
                                                                  }).ToList(),
                                              ClasificacionID = inc.ClasificacionID,
                                              Descripcion = inc.Descripcion,
                                              DetalleResolucion = inc.DetalleResolucion,
                                              Estatus = inc.Estatus,
                                              FechaRegistro = inc.FechaCreacion.ToString(),
                                              FechaResolucion = inc.FechaSolucion.ToString(),
                                              FechaRespuesta = inc.FechaRespuesta.ToString(),
                                              FolioIncidenciaID = inc.IncidenciaID,
                                              MotivoCancelacion = inc.MotivoCancelacion,
                                              RegistradoPor = inc.UsuarioID.ToString(),
                                              RespondidoPor = inc.UsuarioIDRespuesta.ToString(),
                                              Respuesta = inc.Respuesta,
                                              ResueltoPor = inc.UsuarioResuelveID.ToString(),
                                              TipoIncidenciaID = inc.TipoIncidenciaID,
                                              Titulo = inc.Titulo,
                                              FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                              Version = inc.Version.ToString(),
                                              IncidenciaInterna = inc.IncidenciaInterna ,
                                              TituloIngles = inc.TituloIngles,
                                              DescripcionIngles = inc.DescripcionIngles,
                                              RespuestaIngles = inc.RespuestaIngles,
                                              MotivoCancelacionIngles = inc.MotivoCancelacionIngles,
                                              DetalleResolucionIngles = inc.DetalleResolucionIngles
                                          }).AsParallel().SingleOrDefault();

                    switch (detalle.TipoIncidenciaID)
                    {
                        case 1: //Folio Aviso Entrada
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.FolioAvisoLlegadaID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = detalle.ReferenciaID.ToString();

                            if (activarFolioConfiguracion)
                            {
                                Sam3_FolioAvisoLlegada folioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == detalle.ReferenciaID).FirstOrDefault();

                                detalle.ValorReferencia = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                           where pc.Entidad == folioAvisoLlegada.Entidad && pc.Proyecto == folioAvisoLlegada.ProyectoNombrado
                                                           select pc.PreFijoFolioAvisoLlegada + ","
                                                           + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                           + folioAvisoLlegada.Consecutivo.ToString() + ","
                                                           + pc.PostFijoFolioAvisoLlegada).FirstOrDefault();

                                if (detalle.ValorReferencia != null)
                                {
                                    string[] elemntos = detalle.ValorReferencia.Split(',').ToArray();
                                    int digitosAE = Convert.ToInt32(elemntos[1]);
                                    int consecutivoAE = Convert.ToInt32(elemntos[2]);
                                    string formatoAE = "D" + digitosAE.ToString();

                                    detalle.ValorReferencia = elemntos[0].Trim() + consecutivoAE.ToString(formatoAE).Trim() + elemntos[3].Trim();
                                }
                            }

                            break;
                        case 2: // Entrada de Material
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.FolioAvisoEntradaID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = detalle.ReferenciaID.ToString();

                            if (activarFolioConfiguracion)
                            {
                                Sam3_FolioAvisoEntrada folioAvisoEntrada = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoEntradaID == detalle.ReferenciaID).FirstOrDefault();
                                Sam3_FolioAvisoLlegada folioLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioAvisoEntrada.FolioAvisoLlegadaID).FirstOrDefault();

                                detalle.ValorReferencia = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                           where pc.Entidad == folioLlegada.Entidad && pc.Proyecto == folioLlegada.ProyectoNombrado
                                                           select pc.PreFijoFolioAvisoLlegada + ","
                                                            + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                            + folioLlegada.Consecutivo.ToString() + ","
                                                            + pc.PostFijoFolioAvisoLlegada).FirstOrDefault();

                                if (detalle.ValorReferencia != null)
                                {
                                    string[] elemntos = detalle.ValorReferencia.Split(',').ToArray();
                                    int digitosE = Convert.ToInt32(elemntos[1]);
                                    int consecutivoE = Convert.ToInt32(elemntos[2]);
                                    string formatoE = "D" + digitosE.ToString();

                                    detalle.ValorReferencia = elemntos[0].Trim() + consecutivoE.ToString(formatoE).Trim() + elemntos[3].Trim();
                                }
                            }

                            break;
                        case 3: // Pase Salida. Por el momento sin implementacion
                            break;
                        case 4: // Packing List
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_FolioCuantificacion
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.FolioCuantificacionID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = detalle.ReferenciaID.ToString();

                            Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == detalle.ReferenciaID).FirstOrDefault();
                            Sam3_FolioAvisoLlegada folioLl = (from fc in ctx.Sam3_FolioCuantificacion
                                                              join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                              join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                                              where fc.Activo && fe.Activo && fa.Activo
                                                              && fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                              select fa).AsParallel().FirstOrDefault();

                            string NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                              where pc.Proyecto == folioLl.ProyectoNombrado
                                                              && pc.Entidad == folioLl.Entidad
                                                              select pc.PreFijoFolioAvisoLlegada + ","
                                                              + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                              + folioLl.Consecutivo + ","
                                                              + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault();

                            string NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                where pc.Rel_Proyecto_Entidad_Configuracion_ID == folioCuantificacion.Rel_Proyecto_Entidad_Configuracion_ID
                                                                select pc.PreFijoFolioPackingList + ","
                                                                + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                                + folioCuantificacion.ConsecutivoConfiguracion.ToString() + ","
                                                                + pc.PostFijoFolioPackingList).FirstOrDefault();

                            int FolioAvisoLlegadaID = folioLl.FolioAvisoLlegadaID;
                            int ConsecutivoFolioCuanificacion = folioCuantificacion.Consecutivo.Value;
                            int ConsecutivoFolioLlegada = folioLl.Consecutivo.Value;

                            NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioAvisoLlegada);
                            NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioCuantificacion);

                            if (activarFolioConfiguracion && activarFolioConfiguracionCuantificacion)
                            {
                                detalle.ValorReferencia = NombreFolioAvisoLlegada + "-" + NombreFolioCuantificacion;
                            }

                            if (activarFolioConfiguracion && !activarFolioConfiguracionCuantificacion)
                            {
                                detalle.ValorReferencia = NombreFolioAvisoLlegada + "-" + ConsecutivoFolioCuanificacion;
                            }

                            if (!activarFolioConfiguracion && activarFolioConfiguracionCuantificacion)
                            {
                                detalle.ValorReferencia = FolioAvisoLlegadaID + "-" + NombreFolioCuantificacion;
                            }

                            if (!activarFolioConfiguracion && !activarFolioConfiguracionCuantificacion)
                            {
                                detalle.ValorReferencia = FolioAvisoLlegadaID + "-" + ConsecutivoFolioCuanificacion;
                            }

                            break;
                        case 5: // Orden de recepcion
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.OrdenRecepcionID).AsParallel().SingleOrDefault();

                            detalle.ValorReferencia = (from r in ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                                                       join o in ctx.Sam3_OrdenRecepcion on r.OrdenRecepcionID equals o.OrdenRecepcionID
                                                       where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                       select o.Folio).AsParallel().SingleOrDefault().ToString();
                            detalle.registroID = int.Parse(detalle.ValorReferencia);
                            if (activarFolioConfiguracionOrdenRecepcion)
                            {
                                Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.OrdenRecepcionID == detalle.ReferenciaID).FirstOrDefault();

                                detalle.ValorReferencia = orden.Rel_Proyecto_Entidad_Configuracion_ID != null ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                                 where pc.Rel_Proyecto_Entidad_Configuracion_ID == orden.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                                 select pc.PreFijoFolioOrdenRecepcion + ","
                                                                                                                 + pc.CantidadCerosFolioOrdenRecepcion.ToString() + ","
                                                                                                                 + orden.Consecutivo.ToString() + ","
                                                                                                                 + pc.PostFijoFolioOrdenRecepcion).AsParallel().FirstOrDefault() : detalle.ValorReferencia;

                                if (!string.IsNullOrEmpty(detalle.ValorReferencia) && orden.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                {
                                    string[] elemntosOR = detalle.ValorReferencia.Split(',').ToArray();
                                    int digitosOR = Convert.ToInt32(elemntosOR[1]);
                                    int consecutivoOR = Convert.ToInt32(elemntosOR[2]);
                                    string formatoOR = "D" + digitosOR.ToString();

                                    detalle.ValorReferencia = elemntosOR[0].Trim() + consecutivoOR.ToString(formatoOR).Trim() + elemntosOR[3].Trim();
                                }
                            }

                            break;
                        case 6: // Complemento de recepcion. Por el momento sin implementacion
                            break;
                        case 7: // ItemCode
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_ItemCode
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.ItemCodeID).AsParallel().SingleOrDefault();

                            detalle.ValorReferencia = (from r in ctx.Sam3_Rel_Incidencia_ItemCode
                                                       join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.Rel_ItemCode_Diametro_ID
                                                       join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                       join p in ctx.Sam3_Proyecto on it.ProyectoID equals p.ProyectoID
                                                       join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                                       join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                                       where r.Activo && it.Activo && p.Activo && rid.Activo && d1.Activo && d2.Activo
                                                       && r.ItemCodeID == detalle.ReferenciaID
                                                       && r.IncidenciaID == detalle.FolioIncidenciaID
                                                       select it.Codigo + "(" +
                                                       d1.Valor.ToString() + "," +
                                                       d2.Valor.ToString() + ")(" +
                                                       p.Nombre + ")").AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            break;
                        case 8: // Orden de almacenaje
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.OrdenalmacenajeID).AsParallel().SingleOrDefault();

                            detalle.ValorReferencia = (from r in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                                                       join oa in ctx.Sam3_OrdenAlmacenaje on r.OrdenalmacenajeID equals oa.OrdenAlmacenajeID
                                                       where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                       select oa.Folio).AsParallel().SingleOrDefault().ToString();
                            detalle.registroID = int.Parse(detalle.ValorReferencia);
                            if (activarFolioConfiguracionOrdenAlmacenaje)
                            {
                                Sam3_OrdenAlmacenaje orden = ctx.Sam3_OrdenAlmacenaje.Where(x => x.OrdenAlmacenajeID == detalle.ReferenciaID).FirstOrDefault();

                                detalle.ValorReferencia = orden.Rel_Proyecto_Entidad_Configuracion_ID != null ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                                 where pc.Rel_Proyecto_Entidad_Configuracion_ID == orden.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                                 select pc.PreFijoFolioOrdenAlmacenaje + ","
                                                                                                                 + pc.CantidadCerosFolioOrdenAlmacenaje.ToString() + ","
                                                                                                                 + orden.Consecutivo.ToString() + ","
                                                                                                                 + pc.PostFijoFolioOrdenAlmacenaje).AsParallel().FirstOrDefault() : detalle.ValorReferencia;

                                if (!string.IsNullOrEmpty(detalle.ValorReferencia) && orden.Rel_Proyecto_Entidad_Configuracion_ID != null)
                                {
                                    string[] elemntosOA = detalle.ValorReferencia.Split(',').ToArray();
                                    int digitosOA = Convert.ToInt32(elemntosOA[1]);
                                    int consecutivoOA = Convert.ToInt32(elemntosOA[2]);
                                    string formatoOA = "D" + digitosOA.ToString();

                                    detalle.ValorReferencia = elemntosOA[0].Trim() + consecutivoOA.ToString(formatoOA).Trim() + elemntosOA[3].Trim();
                                }
                            }

                            break;
                        case 9: // Numero unico

                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_NumeroUnico
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.NumeroUnicoID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = (from r in ctx.Sam3_Rel_Incidencia_NumeroUnico
                                                       join nu in ctx.Sam3_NumeroUnico on r.NumeroUnicoID equals nu.NumeroUnicoID
                                                       where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                       select nu.Prefijo + "-" + nu.Consecutivo).AsParallel().SingleOrDefault();

                            int digitos = (from d in ctx.Sam3_ProyectoConfiguracion
                                           join nu in ctx.Sam3_NumeroUnico on d.ProyectoID equals nu.ProyectoID
                                           where d.Activo && nu.NumeroUnicoID == detalle.ReferenciaID
                                           select d.DigitosNumeroUnico).AsParallel().SingleOrDefault();


                            string[] elementos = detalle.ValorReferencia.Split('-').ToArray();
                            string formato = "D" + digitos.ToString();
                            int temp = Convert.ToInt32(elementos[1]);

                            detalle.ValorReferencia = elementos[0] + "-" + temp.ToString(formato);


                            break;
                        case 10: // Despacho
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_Despacho
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.DespachoID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = detalle.ReferenciaID.ToString();

                            break;
                        case 11: // Corte
                            detalle.ReferenciaID = (from r in ctx.Sam3_Rel_Incidencia_Corte
                                                    where r.Activo && r.IncidenciaID == detalle.FolioIncidenciaID
                                                    select r.CorteID).AsParallel().SingleOrDefault();
                            detalle.registroID = detalle.ReferenciaID;
                            detalle.ValorReferencia = detalle.ReferenciaID.ToString();

                            break;
                        default:
                            throw new Exception("No se encontro el tipo de incidencia");
                    }

                    DateTime fechaRegistro = new DateTime();
                    DateTime fechaResuelto = new DateTime();
                    DateTime fechaRespuesta = new DateTime();

                    DateTime.TryParse(detalle.FechaRegistro, out fechaRegistro);
                    detalle.FechaRegistro = fechaRegistro.ToString("yyyy-MM-dd");

                    if (detalle.FechaResolucion != "")
                    {
                        DateTime.TryParse(detalle.FechaResolucion, out fechaResuelto);
                        detalle.FechaResolucion = fechaResuelto.ToString("yyyy-MM-dd");
                    }
                    if (detalle.FechaRespuesta != "")
                    {
                        DateTime.TryParse(detalle.FechaRespuesta, out fechaRespuesta);
                        detalle.FechaRespuesta = fechaRespuesta.ToString("yyyy-MM-dd");
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

        public object BuscarIncidencia(int tipoIncidenciaID, int referenciaID, Sam3_Usuario usuario)
        {
            try
            {
                Incidencia detalle = new Incidencia();
                int incidenciaID = 0;

                using (SamContext ctx = new SamContext())
                {
                    switch (tipoIncidenciaID)
                    {
                        case 1: //Folio Aviso Entrada
                            incidenciaID = (from rif in ctx.Sam3_Rel_Incidencia_FolioAvisoLlegada
                                            where rif.Activo
                                            && rif.FolioAvisoLlegadaID == referenciaID
                                            select rif.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 2: // Entrada de Material
                            incidenciaID = (from rif in ctx.Sam3_Rel_Incidencia_FolioAvisoEntrada
                                            where rif.Activo
                                            && rif.FolioAvisoEntradaID == referenciaID
                                            select rif.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 3: // Pase Salida. Por el momento sin implementacion
                            break;
                        case 4: // Packing List
                            incidenciaID = (from rif in ctx.Sam3_Rel_Incidencia_FolioCuantificacion
                                            where rif.Activo
                                            && rif.FolioCuantificacionID == referenciaID
                                            select rif.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 5: // Orden de recepcion
                            incidenciaID = (from rio in ctx.Sam3_Rel_Incidencia_OrdenRecepcion
                                            where rio.Activo
                                            && rio.OrdenRecepcionID == referenciaID
                                            select rio.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 6: // Complemento de recepcion. Por el momento sin implementacion
                            break;
                        case 7: // ItemCode
                            incidenciaID = (from riit in ctx.Sam3_Rel_Incidencia_ItemCode
                                            where riit.Activo
                                            && riit.ItemCodeID == referenciaID
                                            select riit.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 8: // Orden de almacenaje
                            incidenciaID = (from ria in ctx.Sam3_Rel_Incidencia_OrdenAlmacenaje
                                            where ria.Activo
                                            && ria.OrdenalmacenajeID == referenciaID
                                            select ria.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 9: // Numero unico
                            incidenciaID = (from rin in ctx.Sam3_Rel_Incidencia_NumeroUnico
                                            where rin.Activo
                                            && rin.NumeroUnicoID == referenciaID
                                            select rin.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 10: // Despacho
                            incidenciaID = (from rid in ctx.Sam3_Rel_Incidencia_Despacho
                                            where rid.Activo
                                            && rid.DespachoID == referenciaID
                                            select rid.IncidenciaID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        case 11: // Corte
                            incidenciaID = (from ric in ctx.Sam3_Corte
                                            where ric.Activo
                                            && ric.CorteID == referenciaID
                                            select ric.CorteID).AsParallel().SingleOrDefault();

                            detalle = (Incidencia)DetalleIncidencia(incidenciaID, usuario);
                            break;
                        default:
                            throw new Exception("No se encontro el tipo de incidencia");
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

        public object TiposIncidencias(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TipoIncidencia> listado = (from tpi in ctx.Sam3_TipoIncidencia
                                                    where tpi.Activo
                                                    select new TipoIncidencia
                                                    {
                                                        Nombre = tpi.Nombre,
                                                        TipoIncidenciaID = tpi.TipoIncidenciaID.ToString()
                                                    }).AsParallel().Distinct().ToList();

                    return listado.OrderBy(x => x.Nombre).ToList();
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

        public object Clasificaciones(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Clasificacion> listado = (from c in ctx.Sam3_ClasificacionIncidencia
                                                   where c.Activo
                                                   select new Clasificacion
                                                   {
                                                       ClasificacionID = c.ClasificacionIncidenciaID.ToString(),
                                                       Nombre = c.Nombre
                                                   }).AsParallel().ToList();

                    return listado;
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

        public object IncidenciaBilingue(int tipoIncidenciaID, int registroID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ProyectoIncidencia> listado = new List<ProyectoIncidencia>();
                    List<ProyectoIncidencia> listadoAux = new List<ProyectoIncidencia>();

                    switch (tipoIncidenciaID)
                    {
                        case 1: //Folio Aviso Entrada

                            listado = (from fa in ctx.Sam3_FolioAvisoLlegada
                                       join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                       join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                       where fa.Activo && rfp.Activo && p.Activo
                                        && fa.FolioAvisoLlegadaID == registroID


                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();

                            break;
                        case 2: // Entrada de Material
                            listado = (from fe in ctx.Sam3_FolioAvisoEntrada
                                       join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                       join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                       where fe.Activo && rfp.Activo && p.Activo
                                        && fe.FolioAvisoEntradaID == registroID

                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();


                            break;
                        case 3: // Pase Salida. Por el momento sin implementacion
                            break;
                        case 4: // Packing List
                            listado = (from fc in ctx.Sam3_FolioCuantificacion
                                       join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                       join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                       join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                       join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                       where fc.Activo && fe.Activo && fa.Activo && rfp.Activo && p.Activo
                                       && fc.FolioCuantificacionID == registroID

                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();


                            break;
                        case 5: // Orden de recepcion
                            listado = (from o in ctx.Sam3_OrdenRecepcion
                                       join rofe in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rofe.OrdenRecepcionID
                                       join fe in ctx.Sam3_FolioAvisoEntrada on rofe.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                       join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                       join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                       where o.Activo && rofe.Activo && fe.Activo && rfp.Activo && p.Activo
                                        && o.Folio == registroID

                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();

                            break;
                        case 6: // Complemento de recepcion. Por el momento sin implementacion
                            break;
                        case 7: // ItemCode
                            listado = (from it in ctx.Sam3_ItemCode
                                       join p in ctx.Sam3_ProyectoConfiguracion on it.ProyectoID equals p.ProyectoID
                                       join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                       join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                       join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                       where it.Activo && p.Activo 
                                       && rid.Rel_ItemCode_Diametro_ID == registroID
                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();
                            break;
                        case 8: // Orden de almacenaje
                            listado = (from oa in ctx.Sam3_OrdenAlmacenaje
                                       join relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on oa.OrdenAlmacenajeID equals relnu.OrdenAlmacenajeID
                                       join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on relnu.Rel_FolioCuantificacion_ItemCode_ID equals rfi.Rel_FolioCuantificacion_ItemCode_ID
                                       join fc in ctx.Sam3_FolioCuantificacion on rfi.FolioCuantificacionID equals fc.FolioCuantificacionID
                                       join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                       join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                       join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                       where oa.Activo && relnu.Activo && rfi.Activo && fc.Activo && fe.Activo && rfp.Activo && p.Activo
                                       && oa.Folio == registroID
                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();

                            listadoAux = (from oa in ctx.Sam3_OrdenAlmacenaje
                                          join relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on oa.OrdenAlmacenajeID equals relnu.OrdenAlmacenajeID
                                          join rbi in ctx.Sam3_Rel_Bulto_ItemCode on relnu.Rel_Bulto_ItemCode_ID equals rbi.Rel_Bulto_ItemCode_ID
                                          join b in ctx.Sam3_Bulto on rbi.BultoID equals b.BultoID
                                          join fc in ctx.Sam3_FolioCuantificacion on b.FolioCuantificacionID equals fc.FolioCuantificacionID
                                          join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                          join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                          join p in ctx.Sam3_ProyectoConfiguracion on rfp.ProyectoID equals p.ProyectoID
                                          where oa.Activo && relnu.Activo && rbi.Activo && fc.Activo && fe.Activo && rfp.Activo && p.Activo && b.Activo
                                          && oa.Folio == registroID
                                          select new ProyectoIncidencia
                                          {
                                              ProyectoID = p.ProyectoID,
                                              IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                          }).AsParallel().Distinct().ToList();

                            if (listadoAux.Count > listado.Count)
                            {
                                listado = listadoAux;
                            }


                            break;
                        case 9: // Numero unico

                            listado = (from nu in ctx.Sam3_NumeroUnico
                                       join p in ctx.Sam3_ProyectoConfiguracion on nu.ProyectoID equals p.ProyectoID
                                       where nu.Activo && p.Activo
                                       && nu.NumeroUnicoID == registroID
                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();

                            break;
                        case 10: // Despacho
                            listado = (from d in ctx.Sam3_Despacho
                                       join p in ctx.Sam3_ProyectoConfiguracion on d.ProyectoID equals p.ProyectoID
                                       where d.Activo && p.Activo
                                      && d.DespachoID == registroID
                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();
                            break;
                        case 11: // Corte
                            listado = (from c in ctx.Sam3_Corte
                                       join p in ctx.Sam3_ProyectoConfiguracion on c.ProyectoID equals p.ProyectoID
                                       where c.Activo && p.Activo 
                                        && c.CorteID == registroID
                                       select new ProyectoIncidencia
                                       {
                                           ProyectoID = p.ProyectoID,
                                           IncidenciaBilingue = p.RequiereIncidenciaBilingue
                                       }).AsParallel().Distinct().ToList();
                            break;
                        default:
                            throw new Exception("No se encontro el tipo de incidencia");
                    }

                    return listado;
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
    }
}