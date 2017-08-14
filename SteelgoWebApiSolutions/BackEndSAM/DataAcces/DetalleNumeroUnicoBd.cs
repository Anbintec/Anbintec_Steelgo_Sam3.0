using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class DetalleNumeroUnicoBd
    {

        private static readonly object _mutex = new object();
        private static DetalleNumeroUnicoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DetalleNumeroUnicoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static DetalleNumeroUnicoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DetalleNumeroUnicoBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Se obtienen los detalles del numero unico
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public object obtenerDatosInventario(string numeroUnicoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DetalleNumeroUnico detalle = new DetalleNumeroUnico();

                    detalle = (from nu in ctx.Sam3_NumeroUnico
                               join ic in ctx.Sam3_ItemCode on nu.ItemCodeID equals ic.ItemCodeID
                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on ic.ItemCodeID equals rics.ItemCodeID
                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                               join nui in ctx.Sam3_NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                               join c in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals c.CatalogoCedulasID
                               join ced in ctx.Sam3_Cedula on c.CedulaA equals ced.CedulaID
                               where nu.Activo && ic.Activo && rics.Activo && ics.Activo && nui.Activo && c.Activo
                               && nu.NumeroUnicoID.ToString() == numeroUnicoID
                               select new DetalleNumeroUnico
                               {
                                   NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                   NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                   ItemCode = ic.Codigo,
                                   ItemCodeID = ic.ItemCodeID.ToString(),
                                   ItemCodeSteelgo = ics.Codigo,
                                   //D1 = ics.Diametro1.ToString(),
                                   //D2 = ics.Diametro2.ToString(),
                                   Cedula = ced.Codigo,
                                   Profile1 = "X",
                                   Profile2 = "Y",
                                   TotalRecibido = nui.CantidadRecibida.ToString(),
                                   TotalDanado = nui.CantidadDanada.ToString(),

                                   TotalEntradas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                    join tm in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                    where num.Activo && tm.Activo && tm.EsEntrada &&
                                                    num.NumeroUnicoID.ToString() == numeroUnicoID
                                                    select num.Cantidad).AsEnumerable().Sum(b => b).ToString(),

                                   TotalSalidas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                   join tm in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                   where num.Activo && tm.Activo && !tm.EsEntrada &&
                                                   num.NumeroUnicoID.ToString() == numeroUnicoID
                                                   select num.Cantidad).AsEnumerable().Sum(b => b).ToString(),

                                   SaldoActual = nui.InventarioFisico.ToString()
                               }).AsParallel().GroupBy(x => x.NumeroUnicoID).Select(x => x.First()).SingleOrDefault();

                    int itemcodeID = Convert.ToInt32(detalle.ItemCodeID);
                    int numeroDigitos = (from it in ctx.Sam3_ItemCode
                                         join pc in ctx.Sam3_ProyectoConfiguracion on it.ProyectoID equals pc.ProyectoID
                                         where it.ItemCodeID == itemcodeID
                                         select pc.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                    string formato = "D" + numeroDigitos.ToString();

                    string[] codigo = detalle.NumeroUnico.Split('-').ToArray();
                    int consecutivo = Convert.ToInt32(codigo[1]);
                    detalle.NumeroUnico = codigo[0] + "-" + consecutivo.ToString(formato);

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
        /// Obtener datos del Grid Segmento
        /// Detalle de Numero Unico
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public object gridSegmentos(string numeroUnicoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<GridSegmento> segmento = (from nus in ctx.Sam3_NumeroUnicoSegmento
                                                   join nui in ctx.Sam3_NumeroUnicoInventario on nus.NumeroUnicoID equals nui.NumeroUnicoID
                                                   where nus.Activo && nui.Activo && nus.NumeroUnicoID.ToString() == numeroUnicoID
                                                   select new GridSegmento
                                                   {
                                                       Segmento = nus.Segmento,
                                                       Rack = nus.Rack,
                                                       TotalRecibido = nui.CantidadRecibida.ToString(),
                                                       TotalDanado = nui.CantidadDanada.ToString(),
                                                       SalidasTemporales = nus.InventarioTransferenciaCorte.ToString(),
                                                       TotalEntradas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                                        join tm in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                                        where tm.Activo && num.Activo
                                                                        && num.Segmento == nus.Segmento
                                                                        && tm.EsEntrada
                                                                        && num.NumeroUnicoID.ToString() == numeroUnicoID
                                                                        select num.Cantidad).AsEnumerable().Sum(c => c).ToString(),
                                                       TotalSalidas = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                                       join tm in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                                       where tm.Activo && num.Activo
                                                                       && num.Segmento == nus.Segmento
                                                                       && !tm.EsEntrada
                                                                       && num.NumeroUnicoID.ToString() == numeroUnicoID
                                                                       select num.Cantidad).AsEnumerable().Sum(c => c).ToString(),
                                                       SaldoActual = nus.InventarioFisico.ToString()
                                                   }).AsParallel().OrderBy(x => x.Segmento).ToList();
                    return segmento;
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
        /// Funcion para obtener la informacion del Grid de Movimientos
        /// en Detalle de Numero Unico
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public object gridMovimientos(string numeroUnicoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int saldo = 0;

                    List<GridMovimientos> movimientos = (from num in ctx.Sam3_NumeroUnicoMovimiento
                                                         join tm in ctx.Sam3_TipoMovimiento on num.TipoMovimientoID equals tm.TipoMovimientoID
                                                         join nui in ctx.Sam3_NumeroUnicoInventario on num.NumeroUnicoID equals nui.NumeroUnicoID
                                                         where num.Activo && tm.Activo && num.NumeroUnicoID.ToString() == numeroUnicoID
                                                         select new GridMovimientos
                                                         {
                                                             Fecha = num.FechaMovimiento.ToString(),
                                                             Movimiento = tm.Nombre.ToString(),
                                                             Segmento = num.Segmento,
                                                             Entrada = tm.EsEntrada ? num.Cantidad.ToString() : "0",
                                                             Salida = !tm.EsEntrada ? num.Cantidad.ToString() : "0",
                                                             Referencia = num.Referencia,
                                                             SaldoActual = "0"
                                                         }).AsParallel().OrderBy(x => x.Segmento).ToList();

                    movimientos.ForEach(c =>
                    {
                        saldo = saldo + Int32.Parse(c.Entrada) - Int32.Parse(c.Salida);
                        c.SaldoActual = saldo.ToString();
                    });
                    return movimientos;
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