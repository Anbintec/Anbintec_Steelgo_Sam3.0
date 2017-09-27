using DatabaseManager.Sam3;
using BackEndSAM.Models.Sam3General;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General
{
    public class ObreroBD
    {
        private static readonly object _mutex = new object();
        private static ObreroBD _instance;

        public ObreroBD()
        {

        }

        public static ObreroBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ObreroBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerObrero()
        {
            try
            {
                List<Obrero> obrero = new List<Obrero>();

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_Obrero_Result> result = ctx.Sam3_Cat_Obrero(1, 0, 0, 0, "", "").ToList();

                    foreach (Sam3_Cat_Obrero_Result elem in result)
                    {
                        obrero.Add(new Obrero
                        {
                            ObreroID = elem.ObreroID,
                            TipoObreroID = elem.TipoObreroID,
                            Codigo = elem.Codigo,
                            NumeroEmpleado = elem.NumeroEmpleado,
                            TipoObrero = elem.TipoObrero
                        });
                    }
                    return obrero;
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

        public object ObtenerObrero(string TipoObreroID)
        {
            try
            {
                List<Obrero> obrero = new List<Obrero>();

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_Obrero_Result> result = ctx.Sam3_Cat_Obrero(5, 0, 0, int.Parse(TipoObreroID), "", "").ToList();

                    foreach (Sam3_Cat_Obrero_Result elem in result)
                    {
                        obrero.Add(new Obrero
                        {
                            ObreroID = elem.ObreroID,
                            Codigo = elem.Codigo,
                        });
                    }
                    if (result.Count == 0)
                    {
                        obrero.Add(new Obrero
                        {
                            ObreroID = 0,
                            Codigo = "",
                        });
                    }
                    return obrero;
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
        /// <summary>
        /// Obtiene Obrero por proyecto y tipo de obrero
        /// </summary>
        /// <param name="idProyecto">identficador del proyecto</param>
        /// <param name="tipo">1: Retorna todos los obreros activos en todos los Patios, 
        ///                    2: Retorna todos los obreros de un tipo en particular,
        ///                    3: Retorna todos los obreros de un patio en particular por el proyecto y por fecha,
        ///                    4: Retorna todos los obreros de un tipo y un patio en particular por el proyecto y por fechas</param>
        /// <param name="TipoObrero">Tipo de obrero en base al campo TipoObrero de la tabla Sam3_TipoObrero</param>
        /// <returns>listado de obreros</returns>
        public object ObtenerObrero(int idProyecto, int tipo, string TipoObrero)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_Obrero_Result> lista = ctx.Sam3_Steelgo_Get_Obrero(tipo, TipoObrero, idProyecto, null, null).ToList();
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
        public object InsertarObrero(Sam3_Obrero Obrero, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ctx.Sam3_Cat_Obrero(2, 0, usuario.UsuarioID, Obrero.TipoObreroID, Obrero.Codigo, Obrero.NumeroEmpleado);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
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

        public object EliminarObrero(int ObreroID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Sam3_Cat_Obrero(4, ObreroID, usuario.UsuarioID, null, null, null);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
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

        public object ModificarObrero(Sam3_Obrero Obrero, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Sam3_Cat_Obrero(3, Obrero.ObreroID, usuario.UsuarioID, Obrero.TipoObreroID, Obrero.Codigo, Obrero.NumeroEmpleado);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
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


    }
}