using BackEndSAM.Models.Sam3General.Periodo;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BackEndSAM.DataAcces.Sam3General.Periodo
{
    public class PeriodoBD
    {
        private static readonly object _mutex = new object();
        private static PeriodoBD _instance;

        public static PeriodoBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new PeriodoBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtienePeriodo(string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_SteelGo_Get_Periodo_Result> result = ctx.Sam3_SteelGo_Get_Periodo(Lenguaje).ToList();
                    List<DetallePeriodo> listaDetalle = new List<DetallePeriodo>();

                    listaDetalle.Add(new DetallePeriodo());
                    foreach (Sam3_SteelGo_Get_Periodo_Result item in result)
                    {
                        listaDetalle.Add(new DetallePeriodo {
                               PeriodoID = item.PeriodoID,
                               Periodo = item.Periodo,
                               Minuendo = item.Minuendo,
                               Sustraendo = item.Sustraendo
                        });
                    }
                    return listaDetalle;
                }

            }catch(Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtieneRangoFechas(string Lenguaje, string Minuendo, int Sustraendo, string FechaFinal)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var fechaInicio = new ObjectParameter("FechaInicio", typeof(string));
                    var fechaFin = new ObjectParameter("FechaFin", typeof(string));

                    var resultSp = ctx.Sam3_SteelGo_Get_RangoFechas(Lenguaje, Minuendo, Sustraendo, FechaFinal, fechaInicio, fechaFin);
                    RangoFechas obj = new RangoFechas(fechaInicio.Value.ToString(), fechaFin.Value.ToString());
                    
                    return obj;
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