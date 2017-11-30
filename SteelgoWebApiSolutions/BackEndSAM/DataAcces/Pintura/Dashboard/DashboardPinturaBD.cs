using BackEndSAM.Models.Pintura.Dashboard;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.Dashboard
{
    public class DashboardPinturaBD
    {
        private static readonly object _mutex = new Object();
        private static DashboardPinturaBD _instance;

        public static DashboardPinturaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DashboardPinturaBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtieneHeaderDashBoard( string lenguaje, int proyectoID, int ClienteID, string FechaInicial, string FechaFinal)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<CabeceraDashboar> lista = new List<CabeceraDashboar>();
                    List<Sam3_Pintura_Get_ContadorDashboardPintura_Result> result =  ctx.Sam3_Pintura_Get_ContadorDashboardPintura(proyectoID,ClienteID,FechaInicial,FechaFinal).ToList();

                    foreach (Sam3_Pintura_Get_ContadorDashboardPintura_Result item in result)
                    {
                        lista.Add(new CabeceraDashboar
                        {
                            Spools1 = item.Spools1.GetValueOrDefault(),
                            Spools2 = item.Spools2.GetValueOrDefault(),
                            Spools3 = item.Spools3.GetValueOrDefault(),
                            Spools4 = item.Spools4.GetValueOrDefault(),
                            Spools5 = item.Spools5.GetValueOrDefault(),
                            Spools6 = item.Spools6.GetValueOrDefault(),
                            Spools7 = item.Spools7.GetValueOrDefault(),
                            Spools8 = item.Spools8.GetValueOrDefault(),
                            Spools9 = item.Spools9.GetValueOrDefault(),
                            Spools10 = item.Spools10.GetValueOrDefault(),
                            Spools11 = item.Spools11,
                            Spools12 = item.Spools12.GetValueOrDefault(),
                            M21 = item.M21.GetValueOrDefault(),
                            M22 = item.M22.GetValueOrDefault(),
                            M23 = item.M23.GetValueOrDefault(),
                            M24 = item.M24.GetValueOrDefault(),
                            M25 = item.M25.GetValueOrDefault(),
                            M26 = item.M26.GetValueOrDefault(),
                            M27 = item.M27.GetValueOrDefault(),
                            M28 = item.M28.GetValueOrDefault(),
                            M29 = item.M29.GetValueOrDefault(),
                            M210 = item.M210.GetValueOrDefault(),
                            M211 = item.M211,
                            M212 = item.M212.GetValueOrDefault(),
                            Toneladas1 = item.Toneladas1.GetValueOrDefault(),
                            Toneladas2 = item.Toneladas2.GetValueOrDefault(),
                            Toneladas3 = item.Toneladas3.GetValueOrDefault(),
                            Toneladas4 = item.Toneladas4.GetValueOrDefault(),
                            Toneladas5 = item.Toneladas5.GetValueOrDefault(),
                            Toneladas6 = item.Toneladas6.GetValueOrDefault(),
                            Toneladas7 = item.Toneladas7.GetValueOrDefault(),
                            Toneladas8 = item.Toneladas8.GetValueOrDefault(),
                            Toneladas9 = item.Toneladas9.GetValueOrDefault(),
                            Toneladas10 = item.Toneladas10.GetValueOrDefault(),
                            Toneladas11 = item.Toneladas11,
                            Toneladas12 = item.Toneladas12.GetValueOrDefault()

                        });
                    }
                    

                    return lista;
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


        public object ObtieneContenidoDashBoard(string lenguaje, int proyectoID,int estatusID, int clienteID, string fechaInicial, string fechaFinal)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetalleDashboardPintura> lista = new List<DetalleDashboardPintura>();
                    
                    List<Sam3_Pintura_Get_DetalleDashboardPintura_Result> result =  ctx.Sam3_Pintura_Get_DetalleDashboardPintura(proyectoID,clienteID,fechaInicial,fechaFinal,estatusID).OrderBy(x => x.NumeroControl).ToList();

                    foreach (Sam3_Pintura_Get_DetalleDashboardPintura_Result item  in result)
                    {
                        lista.Add(new DetalleDashboardPintura {
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            Proyecto = item.Proyecto,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Cuadrante = item.Cuadrante,
                            M2 = item.M2.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            SistemaPintura = item.SistemaPintura,
                            Carro = item.Carro,
                            Color = item.Color,
                            CantidadSpools = item.CantidadSpools
                        });
                    }

                    return lista;
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