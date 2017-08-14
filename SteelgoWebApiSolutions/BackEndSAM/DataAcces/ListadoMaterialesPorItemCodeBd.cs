using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;

namespace BackEndSAM.DataAcces
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoMaterialesPorItemCodeBd
    {
        private static readonly object _mutex = new object();
        private static ListadoMaterialesPorItemCodeBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ListadoMaterialesPorItemCodeBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ListadoMaterialesPorItemCodeBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ListadoMaterialesPorItemCodeBd();
                    }
                }
                return _instance;
            }
        }

        public object obtenerListadoPorItemCode(string proyectoID)
        {
            try
            {
                List<ListadoMaterialesPorItemCode> listado = new List<ListadoMaterialesPorItemCode>();
                using (SamContext ctx = new SamContext())
                {
                    listado = (from ic in ctx.Sam3_ItemCode
                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                               join ricd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals ricd.Rel_ItemCodeSteelgo_Diametro_ID
                               join ics in ctx.Sam3_ItemCodeSteelgo on ricd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                               join tm in ctx.Sam3_TipoMaterial on ic.TipoMaterialID equals tm.TipoMaterialID
                               join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                               join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                               where ic.Activo && rics.Activo && ics.Activo && tm.Activo && rid.Activo && ricd.Activo && d1.Activo && d2.Activo
                               && ic.ProyectoID.ToString() == proyectoID
                               select new ListadoMaterialesPorItemCode
                               {
                                   ItemCodeID = ic.ItemCodeID.ToString(),
                                   ItemCode = ic.Codigo,
                                   ItemCodeSteelgoID = ics.ItemCodeSteelgoID.ToString(),
                                   ItemCodeSteelgo = ics.Codigo,
                                   Descripcion = ics.DescripcionEspanol,
                                   D1 = d1.Valor.ToString(),
                                   D2 = d2.Valor.ToString(),
                                   TipoMaterial = tm.Nombre,

                                   TotalRecibido = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                    join nu in ctx.Sam3_NumeroUnico on nui.NumeroUnicoID equals nu.NumeroUnicoID
                                                    where nui.Activo && nu.Activo
                                                    && nu.ItemCodeID == ic.ItemCodeID
                                                    select (int?)nui.CantidadRecibida
                                                        ).Sum().ToString(),

                                   OtrasEntradas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                    join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                    join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                    where num.Activo && tmov.Activo && nu.Activo
                                                    && tmov.TipoMovimientoID == 9
                                                    && nu.ItemCodeID == ic.ItemCodeID
                                                    select (int?)num.Cantidad).Sum().ToString(),

                                   TotalCondicionada = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                        join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                        where num.Activo && nu.Activo
                                                        && nu.ItemCodeID == ic.ItemCodeID
                                                        && num.Estatus == "C"
                                                        select (int?)num.Cantidad).Sum().ToString(),

                                   TotalRechazado = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                     join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                     where num.Activo && nu.Activo
                                                     && nu.ItemCodeID == ic.ItemCodeID
                                                     && num.Estatus == "R"
                                                     select (int?)num.Cantidad).Sum().ToString(),

                                   TotalDanado = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                  join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                  where num.Activo && nu.Activo
                                                  && nu.ItemCodeID == ic.ItemCodeID
                                                  && num.Estatus == "D"
                                                  select (int?)num.Cantidad).Sum().ToString(),

                                   TotalRecibidoNeto = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                        join nu in ctx.Sam3_NumeroUnico on nui.NumeroUnicoID equals nu.NumeroUnicoID
                                                        where nui.Activo && nu.Activo
                                                        && nu.ItemCodeID == ic.ItemCodeID
                                                        select nui.CantidadRecibida - nui.CantidadDanada).Sum().ToString(),

                                   EntradasDesdeICEquivalente = "",

                                   SalidasTemporales = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                        join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                        join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                        where num.Activo && tmov.Activo && nu.Activo
                                                        && nu.ItemCodeID == ic.ItemCodeID
                                                        && tmov.TipoMovimientoID == 12
                                                        select (int?)num.Cantidad).Sum().ToString(),

                                   TotalOtrasSalidas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                        join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                        join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                        where num.Activo && tmov.Activo && nu.Activo
                                                        && tmov.TipoMovimientoID == 8
                                                        && nu.ItemCodeID == ic.ItemCodeID
                                                        select (int?)num.Cantidad).Sum().ToString(),

                                   TotalMermas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                  join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                  join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                  where num.Activo && tmov.Activo && nu.Activo
                                                  && nu.ItemCodeID == ic.ItemCodeID
                                                  && tmov.TipoMovimientoID == 4
                                                  select (int?)num.Cantidad).Sum().ToString(),

                                   TotalDespachadoPorCortar = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                               join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                               join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                               where num.Activo && tmov.Activo && nu.Activo
                                                               && tmov.TipoMovimientoID == 12
                                                               && nu.ItemCodeID == ic.ItemCodeID
                                                               select (int?)num.Cantidad).Sum().ToString(),

                                   TotalCortado = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                   join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                   join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                   where num.Activo && tmov.Activo && nu.Activo
                                                   && tmov.TipoMovimientoID == 15
                                                   && nu.ItemCodeID == ic.ItemCodeID
                                                   select (int?)num.Cantidad).Sum().ToString(),

                                   DespachadoAProduccion = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                            join tmov in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tmov.TipoMovimientoID
                                                            join nu in ctx.Sam3_NumeroUnico on num.NumeroUnicoID equals nu.NumeroUnicoID
                                                            where num.Activo && tmov.Activo && nu.Activo
                                                            && tmov.TipoMovimientoID == 2
                                                            && nu.ItemCodeID == ic.ItemCodeID
                                                            select (int?)num.Cantidad).Sum().ToString(),

                                   TotalFisicoEnAlmacen = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                           join nu in ctx.Sam3_NumeroUnico on nui.NumeroUnicoID equals nu.NumeroUnicoID
                                                           where nui.Activo && nu.Activo
                                                           && nu.ItemCodeID == ic.ItemCodeID
                                                           select (int?)nui.InventarioFisico).Sum().ToString(),

                                   InventarioDisponibleCruce = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                                join nu in ctx.Sam3_NumeroUnico on nui.NumeroUnicoID equals nu.NumeroUnicoID
                                                                where nui.Activo && nu.Activo
                                                                && nu.ItemCodeID == ic.ItemCodeID
                                                                select (int?)nui.InventarioDisponibleCruce).Sum().ToString(),
                               }).AsParallel().ToList();

                    listado = listado.GroupBy(x => x.ItemCodeID).Select(x => x.First()).ToList();
                }
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    listado.ForEach(x =>
                    {
                        x.TotalIngenieria = (from otm in ctx2.OrdenTrabajoMaterial
                                             join nu in ctx2.NumeroUnico on otm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                             where nu.ItemCodeID.ToString() == x.ItemCodeID
                                             select (int?)otm.CantidadDespachada).Sum().ToString();

                        x.TotalOrdenTrabajo = (from ms in ctx2.MaterialSpool
                                               join otm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals otm.MaterialSpoolID
                                               where ms.ItemCodeID.ToString() == x.ItemCodeID
                                               select (int?)otm.OrdenTrabajoMaterialID).Count().ToString();

                        x.DespachadoPorCortarParaICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                                  join nu in ctx2.NumeroUnico on ice.ItemCodeEquivalenteID equals nu.ItemCodeID
                                                                  join num in ctx2.NumeroUnicoMovimiento on nu.NumeroUnicoID equals num.NumeroUnicoID
                                                                  join tm in ctx2.TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                                  where ice.ItemCodeID.ToString() == x.ItemCodeID && tm.TipoMovimientoID == 15
                                                                  select (int?)num.Cantidad).Sum().ToString();

                        x.CortadoParaICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                      join nu in ctx2.NumeroUnico on ice.ItemCodeEquivalenteID equals nu.ItemCodeID
                                                      join num in ctx2.NumeroUnicoMovimiento on nu.NumeroUnicoID equals num.NumeroUnicoID
                                                      join tm in ctx2.TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                      where ice.ItemCodeID.ToString() == x.ItemCodeID && tm.TipoMovimientoID == 18
                                                      select (int?)num.Cantidad).Sum().ToString();

                        x.TotalCongelado = (from otm in ctx2.OrdenTrabajoMaterial
                                            join nu in ctx2.NumeroUnico on otm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                            where nu.ItemCodeID.ToString() == x.ItemCodeID
                                            select (int?)otm.CantidadCongelada).Sum().ToString();

                        x.TotalPorDespachar = (from nu in ctx2.NumeroUnico
                                               join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                               join otm in ctx2.OrdenTrabajoMaterial on nu.NumeroUnicoID equals otm.NumeroUnicoDespachadoID
                                               where nu.ItemCodeID.ToString() == x.ItemCodeID
                                               select (int?)nui.CantidadRecibida - otm.CantidadDespachada).Sum().ToString();

                        x.DespachadoDesdeICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                          join nu in ctx2.NumeroUnico on ice.ItemCodeEquivalenteID equals nu.ItemCodeID
                                                          join num in ctx2.NumeroUnicoMovimiento on nu.NumeroUnicoID equals num.NumeroUnicoID
                                                          join tm in ctx2.TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                          where ice.ItemCodeID.ToString() == x.ItemCodeID && tm.TipoMovimientoID == 2
                                                          select (int?)num.Cantidad).Sum().ToString();

                        x.EntradasDesdeICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                        join nu in ctx2.NumeroUnico on ice.ItemEquivalenteID equals nu.ItemCodeID
                                                        join num in ctx2.NumeroUnicoMovimiento on nu.NumeroUnicoID equals num.NumeroUnicoID
                                                        join tm in ctx2.TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                        where ice.ItemCodeID.ToString() == x.ItemCodeID && tm.EsEntrada
                                                        select (int?)num.Cantidad).Sum().ToString();

                        x.SalidaParaICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                     join nu in ctx2.NumeroUnico on ice.ItemEquivalenteID equals nu.ItemCodeID
                                                     join num in ctx2.NumeroUnicoMovimiento on nu.NumeroUnicoID equals num.NumeroUnicoID
                                                     join tm in ctx2.TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                     where ice.ItemCodeID.ToString() == x.ItemCodeID && !tm.EsEntrada
                                                     select (int?)num.Cantidad).Sum().ToString();

                        x.DisponibleICEquivalente = (from ice in ctx2.ItemCodeEquivalente
                                                     join nu in ctx2.NumeroUnico on ice.ItemEquivalenteID equals nu.ItemCodeID
                                                     join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                     where ice.ItemCodeID.ToString() == x.ItemCodeID
                                                     select (int?)nui.InventarioDisponibleCruce).Sum().ToString();

                        x.TotalDisponibleParaCruce = ((String.IsNullOrEmpty(x.InventarioDisponibleCruce) ? 0 : Convert.ToInt32(x.InventarioDisponibleCruce)) + (String.IsNullOrEmpty(x.DisponibleICEquivalente) ? 0 : Convert.ToInt32(x.DisponibleICEquivalente))).ToString();
                    });
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
    }
}