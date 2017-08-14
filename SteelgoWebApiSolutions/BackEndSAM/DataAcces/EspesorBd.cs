using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class EspesorBd
    {
         private static readonly object _mutex = new object();
        private static EspesorBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private EspesorBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static EspesorBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EspesorBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Function para obtener los Espesores existentes en el catalogo
        /// para la pantalla Catalogo Item Code Steelgo segun el diametro seleccionado
        /// </summary>
        /// <param name="diametro1">diametro 1 seleccionado</param>
        /// <param name="diametro2">diametro 2 seleccionado</param>
        /// <returns></returns>
        public object ObtenerEspesores(string diametro1, string diametro2)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int d1 = Convert.ToInt32(diametro1);
                    int? d2 = Convert.ToInt32(diametro2);

                    List<ListaDiametros> espesores = (from es in ctx.Sam3_Espesor
                                                      where es.Activo == 1 && es.DiametroID == d1 //|| es.DiametroID == d2
                                                      select new ListaDiametros
                                                      {
                                                          id = es.EspesorID,
                                                          value = Math.Round(es.Valor, 4)
                                                      }).AsParallel().GroupBy(x => x.value).Select(x => x.First()).ToList();

                    List<ListaCombos> lista = (from es in espesores.OrderBy(x => x.value)
                                                 select new ListaCombos
                                                 {
                                                     id = es.id.ToString(),
                                                     value = es.value.ToString()
                                                 }).AsParallel().ToList();

                    return lista;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Funcion para obtener los espesores en pulgadas para el diametro seleccionado
        /// </summary>
        /// <param name="diametro1">Diametro 1 seleccionado</param>
        /// <param name="diametro2">Diametro 2 seleccionado</param>
        /// <returns></returns>
        public object ObtenerEspesoresIN(int diametro1, int? diametro2)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    decimal factor = Convert.ToDecimal(0.0393701);

                    List<ListaDiametros> espesores = (from es in ctx.Sam3_Espesor
                                                      where es.Activo == 1 && es.DiametroID == diametro1 //|| es.DiametroID == diametro2
                                                      select new ListaDiametros
                                                      {
                                                          id = es.EspesorID,
                                                          value = Math.Round(es.Valor, 4)
                                                      }).AsParallel().GroupBy(x => x.value).Select(x => x.First()).ToList();

                    List<ListaCombos> lista = (from es in espesores.OrderBy(x => x.value)
                                                 select new ListaCombos
                                                 {
                                                     id = es.id.ToString(),
                                                     value = es.value.ToString()
                                                 }).AsParallel().ToList();

                    foreach (var item in lista)
                    {
                        item.value = Math.Round((Convert.ToDecimal(item.value) * factor), 4).ToString();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Funcion para obtener todos los espesores
        /// cuando se selecciona un diametro no existente en el
        /// catalogo de item code steelgo
        /// </summary>
        /// <returns></returns>
        public object obtenerTodosEspesoresMM()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaDiametros> espesores = (from es in ctx.Sam3_Espesor
                                                      where es.Activo == 1
                                                      select new ListaDiametros
                                                      {
                                                          id = es.EspesorID,
                                                          value = Math.Round(es.Valor, 4)
                                                      }).AsParallel().GroupBy(x=> x.value).Select(x=> x.First()).ToList();

                    List<ListaCombos> listado = (from es in espesores.OrderBy(x=> x.value)
                                                 select new ListaCombos
                                                 {
                                                     id = es.id.ToString(),
                                                     value = es.value.ToString()
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
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Funcion para obtener todos los espesores en pulgadas 
        /// cuando se selecciona un diametro no existente
        /// en el catalogo de item code steelgo
        /// </summary>
        /// <returns></returns>
        public object obtenerTodosEspesoresIN()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    decimal factor = Convert.ToDecimal(0.0393701);

                    List<ListaDiametros> espesores = (from es in ctx.Sam3_Espesor
                                                      where es.Activo == 1
                                                      select new ListaDiametros
                                                      {
                                                          id = es.EspesorID,
                                                          value =  Math.Round(es.Valor, 4)
                                                      }).AsParallel().GroupBy(x=> x.value).Select(x=> x.First()).ToList();

                    List<ListaCombos> listado = (from es in espesores.OrderBy(x=> x.value)
                                                 select new ListaCombos
                                                 {
                                                     id = es.id.ToString(),
                                                     value = es.value.ToString()
                                                 }).AsParallel().ToList();

                    foreach (var item in listado)
                    {
                        item.value = Math.Round((Convert.ToDecimal(item.value) * factor), 4).ToString();
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
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

    }
}