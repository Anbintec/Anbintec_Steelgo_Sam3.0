using BackEndSAM.Models.Sam3General.OpcionValidacion;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.OpcionValidacion
{
    public class OpcionValidacionBD
    {
        private static readonly object _mutex = new object();
        private static OpcionValidacionBD _instance;

        public static OpcionValidacionBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OpcionValidacionBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtenerListaOpcionValidacion(string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_OpcionValidacion_Result> result = ctx.Sam3_Steelgo_Get_OpcionValidacion(Lenguaje).ToList();
                    List<DetalleOpcionValidacion> listaDetalle = new List<DetalleOpcionValidacion>();
                    listaDetalle.Add(new DetalleOpcionValidacion());

                    foreach (Sam3_Steelgo_Get_OpcionValidacion_Result item in result)
                    {
                        listaDetalle.Add(new DetalleOpcionValidacion {
                            OpcionValidacionID = item.OpcionValidacionID,
                            Descripcion = item.Nombre
                        });
                    }

                    return listaDetalle;

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