using BackEndSAM.Models.Pintura.ListadoSistemaPintura;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.ListadoSistemaPintura
{
    public class ListadoSistemaPinturaBD
    {
        private static readonly object _mutex = new object();
        private static ListadoSistemaPinturaBD _instance;

        public static ListadoSistemaPinturaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ListadoSistemaPinturaBD();
                    }
                }
                return _instance;
            }
        }
        public object ObtieneListadoPruebas(int SistemaPinturaProyectoProcesoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetallePruebas> detalle = new List<DetallePruebas>();
                    List<Sam3_SP_ObtieneListadoPruebasPorProceso_Result> result = ctx.Sam3_SP_ObtieneListadoPruebasPorProceso(SistemaPinturaProyectoProcesoID, lenguaje).ToList();

                    foreach (Sam3_SP_ObtieneListadoPruebasPorProceso_Result item in result)
                    {
                        detalle.Add(new DetallePruebas {
                            Accion = 1,
                            Prueba = item.Prueba,
                            UnidadMedida = item.UnidadMedida,
                            UnidadMin = item.UnidadMinima,
                            UnidadMax = item.UnidadMaxima
                        });
                    }

                    return detalle;
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

        public object ObtieneListadoColorPintura(int SistemaPinturaID, string lenguaje,int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    string cadena = "";
                    List<Sam3_SPA_Get_ColorPintura_Result> result = ctx.Sam3_SPA_Get_ColorPintura(SistemaPinturaID, lenguaje, ProyectoID).ToList();

                    int posicion = 0;
                    foreach (Sam3_SPA_Get_ColorPintura_Result item in result)
                    {
                        cadena += item.ColorPintura;
                        if (posicion < result.Count - 1)
                            cadena +=   " ,";
                        posicion++;
                    }
                    if (cadena.Length == 0)
                        cadena = "N/A";

                    return cadena;
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

        public object ObtieneDetalleSistemaPintura(int UsuarioID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetalleSistemaPintura> list = new List<DetalleSistemaPintura>();
                    List<Sam3_SP_ObtieneListadoSistemaPintura_Result> result = ctx.Sam3_SP_ObtieneListadoSistemaPintura(UsuarioID,lenguaje).ToList();

                    foreach(Sam3_SP_ObtieneListadoSistemaPintura_Result item in result)
                    {
                        list.Add(new DetalleSistemaPintura {
                            Accion = 1,
                            SistemaPinturaID = item.SistemaPinturaID,
                            SistemaPintura = item.SistemaPintura.Split('~')[0],
                           
                            ProyectoProcesoShotblastID = item.ProyectoProcesoShotblastID.GetValueOrDefault(),
                            ProyectoProcesoPrimarioID = item.ProyectoProcesoPrimarioID.GetValueOrDefault(),
                            ProyectoProcesoIntermedioID = item.ProyectoProcesoIntermedioID.GetValueOrDefault(),
                            ProyectoProcesoAcabadoID = item.ProyectoProcesoAcabadoID.GetValueOrDefault(),
                            Proyecto = item.Proyecto,
                            ProyectoID=item.ProyectoID,
                            Color = (string)ListadoSistemaPinturaBD.Instance.ObtieneListadoColorPintura(item.SistemaPinturaID, lenguaje, item.ProyectoID),
                            PruebaPorLoteShotblast = item.PruebaPorLoteShotblast.GetValueOrDefault(),
                            MetrosPorLoteShotblast = item.MetrosPorLoteShotblast.GetValueOrDefault(),
                            PruebaShotblast = lenguaje.Equals("es-MX") ? "Detalle Pruebas" : "Detail Testing",
                            PruebaPorLotePrimario = item.PruebaPorLotePrimario.GetValueOrDefault(),
                            MetrosPorLotePrimario = item.MetrosPorLotePrimario.GetValueOrDefault(),
                            PruebaPrimario = lenguaje.Equals("es-MX") ? "Detalle Pruebas" : "Detail Testing",
                            PruebaPorLoteIntermedio = item.PruebaPorLoteIntermedio.GetValueOrDefault(),
                            MetrosPorLoteIntermedio = item.MetrosPorLoteIntermedio.GetValueOrDefault(),
                            PruebaIntermedio = lenguaje.Equals("es-MX") ? "Detalle Pruebas" : "Detail Testing",
                            PruebaPorLoteAcabado = item.PruebaPorLoteAcabado.GetValueOrDefault(),
                            MetrosPorLoteAcabado = item.MetrosPorLoteAcabado.GetValueOrDefault(),
                            PruebaAcabado = lenguaje.Equals("es-MX") ? "Detalle Pruebas" : "Detail Testing",

                            NumeroComponentesShootblast = item.NumeroComponentesShotblast,
                            TemplateComponentesShotBlast = item.TemplateComponentesShotblast,
                            TemplateReductoresShotblast=item.TemplateReductoresShotblast,

                            NumeroComponentesPrimario = item.NumeroComponentesPrimario,
                            TemplateComponentesPrimario = item.TemplateComponentesPrimario,
                            TemplateReductoresPrimario = item.TemplateReductoresPrimario,

                            NumeroComponentesIntermedio = item.NumeroComponentesIntermedio,
                            TemplateComponentesIntermedio = item.TemplateComponentesIntermedio,
                            TemplateReductoresIntermedio = item.TemplateReductoresIntermedio,

                            NumeroComponentesAcabado = item.NumeroComponentesAcabado,
                            TemplateComponentesAcabado =item.TemplateComponentesAcabado,
                            TemplateReductoresAcabado = item.TemplateReductoresAcabado,
                        });
                    }
                    return list;
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

        public object EliminaSistemaPintura(int SistemaPinturaID, int UsuarioID, int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    ObjectResult<int?> resultSp = ctx.Sam3_SP_EliminaSistemaPintura(SistemaPinturaID, UsuarioID, ProyectoID);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];
                    

                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("El sistema pintura se encuentra asignado");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
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
    }
}