using BackEndSAM.DataAcces.ServiciosTecnicos;
using BackEndSAM.Models.ServiciosTecnicos.AsignarRequisicion;
using BackEndSAM.Models.ServiciosTecnicos.DashboardPND;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Dashboard
{
    public class DashboardBD
    {
        private static readonly object _mutex = new Object();
        private static DashboardBD _instance;

        public static DashboardBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DashboardBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtieneHeaderDashBoard(int modulo, string lenguaje,int proyectoID, int tipoPruebaID,int proveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Get_EstatusPorModulo_Result> result = ctx.Sam3_Get_EstatusPorModulo(modulo, lenguaje,proyectoID,tipoPruebaID,proveedorID,"","").OrderBy(x => x.Orden).ToList();

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

        public object ObtenerInformacionGrid(string lenguaje,int ProyectoID,int TipoPruebaID, int ProveedorID, string FechaInicial, string FechaFinal, int EstatusID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetalleDashboard> listadoDetalleDashboard = new List<DetalleDashboard>();
                    List<Sam3_ST_DASH_GET_RequisicionesDashboard_Result> result =  ctx.Sam3_ST_DASH_GET_RequisicionesDashboard(lenguaje,ProyectoID,TipoPruebaID,ProveedorID,FechaInicial,FechaInicial,EstatusID).ToList();

                    foreach (Sam3_ST_DASH_GET_RequisicionesDashboard_Result item in result)
                    {
                        listadoDetalleDashboard.Add( new DetalleDashboard{
                            RequisicionID = item.RequisicionID,
                            Requisicion = item.NumeroRequisicion,
                            TipoPrueba = item.TipoPrueba,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Equipo = item.Equipo,
                            EquipoID = item.EquipoID.GetValueOrDefault(),
                            EstatusRequisicion= item.EstatusRequisicion.GetValueOrDefault(),
                            Fecha = item.Fecha,
                            NumeroElementos = item.NumeroElementos.GetValueOrDefault(),
                            Proveedor = item.Proveedor,
                            ProveedorID = item.ProveedorID.GetValueOrDefault(),
                            Turno = item.TurnoLaboral,
                            TurnoLaboralID = item.TurnoLaboralID.GetValueOrDefault(),
                            Url = item.Url,
                            listaElementosRequisicion =  (List<ElementosRequisicion>) AsignarRequisicionBD.Instance.ObtenerElementosRequisicion(lenguaje, ProyectoID, item.TipoPruebaID.GetValueOrDefault(), item.RequisicionID),
                        });
                    }
                    return listadoDetalleDashboard;
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

    }
}