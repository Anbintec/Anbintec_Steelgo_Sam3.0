using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Script.Serialization;
using System.Transactions;
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    public class CorteBd
    {
        private static readonly object _mutex = new object();
        private static CorteBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CorteBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static CorteBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CorteBd();
                    }
                }
                return _instance;
            }
        }

        public object ListadoGenerarCorte(string prefijo, int consecutivo, string segmento, Sam3_Usuario usuario)
        {
            try
            {
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                List<DatosBusquedaODT> listado = new List<DatosBusquedaODT>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {

                        proyectos = (from p in ctx.Sam3_Rel_Usuario_Proyecto
                                     join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                     where p.Activo && eqp.Activo
                                     && p.UsuarioID == usuario.UsuarioID
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();


                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam3_PatioID
                                  join up in ctx.Sam3_Rel_Usuario_Proyecto on p.ProyectoID equals up.ProyectoID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && up.UsuarioID == usuario.UsuarioID
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();

                        //buscamos el numero unico

                        Sam3_NumeroUnico numeroUnicoCorte = (from nu in ctx.Sam3_NumeroUnico
                                                             where nu.Activo
                                                             && nu.Prefijo == prefijo
                                                             && nu.Consecutivo == consecutivo
                                                             && nu.Estatus == "A"
                                                             select nu).AsParallel().SingleOrDefault();

                        //buscamos su equivalente en SAM 2
                        int sam2_numeroUnicoID = (from nueq in ctx.Sam3_EquivalenciaNumeroUnico
                                                  where nueq.Activo && nueq.Sam3_NumeroUnicoID == numeroUnicoCorte.NumeroUnicoID
                                                  select nueq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                        NumeroUnico sam2_NumeroUnico = (from nu in ctx2.NumeroUnico
                                                        where nu.NumeroUnicoID == sam2_numeroUnicoID
                                                        && nu.Estatus == "A"
                                                        select nu).AsParallel().SingleOrDefault();

                        Sam3_ProyectoConfiguracion configuracion = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == numeroUnicoCorte.ProyectoID)
                            .AsParallel().SingleOrDefault();


                        listado = (from odtm in ctx2.OrdenTrabajoMaterial
                                   join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                   join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                   join nu in ctx2.NumeroUnico on odtm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                   join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                   where odtm.NumeroUnicoDespachadoID == sam2_NumeroUnico.NumeroUnicoID
                                   && odtm.NumeroUnicoCongeladoID == null
                                   && it.TipoMaterialID == 1
                                   select new DatosBusquedaODT
                                   {
                                       Cantidad = odtm.CantidadDespachada.Value,
                                       CantidadIngenieria = ms.Cantidad,
                                       SpoolID = odts.NumeroControl,
                                       Etiqueta = ms.Etiqueta,
                                       MaterialSpoolID = ms.MaterialSpoolID,
                                       OrdenTrbajoSpoolID = odts.OrdenTrabajoSpoolID,
                                       NumerUnicoID = nu.NumeroUnicoID.ToString(),
                                       CantidadBak = odtm.CantidadDespachada.Value
                                   }).Distinct().AsParallel().ToList();

                        foreach (DatosBusquedaODT item in listado)
                        {
                            item.CorteDetalleID = (from cd in ctx.Sam3_CorteDetalle
                                                   where cd.MaterialSpoolID == item.MaterialSpoolID
                                                   && cd.OrdenTrabajoSpoolID == item.OrdenTrbajoSpoolID
                                                   && cd.Cantidad == item.Cantidad
                                                   && cd.Activo
                                                   select cd.CorteDetalleID).AsParallel().Distinct().SingleOrDefault();
                        }


                    }// fin sam2
                }
                return listado;
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

        public object GenerarCorte(GuardarCorte corte, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Corte nuevoCorte = new Sam3_Corte();
                List<Sam3_CorteDetalle> detalleCorte = new List<Sam3_CorteDetalle>();
                int totalCorte = 0;
                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var sam2_tran = ctx2.Database.BeginTransaction())
                            {
                                int numeroUnicoID = Convert.ToInt32(corte.NumeroUnico);

                                #region Edicion
                                //si alguno de los detalles tiene ya un corte detalle asignado
                                if (corte.Detalle.Where(x => x.CorteDetalleID > 0).Any())
                                {
                                    List<DetalleCortes> cortesExistentes = corte.Detalle.Where(x => x.CorteDetalleID > 0).AsParallel().ToList();
                                    foreach (DetalleCortes item in cortesExistentes)
                                    {
                                        int cantidad = Convert.ToInt32(item.Cantidad);
                                        Sam3_CorteDetalle detalle = ctx.Sam3_CorteDetalle.Where(x => x.CorteDetalleID == item.CorteDetalleID && x.Activo)
                                            .AsParallel().SingleOrDefault();
                                        int cantidadOriginal = 0;

                                        if (detalle.Cantidad != cantidad)
                                        {
                                            cantidadOriginal = detalle.Cantidad;
                                            detalle.Cantidad = cantidad;
                                            detalle.FechaCorte = DateTime.Now;
                                            detalle.FechaModificacion = DateTime.Now;
                                            detalle.UsuarioModificacion = usuario.UsuarioID;

                                            Sam3_Corte corteOriginal = ctx.Sam3_Corte.Where(x => x.CorteID == detalle.CorteID && x.Activo).AsParallel().SingleOrDefault();
                                            corteOriginal.FechaModificacion = DateTime.Now;
                                            corteOriginal.Sobrante = cantidadOriginal > cantidad ? //es menor al corte anterior
                                                corteOriginal.Sobrante + (cantidadOriginal - cantidad) :
                                                corteOriginal.Sobrante - (cantidad - cantidadOriginal);
                                            corteOriginal.UsuarioModificacion = usuario.UsuarioID;

                                            int despachoOriginal = ctx2.OrdenTrabajoMaterial.Where(x => x.OrdenTrabajoSpoolID == detalle.OrdenTrabajoSpoolID
                                                && x.MaterialSpoolID == detalle.MaterialSpoolID).Select(x => x.DespachoID.Value).AsParallel().SingleOrDefault();

                                            Sam3_Despacho despacho = ctx.Sam3_Despacho.Where(x => x.DespachoID == despachoOriginal && x.Activo).AsParallel().SingleOrDefault();
                                            despacho.Cantidad = cantidad;
                                            despacho.FechaModificacion = DateTime.Now;
                                            despacho.UsuarioModificacion = usuario.UsuarioID;

                                            Sam3_NumeroUnicoMovimiento movimiento = ctx.Sam3_NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == despacho.SalidaInventarioID
                                                && x.Activo).AsParallel().SingleOrDefault();
                                            movimiento.Cantidad = cantidad;
                                            movimiento.FechaModificacion = DateTime.Now;
                                            movimiento.UsuarioModificacion = usuario.UsuarioID;

                                            Sam3_NumeroUnicoInventario sam3_inventario = ctx.Sam3_NumeroUnicoInventario.Where(x => x.NumeroUnicoID == despacho.NumeroUnicoID)
                                                .AsParallel().SingleOrDefault();
                                            //Sumamos o restamos la diferencia
                                            sam3_inventario.InventarioFisico = cantidadOriginal > cantidad ?
                                                sam3_inventario.InventarioFisico + (cantidadOriginal - cantidad) : // la cantidad original es mayor a la edicion
                                                sam3_inventario.InventarioFisico - (cantidad - cantidadOriginal); // la cantidad en edicion es mayor a la cantidad original
                                            sam3_inventario.InventarioBuenEstado = sam3_inventario.InventarioFisico - sam3_inventario.CantidadDanada;
                                            sam3_inventario.InventarioDisponibleCruce = sam3_inventario.InventarioBuenEstado - sam3_inventario.InventarioCongelado;

                                            Sam3_NumeroUnicoSegmento sam3_segmento = ctx.Sam3_NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == sam3_inventario.NumeroUnicoID
                                                && x.Activo && x.Segmento == "A").AsParallel().SingleOrDefault();
                                            sam3_segmento.InventarioFisico = cantidadOriginal > cantidad ?
                                                sam3_segmento.InventarioFisico + (cantidadOriginal - cantidad) : // la cantidad original es mayor a la edicion
                                                sam3_segmento.InventarioFisico - (cantidad - cantidadOriginal); // la cantidad en edicion es mayor a la cantidad original
                                            sam3_segmento.InventarioBuenEstado = sam3_segmento.InventarioFisico - sam3_segmento.CantidadDanada;
                                            sam3_segmento.InventarioDisponibleCruce = sam3_segmento.InventarioBuenEstado - sam3_segmento.InventarioCongelado;


                                            if (sam3_inventario.InventarioCongelado < 0 || sam3_inventario.InventarioDisponibleCruce < 0)
                                            {
                                                throw new Exception("No existe suficiente material para el corte");
                                            }

                                            //ahora el inventario en sam2
                                            int sam2_numeroUnicoId = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == sam3_inventario.NumeroUnicoID
                                                && x.Activo).Select(x => x.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                            NumeroUnicoInventario sam2_inventario = ctx2.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == sam2_numeroUnicoId)
                                                .AsParallel().SingleOrDefault();
                                            sam2_inventario.InventarioFisico = cantidadOriginal > cantidad ?
                                               sam2_inventario.InventarioFisico + (cantidadOriginal - cantidad) : // la cantidad original es mayor a la edicion
                                               sam2_inventario.InventarioFisico - (cantidad - cantidadOriginal); // la cantidad en edicion es mayor a la cantidad original
                                            sam2_inventario.InventarioBuenEstado = sam2_inventario.InventarioFisico - sam2_inventario.CantidadDanada;
                                            sam2_inventario.InventarioDisponibleCruce = sam2_inventario.InventarioBuenEstado - sam2_inventario.InventarioCongelado;

                                            NumeroUnicoSegmento sam2_segmentoOrginal = ctx2.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == sam2_numeroUnicoId && x.Segmento == "A")
                                                .AsParallel().SingleOrDefault();
                                            sam2_segmentoOrginal.InventarioFisico = cantidadOriginal > cantidad ?
                                               sam2_segmentoOrginal.InventarioFisico + (cantidadOriginal - cantidad) : // la cantidad original es mayor a la edicion
                                               sam2_segmentoOrginal.InventarioFisico - (cantidad - cantidadOriginal); // la cantidad en edicion es mayor a la cantidad original
                                            sam2_segmentoOrginal.InventarioBuenEstado = sam2_segmentoOrginal.InventarioFisico - sam2_segmentoOrginal.CantidadDanada;
                                            sam2_segmentoOrginal.InventarioDisponibleCruce = sam2_segmentoOrginal.InventarioBuenEstado - sam2_segmentoOrginal.InventarioCongelado;

                                            OrdenTrabajoMaterial odtmOrigial = ctx2.OrdenTrabajoMaterial.Where(x => x.MaterialSpoolID == despacho.MaterialSpoolID
                                                && x.OrdenTrabajoSpoolID == despacho.OrdenTrabajoSpoolID).AsParallel().SingleOrDefault();
                                            odtmOrigial.CantidadDespachada = cantidad;
                                            odtmOrigial.FechaModificacion = DateTime.Now;

                                            if (odtmOrigial.DespachoID <= 0 || odtmOrigial.DespachoID == null
                                            || odtmOrigial.CantidadDespachada <= 0 || odtmOrigial.CantidadDespachada == null)
                                            {
                                                throw new Exception("Error al actualizar la orden de trabajo material: ID: " + odtmOrigial.OrdenTrabajoMaterialID.ToString());
                                            }

                                            ctx.SaveChanges();
                                            ctx2.SaveChanges();
                                        }
                                    }
                                }
                                #endregion

                                // Si hay detalles que aun no tengan cortes
                                if (corte.Detalle.Where(x => x.CorteDetalleID == 0 || x.CorteDetalleID < 0).Any())
                                {
                                    //recuperamos los numeros unicos con inventarios
                                    //sam3
                                    Sam3_NumeroUnico numeroUnicoCorte = ctx.Sam3_NumeroUnico
                                        .Include("Sam3_NumeroUnicoInventario")
                                        .Include("Sam3_NumeroUnicoSegmento")
                                        .Include("Sam3_NumeroUnicoMovimiento")
                                        .Where(x => x.NumeroUnicoID == numeroUnicoID).AsParallel().SingleOrDefault();



                                    //sam2
                                    int sam2_numeroUnicoID = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                              where eq.Activo && eq.Sam3_NumeroUnicoID == numeroUnicoID
                                                              select eq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                    NumeroUnico sam2_numeroUnicoCorte = ctx2.NumeroUnico
                                        .Include("NumeroUnicoInventario")
                                        .Include("NumeroUnicoSegmento")
                                        .Include("NumeroUnicoMovimiento")
                                        .Where(x => x.NumeroUnicoID == sam2_numeroUnicoID).AsParallel().SingleOrDefault();




                                    //Generar movimientos de inventario
                                    //Movimeinto de Preparacion a corte
                                    Sam3_NumeroUnicoMovimiento movimientoPreparacionCorte = new Sam3_NumeroUnicoMovimiento();
                                    movimientoPreparacionCorte.Activo = true;
                                    movimientoPreparacionCorte.Cantidad = numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioFisico;
                                    movimientoPreparacionCorte.Estatus = "A";
                                    movimientoPreparacionCorte.FechaModificacion = DateTime.Now;
                                    movimientoPreparacionCorte.FechaMovimiento = DateTime.Now;
                                    movimientoPreparacionCorte.NumeroUnicoID = numeroUnicoCorte.NumeroUnicoID;
                                    movimientoPreparacionCorte.ProyectoID = numeroUnicoCorte.ProyectoID;
                                    movimientoPreparacionCorte.Segmento = corte.Segmento;
                                    movimientoPreparacionCorte.TipoMovimientoID = (from tpm in ctx.Sam3_TipoMovimiento
                                                                                   where tpm.Activo && tpm.Nombre == "Preparación para Corte"
                                                                                   select tpm.TipoMovimientoID).AsParallel().SingleOrDefault();
                                    movimientoPreparacionCorte.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_NumeroUnicoMovimiento.Add(movimientoPreparacionCorte);
                                    ctx.SaveChanges();

                                    int Sam3_MovimientoPreparacionID = movimientoPreparacionCorte.NumeroUnicoMovimientoID;
                                    int Sam3_MovimientoMermaID = 0;
                                    if (Convert.ToBoolean(corte.TramoCompleto))
                                    {
                                        Sam3_NumeroUnicoMovimiento movimientoMerma = new Sam3_NumeroUnicoMovimiento();
                                        movimientoMerma.Activo = true;
                                        movimientoMerma.Cantidad = Convert.ToInt32(corte.Merma);
                                        movimientoMerma.Estatus = "A";
                                        movimientoMerma.FechaModificacion = DateTime.Now;
                                        movimientoMerma.FechaMovimiento = DateTime.Now;
                                        movimientoMerma.NumeroUnicoID = numeroUnicoCorte.NumeroUnicoID;
                                        movimientoMerma.ProyectoID = numeroUnicoCorte.ProyectoID;
                                        movimientoMerma.Segmento = corte.Segmento;
                                        movimientoMerma.TipoMovimientoID = (from tpm in ctx.Sam3_TipoMovimiento
                                                                            where tpm.Activo && tpm.Nombre == "Merma"
                                                                            select tpm.TipoMovimientoID).AsParallel().SingleOrDefault();
                                        movimientoMerma.UsuarioModificacion = usuario.UsuarioID;
                                        ctx.Sam3_NumeroUnicoMovimiento.Add(movimientoMerma);
                                        ctx.SaveChanges();
                                        Sam3_MovimientoMermaID = movimientoMerma.NumeroUnicoMovimientoID;
                                    }

                                    string rack = (from nu in ctx.Sam3_NumeroUnico
                                                   where nu.Activo &&
                                                   nu.NumeroUnicoID == numeroUnicoID
                                                   select nu.Rack).AsParallel().SingleOrDefault();

                                    //generamos el nuevo corte
                                    nuevoCorte.Activo = true;
                                    nuevoCorte.Cancelado = false;
                                    nuevoCorte.FechaModificacion = DateTime.Now;
                                    nuevoCorte.Merma = Convert.ToInt32(corte.Merma);
                                    if (Convert.ToBoolean(corte.TramoCompleto))
                                    {
                                        nuevoCorte.MermaMovimientoID = Sam3_MovimientoMermaID;
                                    }
                                    //nuevoCorte.NumeroUnicoCorteID = numeroUnicoCorte.NumeroUnicoID;
                                    nuevoCorte.PreparacionCorteMovimientoID = Sam3_MovimientoPreparacionID;
                                    nuevoCorte.ProyectoID = numeroUnicoCorte.ProyectoID;
                                    nuevoCorte.Rack = rack;
                                    nuevoCorte.Sobrante = Convert.ToInt32(corte.Sobrante);
                                    nuevoCorte.UsuarioModificacion = usuario.UsuarioID;


                                    ctx.Sam3_Corte.Add(nuevoCorte);
                                    ctx.SaveChanges();

                                    foreach (DetalleCortes detalle in corte.Detalle)
                                    {
                                        if (detalle.CorteDetalleID > 0)
                                        {
                                            continue; // si ya existe un corte detalle no debe crearse uno nuevo
                                        }

                                        totalCorte += Convert.ToInt32(detalle.Cantidad);
                                        //buscamos las ordenes de trabajo material
                                        OrdenTrabajoMaterial odtsMaterial = (from odts in ctx2.OrdenTrabajoSpool
                                                                             join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                                             join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                                             where odts.NumeroControl == detalle.SpoolID
                                                                             && ms.Etiqueta == detalle.Etiqueta
                                                                             select odtm).Distinct().AsParallel().SingleOrDefault();



                                        //verificamos si el numero unico que se esta despachando es el mismo que estaba congelado para orden
                                        if (odtsMaterial.NumeroUnicoCongeladoID != sam2_numeroUnicoID)
                                        {
                                            //buscamos en sam2 el numero unico que estaba congelado
                                            NumeroUnico numeroCongelado = ctx2.NumeroUnico
                                                .Include("NumeroUnicoInventario")
                                                .Include("NumeroUnicoSegmento")
                                                .Where(x => x.NumeroUnicoID == odtsMaterial.NumeroUnicoCongeladoID)
                                                .AsParallel().SingleOrDefault();

                                            //quitamos los congelados y devolvemos el inventario
                                            numeroCongelado.NumeroUnicoInventario.InventarioCongelado -= Convert.ToInt32(detalle.Cantidad);
                                            numeroCongelado.NumeroUnicoInventario.InventarioFisico += Convert.ToInt32(detalle.Cantidad);
                                            numeroCongelado.NumeroUnicoInventario.InventarioBuenEstado += Convert.ToInt32(detalle.Cantidad);
                                            numeroCongelado.NumeroUnicoInventario.InventarioDisponibleCruce += Convert.ToInt32(detalle.Cantidad);
                                            numeroCongelado.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                            NumeroUnicoSegmento segmentoCongelado = numeroCongelado.NumeroUnicoSegmento.Where(x => x.Segmento == corte.Segmento)
                                                .SingleOrDefault();
                                            segmentoCongelado.InventarioCongelado -= Convert.ToInt32(detalle.Cantidad);
                                            segmentoCongelado.InventarioBuenEstado += Convert.ToInt32(detalle.Cantidad);
                                            segmentoCongelado.InventarioDisponibleCruce += Convert.ToInt32(detalle.Cantidad);
                                            segmentoCongelado.InventarioFisico += Convert.ToInt32(detalle.Cantidad);
                                            segmentoCongelado.FechaModificacion = DateTime.Now;

                                            ctx2.SaveChanges();

                                            //buscamos el inventario en sam3
                                            Sam3_NumeroUnico sam3_NUcongeldo = (from nu in ctx.Sam3_NumeroUnico
                                                                                join nueq in ctx.Sam3_EquivalenciaNumeroUnico on nu.NumeroUnicoID equals nueq.Sam3_NumeroUnicoID
                                                                                where nu.Activo && nueq.Activo
                                                                                && nueq.Sam2_NumeroUnicoID == numeroCongelado.NumeroUnicoID
                                                                                select nu).AsParallel().SingleOrDefault();

                                            //sam3_NUcongeldo.Sam3_NumeroUnicoInventario.InventarioCongelado -= Convert.ToInt32(detalle.Cantidad);
                                            sam3_NUcongeldo.Sam3_NumeroUnicoInventario.InventarioBuenEstado += Convert.ToInt32(detalle.Cantidad);
                                            sam3_NUcongeldo.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce += Convert.ToInt32(detalle.Cantidad);
                                            sam3_NUcongeldo.Sam3_NumeroUnicoInventario.InventarioFisico += Convert.ToInt32(detalle.Cantidad);
                                            sam3_NUcongeldo.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                                            sam3_NUcongeldo.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuario.UsuarioID;

                                            Sam3_NumeroUnicoSegmento sam3_segmentoC = sam3_NUcongeldo.Sam3_NumeroUnicoSegmento.Where(x => x.Segmento == corte.Segmento).AsParallel().SingleOrDefault();
                                            sam3_segmentoC.InventarioCongelado -= Convert.ToInt32(detalle.Cantidad);
                                            sam3_segmentoC.InventarioDisponibleCruce += Convert.ToInt32(detalle.Cantidad);
                                            sam3_segmentoC.InventarioBuenEstado += Convert.ToInt32(detalle.Cantidad);
                                            sam3_segmentoC.InventarioFisico += Convert.ToInt32(detalle.Cantidad);
                                            sam3_segmentoC.FechaModificacion = DateTime.Now;
                                            sam3_segmentoC.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.SaveChanges();
                                        }

                                        //generamos un nuevo movimiento de corte
                                        Sam3_NumeroUnicoMovimiento nuevoMovimiento = new Sam3_NumeroUnicoMovimiento();
                                        nuevoMovimiento.Activo = true;
                                        nuevoMovimiento.Cantidad = Convert.ToInt32(detalle.Cantidad);
                                        nuevoMovimiento.Estatus = "A";
                                        nuevoMovimiento.FechaModificacion = DateTime.Now;
                                        nuevoMovimiento.FechaMovimiento = DateTime.Now;
                                        nuevoMovimiento.NumeroUnicoID = numeroUnicoCorte.NumeroUnicoID;
                                        nuevoMovimiento.ProyectoID = numeroUnicoCorte.ProyectoID;
                                        nuevoMovimiento.Referencia = (from odts in ctx2.OrdenTrabajoSpool
                                                                      where odts.OrdenTrabajoSpoolID == odtsMaterial.OrdenTrabajoSpoolID
                                                                      select odts.NumeroControl).AsParallel().SingleOrDefault();
                                        nuevoMovimiento.Segmento = corte.Segmento;
                                        nuevoMovimiento.TipoMovimientoID = (from tpm in ctx.Sam3_TipoMovimiento
                                                                            where tpm.Activo && tpm.Nombre == "Corte"
                                                                            select tpm.TipoMovimientoID).AsParallel().SingleOrDefault();

                                        nuevoMovimiento.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.Sam3_NumeroUnicoMovimiento.Add(nuevoMovimiento);
                                        ctx.SaveChanges();

                                        //generamos un nuevo detalle de corte
                                        Sam3_CorteDetalle nuevoDetalle = new Sam3_CorteDetalle();
                                        nuevoDetalle.Activo = true;
                                        nuevoDetalle.Cancelado = false;
                                        nuevoDetalle.Cantidad = Convert.ToInt32(detalle.Cantidad);
                                        nuevoDetalle.CorteID = nuevoCorte.CorteID;
                                        nuevoDetalle.EsAjuste = false;
                                        nuevoDetalle.FechaCorte = DateTime.Now;
                                        nuevoDetalle.FechaModificacion = DateTime.Now;
                                        if (corte.Maquina != "" && corte.Maquina != null)
                                        {
                                            nuevoDetalle.MaquinaID = Convert.ToInt32(corte.Maquina);
                                        }
                                        nuevoDetalle.MaterialSpoolID = odtsMaterial.MaterialSpoolID;
                                        nuevoDetalle.OrdenTrabajoSpoolID = odtsMaterial.OrdenTrabajoSpoolID;
                                        nuevoDetalle.SalidaInventarioID = nuevoMovimiento.NumeroUnicoMovimientoID;
                                        nuevoDetalle.UsuarioModificacion = usuario.UsuarioID;


                                        ctx.Sam3_CorteDetalle.Add(nuevoDetalle);

                                        //generamos el despacho
                                        Sam3_Despacho nuevoDespacho = new Sam3_Despacho();
                                        nuevoDespacho.Activo = true;
                                        nuevoDespacho.Cancelado = false;
                                        nuevoDespacho.Cantidad = Convert.ToInt32(detalle.Cantidad);
                                        nuevoDespacho.EsEquivalente = false;
                                        nuevoDespacho.FechaDespacho = DateTime.Now;
                                        nuevoDespacho.FechaModificacion = DateTime.Now;
                                        nuevoDespacho.MaterialSpoolID = odtsMaterial.MaterialSpoolID;
                                        nuevoDespacho.NumeroUnicoID = numeroUnicoCorte.NumeroUnicoID;
                                        nuevoDespacho.OrdenTrabajoSpoolID = odtsMaterial.OrdenTrabajoSpoolID;
                                        nuevoDespacho.ProyectoID = numeroUnicoCorte.ProyectoID;
                                        nuevoDespacho.Segmento = corte.Segmento;
                                        nuevoDespacho.UsuarioModificacion = usuario.UsuarioID;
                                        nuevoDespacho.SalidaInventarioID = nuevoMovimiento.NumeroUnicoMovimientoID;


                                        ctx.Sam3_Despacho.Add(nuevoDespacho);
                                        ctx.SaveChanges();

                                        #region Generar Picking Ticket
                                        Sam3_FolioPickingTicket nuevoPickingTicket;
                                        Sam3_DetalleFolioPickingTicket nuevoDetalleFolioPK;
                                        //verificamos si existe el Picking ticket
                                        if (!ctx.Sam3_FolioPickingTicket.Where(x => x.OrdenTrabajoSpoolID == odtsMaterial.OrdenTrabajoSpoolID && x.Activo).AsParallel().Any())
                                        {
                                            //si no existe creamos el folio
                                            nuevoPickingTicket = new Sam3_FolioPickingTicket();
                                            nuevoPickingTicket.Activo = true;
                                            nuevoPickingTicket.FechaModificacion = DateTime.Now;
                                            nuevoPickingTicket.usuarioModificacion = usuario.UsuarioID;
                                            nuevoPickingTicket.OrdenTrabajoSpoolID = odtsMaterial.OrdenTrabajoSpoolID;

                                            ctx.Sam3_FolioPickingTicket.Add(nuevoPickingTicket);
                                            ctx.SaveChanges();

                                        }
                                        else
                                        {
                                            // traemos el registro del folio
                                            nuevoPickingTicket = ctx.Sam3_FolioPickingTicket.Where(x => x.OrdenTrabajoSpoolID == odtsMaterial.OrdenTrabajoSpoolID && x.Activo).AsParallel().SingleOrDefault();
                                        }

                                        //Buscamos el detalle
                                        if (!ctx.Sam3_DetalleFolioPickingTicket.Where(x => x.OrdenTrabajoMaterialID == odtsMaterial.OrdenTrabajoMaterialID && x.Activo).AsParallel().Any())
                                        {
                                            // si no existe lo agregamo
                                            nuevoDetalleFolioPK = new Sam3_DetalleFolioPickingTicket
                                            {
                                                Activo = true,
                                                FechaModificacion = DateTime.Now,
                                                OrdenTrabajoMaterialID = odtsMaterial.OrdenTrabajoMaterialID,
                                                FolioPickingTicketID = nuevoPickingTicket.FolioPickingTicketID,
                                                UsuarioModificacion = usuario.UsuarioID,
                                                DespachoID = nuevoDespacho.DespachoID
                                            };

                                            ctx.Sam3_DetalleFolioPickingTicket.Add(nuevoDetalleFolioPK);
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            // no hay implementación para el caso de existir el detalle
                                        }

                                        #endregion

                                        odtsMaterial.TieneCorte = true;
                                        odtsMaterial.TieneDespacho = true;
                                        odtsMaterial.CorteDetalleID = nuevoDetalle.CorteDetalleID;
                                        odtsMaterial.DespachoID = nuevoDespacho.DespachoID;

                                        odtsMaterial.CantidadDespachada = nuevoDespacho.Cantidad;
                                        odtsMaterial.NumeroUnicoDespachadoID = sam2_numeroUnicoCorte.NumeroUnicoID;
                                        odtsMaterial.SegmentoDespachado = corte.Segmento;
                                        odtsMaterial.SegmentoCongelado = null;
                                        odtsMaterial.CantidadCongelada = null;
                                        odtsMaterial.NumeroUnicoCongeladoID = null;
                                        odtsMaterial.NumeroUnicoSugeridoID = null;
                                        odtsMaterial.SegmentoSugerido = null;
                                        odtsMaterial.SugeridoEsEquivalente = false;
                                        odtsMaterial.DespachoEsEquivalente = false;
                                        odtsMaterial.CongeladoEsEquivalente = false;
                                        odtsMaterial.TieneInventarioCongelado = false;
                                        odtsMaterial.FechaModificacion = DateTime.Now;

                                        if (odtsMaterial.DespachoID <= 0 || odtsMaterial.DespachoID == null
                                            || odtsMaterial.CantidadDespachada <= 0 || odtsMaterial.CantidadDespachada == null)
                                        {
                                            throw new Exception("Error al actualizar la orden de trabajo material: ID: " + odtsMaterial.OrdenTrabajoMaterialID.ToString());
                                        }

                                        ctx2.SaveChanges();
                                    }

                                    //Actualizar inventarios
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioFisico = Convert.ToInt32(corte.Sobrante);
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioBuenEstado = Convert.ToInt32(corte.Sobrante);
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioCongelado -= totalCorte;
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce =
                                        numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioBuenEstado - numeroUnicoCorte.Sam3_NumeroUnicoInventario.InventarioCongelado;
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                                    numeroUnicoCorte.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuario.UsuarioID;

                                    Sam3_NumeroUnicoSegmento segmento = numeroUnicoCorte.Sam3_NumeroUnicoSegmento.Where(x => x.Segmento == corte.Segmento).SingleOrDefault();
                                    segmento.InventarioBuenEstado = Convert.ToInt32(corte.Sobrante);
                                    segmento.InventarioFisico = Convert.ToInt32(corte.Sobrante);
                                    segmento.InventarioCongelado -= totalCorte;
                                    segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                                    segmento.FechaModificacion = DateTime.Now;
                                    segmento.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.SaveChanges();

                                    //Actualizar sam2
                                    sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioFisico = Convert.ToInt32(corte.Sobrante);
                                    sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioBuenEstado = Convert.ToInt32(corte.Sobrante);
                                    sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioCongelado -= totalCorte;
                                    sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioDisponibleCruce =
                                        sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioBuenEstado - sam2_numeroUnicoCorte.NumeroUnicoInventario.InventarioCongelado;
                                    sam2_numeroUnicoCorte.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                    NumeroUnicoSegmento sam2_segmento = sam2_numeroUnicoCorte.NumeroUnicoSegmento.Where(x => x.Segmento == corte.Segmento).SingleOrDefault();
                                    sam2_segmento.InventarioBuenEstado = Convert.ToInt32(corte.Sobrante);
                                    sam2_segmento.InventarioFisico = Convert.ToInt32(corte.Sobrante);
                                    sam2_segmento.InventarioCongelado -= totalCorte;
                                    sam2_segmento.InventarioDisponibleCruce = sam2_segmento.InventarioBuenEstado - sam2_segmento.InventarioCongelado;
                                    sam2_segmento.FechaModificacion = DateTime.Now;

                                    ctx2.SaveChanges();

                                }
                                sam2_tran.Commit();
                                sam3_tran.Commit();
                            } // tran sam2
                        } //using ctx2
                    }// tran sam3
                }// using ctx

                corte.CorteID = nuevoCorte.CorteID.ToString();
                return corte;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(" No se puede generar el corte");
                if (Convert.ToInt32(corte.Sobrante) < 0)
                {
                    result.ReturnMessage.Add(" El sobrante no puede ser negativo");
                }
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
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Corte> registros = new List<Sam3_Corte>();

                    if (proyectoID > 0)
                    {
                        registros = (from c in ctx.Sam3_Corte
                                     join p in ctx.Sam3_Proyecto on c.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(c.CorteID)
                                     select c).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from c in ctx.Sam3_Corte
                                     join p in ctx.Sam3_Proyecto on c.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && IDs.Contains(c.CorteID)
                                     select c).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                     where p.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join ric in ctx.Sam3_Rel_Incidencia_Corte on r.CorteID equals ric.CorteID
                               join inc in ctx.Sam3_Incidencia on ric.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where ric.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
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

        public object ListadoCorteDesdeImpresion(int materialSpoolID, Sam3_Usuario usuario)
        {
            try
            {
                List<object> resultado = new List<object>();

                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {

                        //buscamos su equivalente en SAM 2
                        int sam2_numeroUnicoID = (from ms in ctx2.MaterialSpool
                                                  join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                                                  join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                  where ms.MaterialSpoolID == materialSpoolID
                                                  select nu.NumeroUnicoID).AsParallel().SingleOrDefault();

                        string segmento = (from ms in ctx2.MaterialSpool
                                           join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                                           where ms.MaterialSpoolID == materialSpoolID
                                           select odtm.SegmentoCongelado).AsParallel().SingleOrDefault();

                        int sam3_numeroUnicoID = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                  where eq.Activo
                                                  && eq.Sam2_NumeroUnicoID == sam2_numeroUnicoID
                                                  select eq.Sam3_NumeroUnicoID).AsParallel().SingleOrDefault();

                        resultado.Add((from nu in ctx.Sam3_NumeroUnico
                                       join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                       where nu.Activo
                                       && nu.NumeroUnicoID == sam3_numeroUnicoID
                                       select new ListaCombos
                                       {
                                           id = p.ProyectoID.ToString(),
                                           value = p.Nombre
                                       }).AsParallel().SingleOrDefault());

                        resultado.Add((from ms in ctx2.MaterialSpool
                                       join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                                       join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                       join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                       join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                       join p in ctx2.ProyectoConfiguracion on it.ProyectoID equals p.ProyectoID
                                       where ms.MaterialSpoolID == materialSpoolID
                                       select new DetalleNumeroUnicoCorte
                                       {
                                           Cantidad = nui.InventarioFisico.ToString(),
                                           D1 = nu.Diametro1.ToString(),
                                           ItemCode = it.Codigo,
                                           Tolerancia = p.ToleranciaCortes.Value.ToString()
                                       }).AsParallel().SingleOrDefault());

                        //resultado.Add((from ms in ctx2.MaterialSpool
                        //               join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                        //               join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                        //               where ms.MaterialSpoolID == materialSpoolID
                        //               select new DetalleOdtsCorte
                        //               {
                        //                   Consecutivo = odts.OrdenTrabajoSpoolID.ToString(),
                        //                   Etiqueta = ms.MaterialSpoolID.ToString(),
                        //                   NumeroControl = odts.OrdenTrabajoID.ToString()
                        //               }).AsParallel().SingleOrDefault());

                        //string[] temp = (resultado[2] as DetalleOdtsCorte).Consecutivo.Split('-').ToArray();
                        //(resultado[2] as DetalleOdtsCorte).Consecutivo = temp[1];
                        //(resultado[2] as DetalleOdtsCorte).NumeroControl = temp[0];

                        resultado.Add((from nu in ctx.Sam3_NumeroUnico
                                       where nu.Activo
                                       && nu.NumeroUnicoID == sam3_numeroUnicoID
                                       select new ListaCombos
                                       {
                                           id = nu.NumeroUnicoID.ToString(),
                                           value = nu.Prefijo + "-" + nu.Consecutivo
                                       }).AsParallel().SingleOrDefault());

                        int numeroDigitos = (from nu in ctx.Sam3_NumeroUnico
                                             join p in ctx.Sam3_ProyectoConfiguracion on nu.ProyectoID equals p.ProyectoID
                                             where nu.NumeroUnicoID == sam3_numeroUnicoID
                                             select p.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                        string formato = "D" + numeroDigitos.ToString();

                        string[] temp = (resultado[2] as ListaCombos).value.Split('-').ToArray();
                        int consecutivo = Convert.ToInt32(temp[1]);
                        (resultado[2] as ListaCombos).value = temp[0] + "-" + consecutivo.ToString(formato) + "-" + segmento;

                        (resultado[1] as DetalleNumeroUnicoCorte).ListadoCortes = (from odtm in ctx2.OrdenTrabajoMaterial
                                                                                   join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                                                   join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                                                   join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                                                   join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                                                   where ms.MaterialSpoolID == materialSpoolID
                                                                                   && it.TipoMaterialID == 1
                                                                                   select new DatosBusquedaODT
                                                                                   {
                                                                                       Cantidad = odtm.CantidadCongelada.Value,
                                                                                       CantidadIngenieria = odtm.CantidadCongelada.Value,
                                                                                       SpoolID = odts.NumeroControl,
                                                                                       Etiqueta = ms.Etiqueta
                                                                                   }).Distinct().AsParallel().ToList();

#if DEBUG
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string json = serializer.Serialize(resultado);
#endif

                    }// fin sam2
                }
                return resultado;
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

        public object EliminarCorte(int corteDetalleID, Sam3_Usuario usuario)
        {
            try
            {
                #region variables
                Sam3_Corte corteSam3;
                Sam3_CorteDetalle corteDetalle;
                Sam3_Despacho despachoSam3;
                Sam3_NumeroUnico numeroUnicoSam3;
                NumeroUnico numeroUnicoSam2;
                OrdenTrabajoMaterial odtm;
                int ordenTrabajoSpoolID = 0;
                MaterialSpool materialSpoolSam2;
                int cantidadInventario = 0;
                bool tieneProcesos = false;
                #endregion
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                #region Eliminar corte de sam 3
                                corteDetalle = ctx.Sam3_CorteDetalle.Where(x => x.CorteDetalleID == corteDetalleID).AsParallel().SingleOrDefault();

                                ordenTrabajoSpoolID = corteDetalle.OrdenTrabajoSpoolID;
                                materialSpoolSam2 = ctx2.MaterialSpool.Where(x => x.MaterialSpoolID == corteDetalle.MaterialSpoolID).AsParallel().SingleOrDefault();
                                cantidadInventario = corteDetalle.Cantidad;

                                tieneProcesos = (from ms in ctx2.MaterialSpool
                                                 join s in ctx2.Spool on ms.SpoolID equals s.SpoolID
                                                 join js in ctx2.JuntaSpool on s.SpoolID equals js.SpoolID
                                                 join jw in ctx2.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                                                 where (js.EtiquetaMaterial1 == ms.Etiqueta || js.EtiquetaMaterial2 == ms.Etiqueta)
                                                 && ms.MaterialSpoolID == materialSpoolSam2.MaterialSpoolID
                                                 && (jw.ArmadoAprobado || jw.SoldaduraAprobada || jw.InspeccionVisualAprobada)
                                                 select jw).AsParallel().Distinct().Any();

                                if (tieneProcesos)
                                {
                                    throw new Exception("No se puede eliminar el corte, pues el material relacionado ya cuenta con algun proceso registrado");
                                }

                                //Comenzamos el proceso
                                corteDetalle.Activo = false;
                                corteDetalle.FechaModificacion = DateTime.Now;
                                corteDetalle.UsuarioModificacion = usuario.UsuarioID;
                                ctx.SaveChanges();

                                if (!ctx.Sam3_CorteDetalle.Where(x => x.CorteID == corteDetalle.CorteID && x.Activo).AsParallel().Any())
                                {
                                    // Ya no hay detalles activos para ese Corte, por lo que el corte de elimina
                                    corteSam3 = ctx.Sam3_Corte.Where(x => x.CorteID == corteDetalle.CorteID).AsParallel().SingleOrDefault();
                                    corteSam3.Activo = false;
                                    corteSam3.FechaModificacion = DateTime.Now;
                                    corteSam3.UsuarioModificacion = usuario.UsuarioID;
                                    ctx.SaveChanges();
                                }

                                despachoSam3 = ctx.Sam3_Despacho.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID
                                    && x.MaterialSpoolID == materialSpoolSam2.MaterialSpoolID && x.Cantidad == cantidadInventario && x.Activo).AsParallel().SingleOrDefault();
                                despachoSam3.Activo = false;
                                despachoSam3.FechaModificacion = DateTime.Now;
                                despachoSam3.UsuarioModificacion = usuario.UsuarioID;

                                numeroUnicoSam3 = ctx.Sam3_NumeroUnico.Where(x => x.NumeroUnicoID == despachoSam3.NumeroUnicoID).AsParallel().SingleOrDefault();
                                //devuelvo el congelado
                                numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado = numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado + cantidadInventario;
                                numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico = numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico + cantidadInventario;
                                numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioBuenEstado = numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico - numeroUnicoSam3.Sam3_NumeroUnicoInventario.CantidadDanada;
                                numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioBuenEstado - numeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado;

                                Sam3_NumeroUnicoSegmento segmentoSam3 = numeroUnicoSam3.Sam3_NumeroUnicoSegmento.Where(x => x.Segmento == "A").AsParallel().SingleOrDefault();
                                segmentoSam3.InventarioFisico = segmentoSam3.InventarioFisico + cantidadInventario;
                                segmentoSam3.InventarioCongelado = segmentoSam3.InventarioCongelado + cantidadInventario;
                                segmentoSam3.InventarioBuenEstado = segmentoSam3.InventarioFisico - segmentoSam3.CantidadDanada;
                                segmentoSam3.InventarioDisponibleCruce = segmentoSam3.InventarioBuenEstado - segmentoSam3.InventarioCongelado;

                                Sam3_NumeroUnicoMovimiento movimientoSam3 = ctx.Sam3_NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == corteDetalle.SalidaInventarioID.Value).AsParallel().SingleOrDefault();
                                movimientoSam3.Activo = false;
                                movimientoSam3.FechaModificacion = DateTime.Now;
                                movimientoSam3.UsuarioModificacion = usuario.UsuarioID;


                                ctx.SaveChanges();
                                #endregion

                                #region actualizar sam2
                                int nuSam2ID = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == despachoSam3.NumeroUnicoID)
                                    .Select(x => x.Sam2_NumeroUnicoID).AsParallel().Distinct().SingleOrDefault();

                                numeroUnicoSam2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == nuSam2ID).AsParallel().SingleOrDefault();
                                numeroUnicoSam2.NumeroUnicoInventario.InventarioFisico = numeroUnicoSam2.NumeroUnicoInventario.InventarioFisico + cantidadInventario;
                                numeroUnicoSam2.NumeroUnicoInventario.InventarioCongelado = numeroUnicoSam2.NumeroUnicoInventario.InventarioCongelado + cantidadInventario;
                                numeroUnicoSam2.NumeroUnicoInventario.InventarioBuenEstado = numeroUnicoSam2.NumeroUnicoInventario.InventarioFisico - numeroUnicoSam2.NumeroUnicoInventario.CantidadDanada;
                                numeroUnicoSam2.NumeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoSam2.NumeroUnicoInventario.InventarioBuenEstado - numeroUnicoSam2.NumeroUnicoInventario.InventarioCongelado;
                                numeroUnicoSam2.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                NumeroUnicoSegmento segmentoSam2 = numeroUnicoSam2.NumeroUnicoSegmento.Where(x => x.Segmento == "A").AsParallel().SingleOrDefault();
                                segmentoSam2.InventarioFisico = segmentoSam2.InventarioFisico + cantidadInventario;
                                segmentoSam2.InventarioCongelado = segmentoSam2.InventarioCongelado + cantidadInventario;
                                segmentoSam2.InventarioBuenEstado = segmentoSam2.InventarioFisico - segmentoSam2.CantidadDanada;
                                segmentoSam2.InventarioDisponibleCruce = segmentoSam2.InventarioBuenEstado - segmentoSam2.InventarioCongelado;
                                segmentoSam2.FechaModificacion = DateTime.Now;

                                odtm = ctx2.OrdenTrabajoMaterial.Where(x => x.DespachoID == despachoSam3.DespachoID
                                    && x.MaterialSpoolID == corteDetalle.MaterialSpoolID
                                    && x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID
                                    && x.CantidadDespachada == cantidadInventario).AsParallel().SingleOrDefault();

                                odtm.NumeroUnicoCongeladoID = odtm.NumeroUnicoDespachadoID;
                                odtm.NumeroUnicoDespachadoID = null;
                                odtm.CantidadCongelada = materialSpoolSam2.Cantidad;
                                odtm.TieneCorte = false;
                                odtm.TieneDespacho = false;
                                odtm.CantidadDespachada = null;
                                odtm.DespachoID = null;
                                odtm.CorteDetalleID = null;
                                odtm.TieneInventarioCongelado = true;
                                odtm.SegmentoCongelado = odtm.SegmentoDespachado;
                                odtm.SegmentoDespachado = string.Empty;

                                ctx2.SaveChanges();
                                #endregion

                                //Commits 
                                ctx2_tran.Commit();
                                ctx_tran.Commit();
                            }// fin tran sam2
                        }//fin sam2
                    }// fin tran sam3
                }// fin ctx sam 3


                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
                result.ReturnCode = 200;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al eliminar el corte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ListadoNumerosDeControl(string prefijo, int consecutivo, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombos> resultado = new List<ListaCombos>();

                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {

                        int sam2_NumeroUnicoID = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                  join nu in ctx.Sam3_NumeroUnico on eq.Sam3_NumeroUnicoID equals nu.NumeroUnicoID
                                                  where nu.Prefijo == prefijo && nu.Consecutivo == consecutivo
                                                  && eq.Activo && nu.Activo
                                                  select eq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                        resultado = (from odtm in ctx2.OrdenTrabajoMaterial
                                     join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                     where odtm.NumeroUnicoCongeladoID == sam2_NumeroUnicoID
                                     select new ListaCombos
                                     {
                                         id = odts.OrdenTrabajoSpoolID.ToString(),
                                         value = odts.NumeroControl
                                     }).AsParallel().Distinct().ToList();

                        resultado = resultado.GroupBy(x => x.id).Select(x => x.First()).ToList();

                    }
                }


                return resultado;
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

        public object ObtenerCantidadMaterial(string numeroControl, string etiqueta, Sam3_Usuario usuario)
        {
            try
            {
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    int cantidad = (from odts in ctx2.OrdenTrabajoSpool
                                    join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                    join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                    where ms.Etiqueta == etiqueta && odts.NumeroControl == numeroControl
                                    select odtm.CantidadCongelada.Value).AsParallel().SingleOrDefault();

                    return cantidad;
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